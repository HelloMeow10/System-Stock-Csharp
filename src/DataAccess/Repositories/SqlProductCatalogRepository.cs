using System.Data;
using Contracts;
using Microsoft.Data.SqlClient;

namespace DataAccess.Repositories;

public class SqlProductCatalogRepository : IProductCatalogRepository
{
    private readonly DatabaseConnectionFactory _connectionFactory;

    public SqlProductCatalogRepository(DatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<BrandDto>> GetBrandsAsync(CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);
        using var cmd = new SqlCommand("sp_get_marcas", (SqlConnection)conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        var list = new List<BrandDto>();
        using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(new BrandDto(reader.GetInt32(0), reader.GetString(1)));
        }
        return list;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);
        using var cmd = new SqlCommand("sp_get_categorias", (SqlConnection)conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        var list = new List<CategoryDto>();
        using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(new CategoryDto(reader.GetInt32(0), reader.GetString(1)));
        }
        return list;
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);
        using var cmd = new SqlCommand("sp_get_product_by_id_with_stock", (SqlConnection)conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });
        using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return new ProductDto(
                Id: reader.GetInt32(0),
                Codigo: reader.GetString(1),
                Nombre: reader.GetString(2),
                Categoria: reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                Marca: reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                Precio: reader.GetDecimal(5),
                StockActual: reader.GetInt32(6),
                StockMinimo: reader.GetInt32(7),
                StockMaximo: reader.GetInt32(8),
                UnidadMedida: reader.FieldCount > 9 && !reader.IsDBNull(9) ? reader.GetString(9) : string.Empty,
                Peso: reader.FieldCount > 10 && !reader.IsDBNull(10) ? reader.GetDecimal(10) : 0m,
                Volumen: reader.FieldCount > 11 && !reader.IsDBNull(11) ? reader.GetDecimal(11) : 0m,
                PuntoReposicion: reader.FieldCount > 12 && !reader.IsDBNull(12) ? reader.GetInt32(12) : 0,
                DiasVencimiento: reader.FieldCount > 13 && !reader.IsDBNull(13) ? reader.GetInt32(13) : 0,
                LoteObligatorio: reader.FieldCount > 14 && !reader.IsDBNull(14) && reader.GetBoolean(14),
                ControlVencimiento: reader.FieldCount > 15 && !reader.IsDBNull(15) && reader.GetBoolean(15)
            );
        }
        return null;
    }

    public async Task<(IEnumerable<ProductDto> Items, int TotalCount)> GetProductsAsync(string? search, int pageNumber, int pageSize, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);
        var items = new List<ProductDto>();
        int total;

        using (var cmd = new SqlCommand("sp_get_products_with_stock_paged", (SqlConnection)conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Search", SqlDbType.NVarChar, 200) { Value = (object?)search ?? string.Empty });
            cmd.Parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber });
            cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize });

            using var reader = await cmd.ExecuteReaderAsync(ct);

            // Primer result set: items
            while (await reader.ReadAsync(ct))
            {
                items.Add(new ProductDto(
                    Id: reader.GetInt32(0),
                    Codigo: reader.GetString(1),
                    Nombre: reader.GetString(2),
                    Categoria: reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    Marca: reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    Precio: reader.GetDecimal(5),
                    StockActual: reader.GetInt32(6),
                    StockMinimo: reader.GetInt32(7),
                    StockMaximo: reader.GetInt32(8),
                    UnidadMedida: reader.FieldCount > 9 && !reader.IsDBNull(9) ? reader.GetString(9) : string.Empty,
                    Peso: reader.FieldCount > 10 && !reader.IsDBNull(10) ? reader.GetDecimal(10) : 0m,
                    Volumen: reader.FieldCount > 11 && !reader.IsDBNull(11) ? reader.GetDecimal(11) : 0m,
                    PuntoReposicion: reader.FieldCount > 12 && !reader.IsDBNull(12) ? reader.GetInt32(12) : 0,
                    DiasVencimiento: reader.FieldCount > 13 && !reader.IsDBNull(13) ? reader.GetInt32(13) : 0,
                    LoteObligatorio: reader.FieldCount > 14 && !reader.IsDBNull(14) && reader.GetBoolean(14),
                    ControlVencimiento: reader.FieldCount > 15 && !reader.IsDBNull(15) && reader.GetBoolean(15)
                ));
            }

            // Segundo result set: total count
            if (await reader.NextResultAsync(ct) && await reader.ReadAsync(ct))
            {
                total = reader.GetInt32(0);
            }
            else
            {
                total = items.Count;
            }
        }

        return (items, total);
    }

    public async Task<ProductDto> AddProductAsync(CreateProductRequest request, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);

        int newId;
        using (var cmd = new SqlCommand("sp_AgregarProducto", (SqlConnection)conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@codigo", SqlDbType.VarChar, 50) { Value = request.Codigo });
            cmd.Parameters.Add(new SqlParameter("@codBarras", SqlDbType.VarChar, 50) { Value = (object?)request.CodBarras ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@nombre", SqlDbType.VarChar, 100) { Value = request.Nombre });
            cmd.Parameters.Add(new SqlParameter("@descripcion", SqlDbType.VarChar, 255) { Value = (object?)request.Descripcion ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@id_marca", SqlDbType.Int) { Value = (object?)request.IdMarca ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@precioCompra", SqlDbType.Decimal) { Value = (object?)request.PrecioCompra ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@precioVenta", SqlDbType.Decimal) { Value = (object?)request.PrecioVenta ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@estado", SqlDbType.VarChar, 50) { Value = (object?)request.Estado ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@ubicacion", SqlDbType.VarChar, 100) { Value = (object?)request.Ubicacion ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@habilitado", SqlDbType.Bit) { Value = (object?)request.Habilitado ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@id_categoria", SqlDbType.Int) { Value = (object?)request.IdCategoria ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@unidadMedida", SqlDbType.VarChar, 20) { Value = (object?)request.UnidadMedida ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@peso", SqlDbType.Decimal) { Value = (object?)request.Peso ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@volumen", SqlDbType.Decimal) { Value = (object?)request.Volumen ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@puntoReposicion", SqlDbType.Int) { Value = (object?)request.PuntoReposicion ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@diasVencimiento", SqlDbType.Int) { Value = (object?)request.DiasVencimiento ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@loteObligatorio", SqlDbType.Bit) { Value = (object?)request.LoteObligatorio ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@controlVencimiento", SqlDbType.Bit) { Value = (object?)request.ControlVencimiento ?? DBNull.Value });

            // sp_AgregarProducto no devuelve el Id, así que obtenemos el último ID autogenerado
            await cmd.ExecuteNonQueryAsync(ct);
            using var idCmd = new SqlCommand("SELECT TOP 1 id_producto FROM Productos ORDER BY id_producto DESC", (SqlConnection)conn);
            var result = await idCmd.ExecuteScalarAsync(ct);
            newId = Convert.ToInt32(result);
        }

        var prod = await GetProductByIdAsync(newId, ct);
        return prod ?? throw new InvalidOperationException("Failed to retrieve inserted product");
    }

    private static async Task EnsureOpenAsync(IDbConnection connection, CancellationToken ct)
    {
        if (connection is SqlConnection sqlConn && sqlConn.State != ConnectionState.Open)
        {
            await sqlConn.OpenAsync(ct);
        }
    }
}
