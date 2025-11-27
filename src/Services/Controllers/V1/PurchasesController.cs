using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using DataAccess;

namespace Services.Controllers.V1
{
    [ApiController]
    [Route("api/v1/purchases/orders")]
    public class PurchasesController : ControllerBase
    {
        private readonly DatabaseConnectionFactory _connectionFactory;
        public PurchasesController(DatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // Orders of purchase from FacturaCompra (header) with supplier and totals
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null, [FromQuery] int? supplierId = null, [FromQuery] bool? entregado = null)
        {
            using var conn = _connectionFactory.CreateConnection();
            await EnsureOpenAsync(conn, HttpContext.RequestAborted);
            var sql = @"
                SELECT f.id_factura AS Id,
                       p.nombre AS Proveedor,
                       f.fecha AS Fecha,
                       f.total AS Total,
                       CASE WHEN f.visadoAlmacen = 1 THEN 'Entregada' ELSE 'Pendiente' END AS Estado
                FROM FacturaCompra f
                INNER JOIN Proveedores p ON f.id_proveedor = p.id_proveedor
                                WHERE (@start IS NULL OR f.fecha >= @start)
                                    AND (@end IS NULL OR f.fecha <= @end)
                                    AND (@supplierId IS NULL OR f.id_proveedor = @supplierId)
                                    AND (@entregado IS NULL OR (CASE WHEN f.visadoAlmacen = 1 THEN 1 ELSE 0 END) = CASE WHEN @entregado = 1 THEN 1 ELSE 0 END)
                ORDER BY f.fecha DESC";
            using var cmd = new SqlCommand(sql, (SqlConnection)conn);
            cmd.Parameters.AddWithValue("@start", (object?)start ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@end", (object?)end ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@supplierId", (object?)supplierId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@entregado", (object?)entregado ?? DBNull.Value);

            var list = new List<object>();
            using var reader = await cmd.ExecuteReaderAsync(HttpContext.RequestAborted);
            while (await reader.ReadAsync(HttpContext.RequestAborted))
            {
                list.Add(new
                {
                    Id = reader.GetInt32(0),
                    Proveedor = reader.GetString(1),
                    Fecha = reader.GetDateTime(2),
                    Total = reader.GetDecimal(3),
                    Entregado = reader.GetString(4) == "Entregada"
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
