using Contracts;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DataAccess.Repositories
{
    public class SqlInvoicingRepository : IInvoicingRepository
    {
        private readonly DatabaseConnectionFactory _connectionFactory;

        public SqlInvoicingRepository(DatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        private static async Task EnsureOpenAsync(IDbConnection connection, CancellationToken ct)
        {
            if (connection is SqlConnection sqlConn && sqlConn.State != ConnectionState.Open)
            {
                await sqlConn.OpenAsync(ct);
            }
        }

        public async Task<IEnumerable<InvoiceDto>> GetPurchaseInvoicesAsync(DateTime? from, DateTime? to, int? supplierId, CancellationToken ct = default)
        {
            using var conn = _connectionFactory.CreateConnection();
            await EnsureOpenAsync(conn, ct);

            var sql = @"
                SELECT 
                    f.id_factura,
                    'Compra' as Tipo,
                    'Factura' as TipoDocumento,
                    f.numeroFactura,
                    f.fecha,
                    p.nombre as EntidadNombre,
                    f.total,
                    'Emitida' as Estado
                FROM FacturaCompra f
                JOIN Proveedores p ON f.id_proveedor = p.id_proveedor
                WHERE 1=1";

            if (from.HasValue) sql += " AND f.fecha >= @from";
            if (to.HasValue) sql += " AND f.fecha <= @to";
            if (supplierId.HasValue) sql += " AND f.id_proveedor = @supplierId";

            using var cmd = new SqlCommand(sql, (SqlConnection)conn);
            if (from.HasValue) cmd.Parameters.AddWithValue("@from", from.Value);
            if (to.HasValue) cmd.Parameters.AddWithValue("@to", to.Value);
            if (supplierId.HasValue) cmd.Parameters.AddWithValue("@supplierId", supplierId.Value);

            var list = new List<InvoiceDto>();
            using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                list.Add(new InvoiceDto
                {
                    Id = reader.GetInt32(0),
                    Tipo = reader.GetString(1),
                    TipoDocumento = reader.GetString(2),
                    Numero = reader.GetString(3),
                    Fecha = reader.GetDateTime(4),
                    EntidadNombre = reader.GetString(5),
                    Total = reader.GetDecimal(6),
                    Estado = reader.GetString(7)
                });
            }
            return list;
        }

        public async Task CreatePurchaseInvoiceAsync(CreatePurchaseInvoiceRequest request, CancellationToken ct = default)
        {
            using var conn = _connectionFactory.CreateConnection();
            await EnsureOpenAsync(conn, ct);
            using var transaction = (SqlTransaction)await ((SqlConnection)conn).BeginTransactionAsync(ct);

            try
            {
                // 1. Insert Header
                var sqlHeader = @"
                    INSERT INTO FacturaCompra (id_proveedor, numeroFactura, fecha, total, visadoAlmacen)
                    VALUES (@id_proveedor, @numeroFactura, @fecha, @total, 0);
                    SELECT SCOPE_IDENTITY();";

                int invoiceId;
                decimal total = request.Items.Sum(i => i.Cantidad * i.PrecioUnitario); // Simple calc, ignoring taxes for now if not in DB

                using (var cmd = new SqlCommand(sqlHeader, (SqlConnection)conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@id_proveedor", request.ProveedorId);
                    cmd.Parameters.AddWithValue("@numeroFactura", request.Numero);
                    cmd.Parameters.AddWithValue("@fecha", request.Fecha);
                    cmd.Parameters.AddWithValue("@total", total);
                    invoiceId = Convert.ToInt32(await cmd.ExecuteScalarAsync(ct));
                }

                // 2. Insert Details
                var sqlDetail = @"
                    INSERT INTO DetalleFacturaCompra (id_factura, id_producto, cantidad, precioUnitario)
                    VALUES (@id_factura, @id_producto, @cantidad, @precioUnitario)";

                foreach (var item in request.Items)
                {
                    using (var cmd = new SqlCommand(sqlDetail, (SqlConnection)conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id_factura", invoiceId);
                        cmd.Parameters.AddWithValue("@id_producto", item.ProductoId);
                        cmd.Parameters.AddWithValue("@cantidad", item.Cantidad);
                        cmd.Parameters.AddWithValue("@precioUnitario", item.PrecioUnitario);
                        await cmd.ExecuteNonQueryAsync(ct);
                    }
                }

                await transaction.CommitAsync(ct);
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

        public async Task<IEnumerable<InvoiceDto>> GetSalesInvoicesAsync(DateTime? from, DateTime? to, int? customerId, CancellationToken ct = default)
        {
            using var conn = _connectionFactory.CreateConnection();
            await EnsureOpenAsync(conn, ct);

            var sql = @"
                SELECT 
                    v.id_venta,
                    'Venta' as Tipo,
                    v.tipoDocumento,
                    v.numeroDocumento,
                    v.fecha,
                    c.nombre as EntidadNombre,
                    v.montoTotal,
                    CASE WHEN v.id_estadoVentas = 1 THEN 'Emitida' ELSE 'Desconocido' END as estado
                FROM Ventas v
                JOIN Clientes c ON v.id_cliente = c.id_cliente
                -- LEFT JOIN EstadoVentas ev ON v.id_estadoVentas = ev.id_estadoVentas
                WHERE v.tipoDocumento IN ('Factura', 'NotaCredito', 'NotaDebito')";

            if (from.HasValue) sql += " AND v.fecha >= @from";
            if (to.HasValue) sql += " AND v.fecha <= @to";
            if (customerId.HasValue) sql += " AND v.id_cliente = @customerId";

            using var cmd = new SqlCommand(sql, (SqlConnection)conn);
            if (from.HasValue) cmd.Parameters.AddWithValue("@from", from.Value);
            if (to.HasValue) cmd.Parameters.AddWithValue("@to", to.Value);
            if (customerId.HasValue) cmd.Parameters.AddWithValue("@customerId", customerId.Value);

            var list = new List<InvoiceDto>();
            using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                list.Add(new InvoiceDto
                {
                    Id = reader.GetInt32(0),
                    Tipo = reader.GetString(1),
                    TipoDocumento = reader.IsDBNull(2) ? "Factura" : reader.GetString(2),
                    Numero = reader.IsDBNull(3) ? "-" : reader.GetString(3),
                    Fecha = reader.GetDateTime(4),
                    EntidadNombre = reader.GetString(5),
                    Total = reader.GetDecimal(6),
                    Estado = reader.IsDBNull(7) ? "Desconocido" : reader.GetString(7)
                });
            }
            return list;
        }

        public async Task CreateSalesInvoiceAsync(CreateSalesInvoiceRequest request, CancellationToken ct = default)
        {
            using var conn = _connectionFactory.CreateConnection();
            await EnsureOpenAsync(conn, ct);
            using var transaction = (SqlTransaction)await ((SqlConnection)conn).BeginTransactionAsync(ct);

            try
            {
                // 1. Insert Header
                // Assuming status 1 is 'Emitida' or similar
                var sqlHeader = @"
                    INSERT INTO Ventas (id_cliente, fecha, tipoDocumento, numeroDocumento, montoTotal, id_estadoVentas)
                    VALUES (@id_cliente, @fecha, @tipoDocumento, @numeroDocumento, @montoTotal, 1);
                    SELECT SCOPE_IDENTITY();";

                int saleId;
                decimal total = request.Items.Sum(i => i.Cantidad * i.PrecioUnitario);

                using (var cmd = new SqlCommand(sqlHeader, (SqlConnection)conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@id_cliente", request.ClienteId);
                    cmd.Parameters.AddWithValue("@fecha", request.Fecha);
                    cmd.Parameters.AddWithValue("@tipoDocumento", request.TipoDocumento);
                    cmd.Parameters.AddWithValue("@numeroDocumento", "0001-" + DateTime.Now.Ticks.ToString().Substring(10)); // Auto-gen for now
                    cmd.Parameters.AddWithValue("@montoTotal", total);
                    saleId = Convert.ToInt32(await cmd.ExecuteScalarAsync(ct));
                }

                // 2. Insert Details
                var sqlDetail = @"
                    INSERT INTO DetalleVentas (id_venta, id_producto, cantidad, precioUnitario, subtotal)
                    VALUES (@id_venta, @id_producto, @cantidad, @precioUnitario, @subtotal)";

                foreach (var item in request.Items)
                {
                    using (var cmd = new SqlCommand(sqlDetail, (SqlConnection)conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id_venta", saleId);
                        cmd.Parameters.AddWithValue("@id_producto", item.ProductoId);
                        cmd.Parameters.AddWithValue("@cantidad", item.Cantidad);
                        cmd.Parameters.AddWithValue("@precioUnitario", item.PrecioUnitario);
                        cmd.Parameters.AddWithValue("@subtotal", item.Cantidad * item.PrecioUnitario);
                        await cmd.ExecuteNonQueryAsync(ct);
                    }
                }

                await transaction.CommitAsync(ct);
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }
        public async Task<IEnumerable<PaymentMethodDto>> GetPaymentMethodsAsync(CancellationToken ct = default)
        {
            using var conn = _connectionFactory.CreateConnection();
            await EnsureOpenAsync(conn, ct);
            using var cmd = new SqlCommand("SELECT id_formaPago, descripcion FROM FormaPago", (SqlConnection)conn);
            var list = new List<PaymentMethodDto>();
            using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                list.Add(new PaymentMethodDto(reader.GetInt32(0), reader.GetString(1)));
            }
            return list;
        }
    }
}
