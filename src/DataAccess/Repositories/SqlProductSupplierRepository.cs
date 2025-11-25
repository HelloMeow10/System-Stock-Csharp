using System.Data;
using Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class SqlProductSupplierRepository : IProductSupplierRepository
    {
        private readonly DatabaseConnectionFactory _connectionFactory;
        private readonly ILogger<SqlProductSupplierRepository> _logger;

        public SqlProductSupplierRepository(DatabaseConnectionFactory connectionFactory, ILogger<SqlProductSupplierRepository> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
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

        private async Task<IEnumerable<ProductSupplierRelationDto>> ExecuteReaderAsync(string sp, Action<SqlParameterCollection> addParams, Func<SqlDataReader, ProductSupplierRelationDto> map)
        {
            var list = new List<ProductSupplierRelationDto>();
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

        public async Task RelateAsync(int productId, int supplierId, decimal? precioCompra = null, int? tiempoEntrega = null, decimal? descuento = null)
        {
            await ExecuteNonQueryAsync("sp_RelacionarProductoProveedor", p =>
            {
                p.AddWithValue("@id_proveedor", supplierId);
                p.AddWithValue("@id_producto", productId);
                p.AddWithValue("@precioCompra", (object?)precioCompra ?? DBNull.Value);
                p.AddWithValue("@tiempoEntrega", (object?)tiempoEntrega ?? DBNull.Value);
                p.AddWithValue("@descuento", (object?)descuento ?? DBNull.Value);
            });
        }

        public async Task<IEnumerable<ProductSupplierRelationDto>> GetProductsBySupplierAsync(int supplierId)
        {
            return await ExecuteReaderAsync("sp_ConsultarProductosPorProveedor", p => p.AddWithValue("@id_proveedor", supplierId), reader =>
            {
                return new ProductSupplierRelationDto
                {
                    ProductoId = reader.GetInt32(reader.GetOrdinal("id_producto")),
                    ProveedorId = supplierId,
                    ProductoCodigo = reader["codigo"] as string ?? string.Empty,
                    ProductoNombre = reader["nombre"] as string ?? string.Empty,
                    PrecioCompra = reader["precioCompra"] != DBNull.Value ? Convert.ToDecimal(reader["precioCompra"]) : null,
                    TiempoEntrega = reader["tiempoEntrega"] != DBNull.Value ? Convert.ToInt32(reader["tiempoEntrega"]) : null,
                    Descuento = reader["descuento"] != DBNull.Value ? Convert.ToDecimal(reader["descuento"]) : null
                };
            });
        }

        public async Task<IEnumerable<ProductSupplierRelationDto>> GetSuppliersByProductAsync(int productId)
        {
            return await ExecuteReaderAsync("sp_ConsultarProveedoresPorProducto", p => p.AddWithValue("@id_producto", productId), reader =>
            {
                return new ProductSupplierRelationDto
                {
                    ProveedorId = reader.GetInt32(reader.GetOrdinal("id_proveedor")),
                    ProductoId = productId,
                    ProveedorRazonSocial = reader["razonSocial"] as string ?? string.Empty,
                    ProveedorNombre = reader["razonSocial"] as string ?? string.Empty,
                    PrecioCompra = reader["precioCompra"] != DBNull.Value ? Convert.ToDecimal(reader["precioCompra"]) : null,
                    TiempoEntrega = reader["tiempoEntrega"] != DBNull.Value ? Convert.ToInt32(reader["tiempoEntrega"]) : null,
                    Descuento = reader["descuento"] != DBNull.Value ? Convert.ToDecimal(reader["descuento"]) : null
                };
            });
        }

        public async Task DeleteRelationAsync(int productId, int supplierId)
        {
            await ExecuteNonQueryAsync("sp_DeleteRelacionProductoProveedor", p =>
            {
                p.AddWithValue("@id_producto", productId);
                p.AddWithValue("@id_proveedor", supplierId);
            });
        }
        public async Task UpdateRelationAsync(int productId, int supplierId, decimal? precioCompra = null, int? tiempoEntrega = null, decimal? descuento = null)
        {
            await ExecuteNonQueryAsync("sp_ActualizarRelacionProductoProveedor", p =>
            {
                p.AddWithValue("@id_producto", productId);
                p.AddWithValue("@id_proveedor", supplierId);
                p.AddWithValue("@precioCompra", (object?)precioCompra ?? DBNull.Value);
                p.AddWithValue("@tiempoEntrega", (object?)tiempoEntrega ?? DBNull.Value);
                p.AddWithValue("@descuento", (object?)descuento ?? DBNull.Value);
            });
        }
    }
}
