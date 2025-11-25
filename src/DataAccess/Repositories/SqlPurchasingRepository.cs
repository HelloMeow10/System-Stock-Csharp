using System.Data;
using Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories;

public class SqlPurchasingRepository : IPurchasingRepository
{
    private readonly DatabaseConnectionFactory _connectionFactory;
    private readonly ILogger<SqlPurchasingRepository> _logger;

    public SqlPurchasingRepository(DatabaseConnectionFactory connectionFactory, ILogger<SqlPurchasingRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    private async Task<T> ExecuteScalarAsync<T>(string sp, Action<SqlParameterCollection> addParams)
    {
        using var conn = (SqlConnection)_connectionFactory.CreateConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sp;
        cmd.CommandType = CommandType.StoredProcedure;
        addParams(cmd.Parameters);
        var result = await cmd.ExecuteScalarAsync();
        return result != null && result != DBNull.Value ? (T)Convert.ChangeType(result, typeof(T))! : default!;
    }

    private async Task ExecuteNonQueryAsync(string sp, Action<SqlParameterCollection> addParams)
    {
        using var conn = (SqlConnection)_connectionFactory.CreateConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sp;
        cmd.CommandType = CommandType.StoredProcedure;
        addParams(cmd.Parameters);
        await cmd.ExecuteNonQueryAsync();
    }

    private async Task<IEnumerable<T>> ExecuteReaderAsync<T>(string sp, Action<SqlParameterCollection> addParams, Func<SqlDataReader, T> map)
    {
        var list = new List<T>();
        using var conn = (SqlConnection)_connectionFactory.CreateConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sp;
        cmd.CommandType = CommandType.StoredProcedure;
        addParams(cmd.Parameters);
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(map(reader));
        }
        return list;
    }

    public Task<int> CreateQuoteAsync(int proveedorId, DateTime fecha)
        => ExecuteScalarAsync<int>("sp_RegistrarPresupuestoCompra", p =>
        {
            p.AddWithValue("@id_proveedor", proveedorId);
            p.AddWithValue("@fecha", fecha);
            p.AddWithValue("@total", 0m);
        });

    public Task AddQuoteItemAsync(int quoteId, int productoId, int cantidad, decimal precioUnitario)
        => ExecuteNonQueryAsync("sp_AgregarItemPresupuestoCompra", p =>
        {
            p.AddWithValue("@id_presupuesto", quoteId);
            p.AddWithValue("@id_producto", productoId);
            p.AddWithValue("@cantidad", cantidad);
            p.AddWithValue("@precioUnitario", precioUnitario);
        });

