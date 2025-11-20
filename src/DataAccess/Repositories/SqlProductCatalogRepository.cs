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
        const string sql = @"SELECT id_marca AS Id, marca AS Nombre
                             FROM MarcasProducto
                             WHERE estado IS NULL OR estado = 'Habilitado'";
        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);
        using var cmd = new SqlCommand(sql, (SqlConnection)conn);
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
        const string sql = @"SELECT id_categoria AS Id, categoria AS Nombre
                             FROM CategoriasProducto
                             WHERE estado IS NULL OR estado = 'Habilitado'";
        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);
        using var cmd = new SqlCommand(sql, (SqlConnection)conn);
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
        const string sql = @"
            WITH StockAgg AS (
                SELECT s.id_producto,
                       SUM(COALESCE(s.stock,0)) AS StockActual,
                       MAX(COALESCE(s.stockMinimo,0)) AS StockMinimo,
                       MAX(COALESCE(s.stockMaximo,0)) AS StockMaximo
                FROM Stock s
                GROUP BY s.id_producto
            )
            SELECT p.id_producto,
                   p.codigo,
                   p.nombre,
                   c.categoria,
                   m.marca,
                   COALESCE(p.precioVenta, 0) AS precioVenta,
                   COALESCE(sa.StockActual, 0) AS StockActual,
                   COALESCE(sa.StockMinimo, 0) AS StockMinimo,
                   COALESCE(sa.StockMaximo, 0) AS StockMaximo
            FROM Productos p
            LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
            LEFT JOIN MarcasProducto m ON p.id_marca = m.id_marca
            LEFT JOIN StockAgg sa ON sa.id_producto = p.id_producto
            WHERE p.id_producto = @id;";

        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);
        using var cmd = new SqlCommand(sql, (SqlConnection)conn);
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
                StockMaximo: reader.GetInt32(8)
            );
        }
        return null;
    }

    public async Task<(IEnumerable<ProductDto> Items, int TotalCount)> GetProductsAsync(string? search, int pageNumber, int pageSize, CancellationToken ct = default)
    {
        // Use parameterized search and OFFSET/FETCH for paging
        const string baseCte = @"
            WITH StockAgg AS (
                SELECT s.id_producto,
                       SUM(COALESCE(s.stock,0)) AS StockActual,
                       MAX(COALESCE(s.stockMinimo,0)) AS StockMinimo,
                       MAX(COALESCE(s.stockMaximo,0)) AS StockMaximo
                FROM Stock s
                GROUP BY s.id_producto
            )";

        var where = "";
        if (!string.IsNullOrWhiteSpace(search))
        {
            where = @" WHERE p.codigo LIKE @like OR p.nombre LIKE @like OR c.categoria LIKE @like OR m.marca LIKE @like ";
        }

        var sqlData = $@"{baseCte}
            SELECT p.id_producto,
                   p.codigo,
                   p.nombre,
                   c.categoria,
                   m.marca,
                   COALESCE(p.precioVenta, 0) AS precioVenta,
                   COALESCE(sa.StockActual, 0) AS StockActual,
                   COALESCE(sa.StockMinimo, 0) AS StockMinimo,
                   COALESCE(sa.StockMaximo, 0) AS StockMaximo
            FROM Productos p
            LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
            LEFT JOIN MarcasProducto m ON p.id_marca = m.id_marca
            LEFT JOIN StockAgg sa ON sa.id_producto = p.id_producto
            {where}
            ORDER BY p.id_producto
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;";

        var sqlCount = $@"SELECT COUNT(1)
            FROM Productos p
            LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
            LEFT JOIN MarcasProducto m ON p.id_marca = m.id_marca
            {(string.IsNullOrWhiteSpace(search) ? string.Empty : " WHERE p.codigo LIKE @like OR p.nombre LIKE @like OR c.categoria LIKE @like OR m.marca LIKE @like ")};";

        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);

        // Count first
        int total;
        using (var countCmd = new SqlCommand(sqlCount, (SqlConnection)conn))
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                countCmd.Parameters.Add(new SqlParameter("@like", SqlDbType.NVarChar, 200) { Value = $"%{search}%" });
            }
            total = (int)await countCmd.ExecuteScalarAsync(ct);
        }

        var items = new List<ProductDto>();
        using (var dataCmd = new SqlCommand(sqlData, (SqlConnection)conn))
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                dataCmd.Parameters.Add(new SqlParameter("@like", SqlDbType.NVarChar, 200) { Value = $"%{search}%" });
            }
            dataCmd.Parameters.Add(new SqlParameter("@offset", SqlDbType.Int) { Value = (pageNumber - 1) * pageSize });
            dataCmd.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.Int) { Value = pageSize });

            using var reader = await dataCmd.ExecuteReaderAsync(ct);
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
                    StockMaximo: reader.GetInt32(8)
                ));
            }
        }

        return (items, total);
    }

    public async Task<ProductDto> AddProductAsync(CreateProductRequest request, CancellationToken ct = default)
    {
        const string sql = @"
            INSERT INTO Productos (codigo, codBarras, nombre, descripcion, id_marca, precioCompra, precioVenta, estado, ubicacion, habilitado, id_categoria)
            OUTPUT INSERTED.id_producto
            VALUES (@codigo, @codBarras, @nombre, @descripcion, @id_marca, @precioCompra, @precioVenta, @estado, @ubicacion, @habilitado, @id_categoria);
        ";

        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);

        int newId;
        using (var cmd = new SqlCommand(sql, (SqlConnection)conn))
        {
            cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.VarChar, 50) { Value = request.Codigo });
            cmd.Parameters.Add(new SqlParameter("@codBarras", System.Data.SqlDbType.VarChar, 50) { Value = (object?)request.CodBarras ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@nombre", System.Data.SqlDbType.VarChar, 100) { Value = request.Nombre });
            cmd.Parameters.Add(new SqlParameter("@descripcion", System.Data.SqlDbType.VarChar, 255) { Value = (object?)request.Descripcion ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@id_marca", System.Data.SqlDbType.Int) { Value = (object?)request.IdMarca ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@precioCompra", System.Data.SqlDbType.Decimal) { Value = (object?)request.PrecioCompra ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@precioVenta", System.Data.SqlDbType.Decimal) { Value = (object?)request.PrecioVenta ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@estado", System.Data.SqlDbType.VarChar, 50) { Value = (object?)request.Estado ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@ubicacion", System.Data.SqlDbType.VarChar, 100) { Value = (object?)request.Ubicacion ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@habilitado", System.Data.SqlDbType.Bit) { Value = (object?)request.Habilitado ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@id_categoria", System.Data.SqlDbType.Int) { Value = (object?)request.IdCategoria ?? DBNull.Value });

            var result = await cmd.ExecuteScalarAsync(ct);
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
