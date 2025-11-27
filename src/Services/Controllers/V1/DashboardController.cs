using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using DataAccess;

namespace Services.Controllers.V1
{
    [ApiController]
    [Route("api/v1/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly DatabaseConnectionFactory _connectionFactory;
        public DashboardController(DatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            using var conn = _connectionFactory.CreateConnection();
            await EnsureOpenAsync(conn, HttpContext.RequestAborted);
            using var cmd = new SqlCommand("sp_dashboard_summary", (SqlConnection)conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            using var reader = await cmd.ExecuteReaderAsync(HttpContext.RequestAborted);
            if (!await reader.ReadAsync(HttpContext.RequestAborted)) return NotFound();

            var dto = new
            {
                SalesTodayDisplay = reader.GetString(0),
                SalesDeltaDisplay = reader.GetString(1),
                SalesDeltaAria = reader.GetString(2),
                PurchasesTodayDisplay = reader.GetString(3),
                PurchasesDeltaDisplay = reader.GetString(4),
                PurchasesDeltaAria = reader.GetString(5),
                StockAvailableDisplay = reader.GetString(6),
                StockDeltaDisplay = reader.GetString(7),
                StockDeltaAria = reader.GetString(8),
                AlertsActiveDisplay = reader.GetString(9),
                AlertsDeltaDisplay = reader.GetString(10),
                AlertsDeltaAria = reader.GetString(11)
            };
            return Ok(dto);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecent([FromQuery] int top = 15)
        {
            using var conn = _connectionFactory.CreateConnection();
            await EnsureOpenAsync(conn, HttpContext.RequestAborted);
            using var cmd = new SqlCommand("sp_dashboard_recent", (SqlConnection)conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new SqlParameter("@Top", SqlDbType.Int) { Value = top });

            var list = new List<object>();
            using var reader = await cmd.ExecuteReaderAsync(HttpContext.RequestAborted);
            while (await reader.ReadAsync(HttpContext.RequestAborted))
            {
                list.Add(new
                {
                    When = reader.GetDateTime(0),
                    Description = reader.GetString(1),
                    Category = reader.GetString(2)
                });
            }
            return Ok(list);
        }

        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopProducts([FromQuery] int days = 30, [FromQuery] int top = 10)
        {
            using var conn = _connectionFactory.CreateConnection();
            await EnsureOpenAsync(conn, HttpContext.RequestAborted);
            using var cmd = new SqlCommand("sp_dashboard_top_products", (SqlConnection)conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new SqlParameter("@Days", SqlDbType.Int) { Value = days });
            cmd.Parameters.Add(new SqlParameter("@Top", SqlDbType.Int) { Value = top });

            var list = new List<object>();
            using var reader = await cmd.ExecuteReaderAsync(HttpContext.RequestAborted);
            while (await reader.ReadAsync(HttpContext.RequestAborted))
            {
                list.Add(new
                {
                    Name = reader.GetString(0),
                    Category = reader.GetString(1),
                    Sold = reader.GetInt32(2),
                    Stock = reader.GetInt32(3)
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