    public async Task<IEnumerable<PurchaseQuoteDto>> ListQuotesAsync(int? proveedorId = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        => await ExecuteReaderAsync("sp_ListarPresupuestosCompra", p =>
        {
            p.AddWithValue("@id_proveedor", (object?)proveedorId ?? DBNull.Value);
            p.AddWithValue("@fechaDesde", (object?)fechaDesde ?? DBNull.Value);
            p.AddWithValue("@fechaHasta", (object?)fechaHasta ?? DBNull.Value);
        }, r => new PurchaseQuoteDto
        {
            Id = r.GetInt32(r.GetOrdinal("id_presupuesto")),
            ProveedorId = r.GetInt32(r.GetOrdinal("id_proveedor")),
            Proveedor = r["proveedorNombre"] != DBNull.Value ? r.GetString(r.GetOrdinal("proveedorNombre")) : null,
            ProveedorCUIT = r["proveedorCUIT"] != DBNull.Value ? r.GetString(r.GetOrdinal("proveedorCUIT")) : null,
            Fecha = r.GetDateTime(r.GetOrdinal("fecha")),
            Total = r.GetDecimal(r.GetOrdinal("total")),
            Items = r.GetInt32(r.GetOrdinal("items"))
        });

    public Task<int> ConvertQuoteToOrderAsync(int quoteId, DateTime? fecha = null)
        => ExecuteScalarAsync<int>("sp_ConvertirPresupuestoAOrden", p =>
        {
            p.AddWithValue("@id_presupuesto", quoteId);
            p.AddWithValue("@fecha", (object?)fecha ?? DBNull.Value);
        });

    public Task<IEnumerable<PurchaseOrderDto>> ListOrdersAsync(int? proveedorId = null, bool? entregado = null)
        => ExecuteReaderAsync("sp_ListarOrdenesCompra", p =>
        {
            p.AddWithValue("@id_proveedor", (object?)proveedorId ?? DBNull.Value);
            p.AddWithValue("@entregado", (object?)entregado ?? DBNull.Value);
        }, r => new PurchaseOrderDto
        {
            Id = r.GetInt32(r.GetOrdinal("id_ordenCompra")),
            PresupuestoId = r.GetInt32(r.GetOrdinal("id_presupuesto")),
            ProveedorId = r.GetInt32(r.GetOrdinal("id_proveedor")),
            Proveedor = r["proveedorNombre"] != DBNull.Value ? r.GetString(r.GetOrdinal("proveedorNombre")) : null,
            ProveedorCUIT = r["proveedorCUIT"] != DBNull.Value ? r.GetString(r.GetOrdinal("proveedorCUIT")) : null,
            Fecha = r.GetDateTime(r.GetOrdinal("fecha")),
            Total = r.GetDecimal(r.GetOrdinal("total")),
            Entregado = r.GetBoolean(r.GetOrdinal("entregado")),
            Items = r.GetInt32(r.GetOrdinal("items"))
        });

    public Task AddOrderItemAsync(int orderId, int productoId, int cantidad, decimal precioUnitario)
        => ExecuteNonQueryAsync("sp_AgregarItemOrdenCompra", p =>
        {
            p.AddWithValue("@id_ordenCompra", orderId);
            p.AddWithValue("@id_producto", productoId);
            p.AddWithValue("@cantidad", cantidad);
            p.AddWithValue("@precioUnitario", precioUnitario);
        });

    public Task MarkOrderReceivedAsync(int orderId)
        => ExecuteNonQueryAsync("sp_MarcarOrdenCompraRecibida", p => p.AddWithValue("@id_ordenCompra", orderId));

    public Task<IEnumerable<PurchaseQuoteItemDto>> GetQuoteItemsAsync(int quoteId)
        => ExecuteReaderAsync("sp_ListarItemsPresupuestoCompra", p =>
        {
            p.AddWithValue("@id_presupuesto", quoteId);
        }, r => new PurchaseQuoteItemDto
        {
            Id = r.GetInt32(r.GetOrdinal("id_detalle")),
            PresupuestoId = r.GetInt32(r.GetOrdinal("id_presupuesto")),
            ProductoId = r.GetInt32(r.GetOrdinal("id_producto")),
            Cantidad = r.GetInt32(r.GetOrdinal("cantidad")),
            PrecioUnitario = r.GetDecimal(r.GetOrdinal("precioUnitario"))
        });

    public Task<IEnumerable<PurchaseOrderItemDto>> GetOrderItemsAsync(int orderId)
        => ExecuteReaderAsync("sp_ListarItemsOrdenCompra", p =>
        {
            p.AddWithValue("@id_ordenCompra", orderId);
        }, r => new PurchaseOrderItemDto
        {
            Id = r.GetInt32(r.GetOrdinal("id_detalle")),
            OrdenCompraId = r.GetInt32(r.GetOrdinal("id_ordenCompra")),
            ProductoId = r.GetInt32(r.GetOrdinal("id_producto")),
            Cantidad = r.GetInt32(r.GetOrdinal("cantidad")),
            PrecioUnitario = r.GetDecimal(r.GetOrdinal("precioUnitario")),
            RecibidoCantidad = r["recibidoCantidad"] != DBNull.Value ? r.GetInt32(r.GetOrdinal("recibidoCantidad")) : null
        });

    public Task DeleteQuoteItemAsync(int itemId)
        => ExecuteNonQueryAsync("sp_EliminarItemPresupuestoCompra", p =>
        {
            p.AddWithValue("@id_detalle", itemId);
        });

    public Task UpdateQuoteItemAsync(int itemId, int cantidad, decimal precioUnitario)
        => ExecuteNonQueryAsync("sp_ActualizarItemPresupuestoCompra", p =>
        {
            p.AddWithValue("@id_detalle", itemId);
            p.AddWithValue("@cantidad", cantidad);
            p.AddWithValue("@precioUnitario", precioUnitario);
        });

    public Task DeleteOrderItemAsync(int itemId)
        => ExecuteNonQueryAsync("sp_EliminarItemOrdenCompra", p =>
        {
            p.AddWithValue("@id_detalle", itemId);
        });

    public Task UpdateOrderItemAsync(int itemId, int cantidad, decimal precioUnitario)
        => ExecuteNonQueryAsync("sp_ActualizarItemOrdenCompra", p =>
        {
            p.AddWithValue("@id_detalle", itemId);
            p.AddWithValue("@cantidad", cantidad);
            p.AddWithValue("@precioUnitario", precioUnitario);
        });

    public async Task<PurchaseOrderDto?> GetOrderByIdAsync(int id)
    {
        string sql = @"
            SELECT 
                o.id_ordenCompra, o.id_presupuesto, o.id_proveedor, 
                p.nombre AS proveedorNombre, p.CUIT AS proveedorCUIT,
                o.fecha, o.total, o.entregado,
                (SELECT COUNT(*) FROM DetalleOrdenCompra d WHERE d.id_ordenCompra = o.id_ordenCompra) AS items
            FROM OrdenCompra o
            LEFT JOIN Proveedores p ON p.id_proveedor = o.id_proveedor
            WHERE o.id_ordenCompra = @id";

        var list = await ExecuteReaderSqlAsync(sql, p => p.AddWithValue("@id", id), r => new PurchaseOrderDto
        {
            Id = r.GetInt32(r.GetOrdinal("id_ordenCompra")),
            PresupuestoId = r.GetInt32(r.GetOrdinal("id_presupuesto")),
            ProveedorId = r.GetInt32(r.GetOrdinal("id_proveedor")),
            Proveedor = r["proveedorNombre"] != DBNull.Value ? r.GetString(r.GetOrdinal("proveedorNombre")) : null,
            ProveedorCUIT = r["proveedorCUIT"] != DBNull.Value ? r.GetString(r.GetOrdinal("proveedorCUIT")) : null,
            Fecha = r.GetDateTime(r.GetOrdinal("fecha")),
            Total = r.GetDecimal(r.GetOrdinal("total")),
            Entregado = r.GetBoolean(r.GetOrdinal("entregado")),
            Items = r.GetInt32(r.GetOrdinal("items"))
        });

        return list.FirstOrDefault();
    }

    private async Task<IEnumerable<T>> ExecuteReaderSqlAsync<T>(string sql, Action<SqlParameterCollection> addParams, Func<SqlDataReader, T> map)
    {
        var list = new List<T>();
        using var conn = (SqlConnection)_connectionFactory.CreateConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.CommandType = CommandType.Text;
        addParams(cmd.Parameters);
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(map(reader));
        }
        return list;
    }

