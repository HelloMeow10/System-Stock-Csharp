using System.Data;
using Contracts;
using Microsoft.Data.SqlClient;

namespace DataAccess.Repositories;

public class SqlStockRepository : IStockRepository
{
    private readonly DatabaseConnectionFactory _connectionFactory;
    public SqlStockRepository(DatabaseConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;

    public async Task<IEnumerable<StockMovementDto>> GetMovementsAsync(DateOnly from, DateOnly to, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);
        using var cmd = new SqlCommand("sp_ReporteMovimientosStock", (SqlConnection)conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.Add(new SqlParameter("@fechaDesde", SqlDbType.Date) { Value = from.ToDateTime(TimeOnly.MinValue) });
        cmd.Parameters.Add(new SqlParameter("@fechaHasta", SqlDbType.Date) { Value = to.ToDateTime(TimeOnly.MinValue) });

        var list = new List<StockMovementDto>();
        using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(new StockMovementDto(
                Id: reader.GetInt32(0),
                Fecha: reader.GetDateTime(1),
                Usuario: reader.GetString(2),
                Producto: reader.GetString(3),
                TipoMovimiento: reader.GetString(4),
                Cantidad: reader.GetInt32(5)
            ));
        }
        return list;
    }

    public async Task IngresoMercaderiaAsync(IngresoMercaderiaRequest request, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);
        using var cmd = new SqlCommand("sp_IngresoMercaderia", (SqlConnection)conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.Add(new SqlParameter("@id_producto", SqlDbType.Int) { Value = request.ProductoId });
        cmd.Parameters.Add(new SqlParameter("@id_usuario", SqlDbType.Int) { Value = request.UsuarioId });
        cmd.Parameters.Add(new SqlParameter("@lote", SqlDbType.VarChar, 50) { Value = (object?)request.Lote ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@cantidad", SqlDbType.Int) { Value = request.Cantidad });
        cmd.Parameters.Add(new SqlParameter("@stockMinimo", SqlDbType.Int) { Value = (object?)request.StockMinimo ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@stockIdeal", SqlDbType.Int) { Value = (object?)request.StockIdeal ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@stockMaximo", SqlDbType.Int) { Value = (object?)request.StockMaximo ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@tipoStock", SqlDbType.VarChar, 20) { Value = (object?)request.TipoStock ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@puntoReposicion", SqlDbType.Int) { Value = (object?)request.PuntoReposicion ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@fechaVencimiento", SqlDbType.Date) { Value = request.FechaVencimiento.HasValue ? request.FechaVencimiento.Value : DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@estadoHabilitaciones", SqlDbType.VarChar, 50) { Value = (object?)request.EstadoHabilitaciones ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@id_movimientosStock", SqlDbType.Int) { Value = (object?)request.MovimientoStockId ?? DBNull.Value });

        await cmd.ExecuteNonQueryAsync(ct);
    }

    public async Task CreateScrapAsync(CreateScrapRequest request, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);
        using var cmd = new SqlCommand("sp_MoverStockAScrap", (SqlConnection)conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.Add(new SqlParameter("@id_producto", SqlDbType.Int) { Value = request.ProductoId });
        cmd.Parameters.Add(new SqlParameter("@cantidad", SqlDbType.Int) { Value = request.Cantidad });
        cmd.Parameters.Add(new SqlParameter("@id_usuario", SqlDbType.Int) { Value = request.UsuarioId });
        cmd.Parameters.Add(new SqlParameter("@id_motivoScrap", SqlDbType.Int) { Value = request.MotivoScrapId });
        await cmd.ExecuteNonQueryAsync(ct);
    }

    public async Task<IEnumerable<ScrapReportItemDto>> GetScrapAsync(DateOnly from, DateOnly to, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);
        using var cmd = new SqlCommand("sp_ReporteScrap", (SqlConnection)conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.Add(new SqlParameter("@fechaDesde", SqlDbType.Date) { Value = from.ToDateTime(TimeOnly.MinValue) });
        cmd.Parameters.Add(new SqlParameter("@fechaHasta", SqlDbType.Date) { Value = to.ToDateTime(TimeOnly.MinValue) });

        var list = new List<ScrapReportItemDto>();
        using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(new ScrapReportItemDto(
                Id: reader.GetInt32(0),
                Fecha: reader.GetDateTime(1),
                Usuario: reader.GetString(2),
                Producto: reader.GetString(3),
                Motivo: reader.GetString(4),
                Cantidad: reader.GetInt32(5)
            ));
        }
        return list;
    }

    public async Task<IEnumerable<ScrapReasonDto>> GetScrapReasonsAsync(CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);
        using var cmd = new SqlCommand("sp_get_motivos_scrap", (SqlConnection)conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        var list = new List<ScrapReasonDto>();
        using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(new ScrapReasonDto(
                Id: reader.GetInt32(0),
                Dano: reader.GetBoolean(1),
                Vencido: reader.GetBoolean(2),
                Obsoleto: reader.GetBoolean(3),
                MalaCalidad: reader.GetBoolean(4)
            ));
        }
        return list;
    }

    public async Task<IEnumerable<StockItemDto>> GetStockAsync(CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        await EnsureOpenAsync(conn, ct);
        using var cmd = new SqlCommand("sp_get_stock", (SqlConnection)conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        
        var list = new List<StockItemDto>();
        using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(new StockItemDto
            {
                Id = reader.GetInt32(0),
                Product = reader.GetString(1),
                Warehouse = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Quantity = reader.GetInt32(3),
                Min = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                Max = reader.IsDBNull(5) ? 0 : reader.GetInt32(5)
            });
        }
        return list;
    }

    private static async Task EnsureOpenAsync(IDbConnection connection, CancellationToken ct)
    {
        if (connection is SqlConnection sqlConn && sqlConn.State != ConnectionState.Open)
        {
            await sqlConn.OpenAsync(ct);
        }
    }
}
