using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using DataAccess;

namespace Services.Controllers.V1
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly DatabaseConnectionFactory _connectionFactory;
        public OrdersController(DatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // Basic orders list mapped from Ventas
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null, [FromQuery] int? clientId = null)
        {
            using var conn = _connectionFactory.CreateConnection();
            await EnsureOpenAsync(conn, HttpContext.RequestAborted);
                        var sql = @"
                                SELECT v.id_venta AS Id,
                                             c.nombre AS Cliente,
                                             v.fecha AS Fecha,
                                             v.montoTotal AS Total,
                                             CASE 
                                                     WHEN ev.cancelada = 1 THEN 'Cancelada'
                                                     WHEN ev.entregada = 1 THEN 'Entregada'
                                                     WHEN ev.facturada = 1 THEN 'Facturada'
                                                     ELSE 'Registrado'
                                             END AS Estado
                                FROM Ventas v
                                INNER JOIN Clientes c ON v.id_cliente = c.id_cliente
                                LEFT JOIN EstadoVentas ev ON v.id_estadoVentas = ev.id_estadoVentas
                                WHERE (@start IS NULL OR v.fecha >= @start)
                                    AND (@end IS NULL OR v.fecha <= @end)
                                    AND (@clientId IS NULL OR v.id_cliente = @clientId)
                                ORDER BY v.fecha DESC";
            using var cmd = new SqlCommand(sql, (SqlConnection)conn);
            cmd.Parameters.AddWithValue("@start", (object?)start ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@end", (object?)end ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@clientId", (object?)clientId ?? DBNull.Value);

            var list = new List<object>();
            using var reader = await cmd.ExecuteReaderAsync(HttpContext.RequestAborted);
            while (await reader.ReadAsync(HttpContext.RequestAborted))
            {
                list.Add(new
                {
                    Id = reader.GetInt32(0),
                    Cliente = reader.GetString(1),
                    Fecha = reader.GetDateTime(2),
                    Total = reader.GetDecimal(3),
                    Estado = reader.GetString(4)
                });
            }
            return Ok(list);
        }

        private static async Task EnsureOpenAsync(IDbConnection connection, CancellationToken ct)
        {
            if (connection is SqlConnection sqlConn && sqlConn.State != ConnectionState.Open)
            {
                await sqlConn.OpenAsync(ct);
            }
        }
    }
}