    public async Task<IEnumerable<PurchaseReportDto>> GetPurchaseReportAsync(DateTime? start, DateTime? end)
    {
        string sql = @"
            SELECT 
                o.id_ordenCompra, o.fecha, p.razonSocial AS Proveedor,
                pr.nombre AS Producto,
                d.cantidad, d.precioUnitario, (d.cantidad * d.precioUnitario) AS subtotal,
                o.entregado
            FROM OrdenCompra o
            INNER JOIN DetalleOrdenCompra d ON o.id_ordenCompra = d.id_ordenCompra
            INNER JOIN Proveedores p ON o.id_proveedor = p.id_proveedor
            INNER JOIN Productos pr ON d.id_producto = pr.id_producto
            WHERE (@start IS NULL OR o.fecha >= @start)
              AND (@end IS NULL OR o.fecha <= @end)
            ORDER BY o.fecha DESC";

        return await ExecuteReaderSqlAsync(sql, p =>
        {
            p.AddWithValue("@start", (object?)start ?? DBNull.Value);
            p.AddWithValue("@end", (object?)end ?? DBNull.Value);
        }, r => new PurchaseReportDto(
            IdOrden: r.GetInt32(r.GetOrdinal("id_ordenCompra")),
            Fecha: r.GetDateTime(r.GetOrdinal("fecha")),
            Proveedor: r.GetString(r.GetOrdinal("Proveedor")),
            Producto: r.GetString(r.GetOrdinal("Producto")),
            Cantidad: r.GetInt32(r.GetOrdinal("cantidad")),
            PrecioUnitario: r.GetDecimal(r.GetOrdinal("precioUnitario")),
            Subtotal: r.GetDecimal(r.GetOrdinal("subtotal")),
            Entregado: r.GetBoolean(r.GetOrdinal("entregado"))
        ));
    }
}
