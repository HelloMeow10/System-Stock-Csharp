using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Services.Infrastructure
{
    public class DatabaseHealthHostedService : IHostedService
    {
        private readonly ILogger<DatabaseHealthHostedService> _logger;
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        private readonly List<string> _requiredTables = new();
        private readonly List<string> _requiredProcedures = new();

        public DatabaseHealthHostedService(ILogger<DatabaseHealthHostedService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection")!;
            _requiredTables.AddRange(_config.GetSection("DbHealth:RequiredTables").Get<string[]>() ?? []);
            _requiredProcedures.AddRange(_config.GetSection("DbHealth:RequiredProcedures").Get<string[]>() ?? []);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[DB Health] Starting validation of required tables/procedures.");
            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync(cancellationToken);

                var missingTables = new List<string>();
                foreach (var t in _requiredTables)
                {
                    if (!await TableExistsAsync(conn, t, cancellationToken))
                        missingTables.Add(t);
                }

                var missingProcedures = new List<string>();
                foreach (var p in _requiredProcedures)
                {
                    if (!await ProcedureExistsAsync(conn, p, cancellationToken))
                        missingProcedures.Add(p);
                }

                if (missingTables.Count == 0 && missingProcedures.Count == 0)
                {
                    _logger.LogInformation("[DB Health] All required objects exist.");
                }
                else
                {
                    if (missingTables.Count > 0)
                        _logger.LogWarning("[DB Health] Missing tables: {Tables}", string.Join(", ", missingTables));
                    if (missingProcedures.Count > 0)
                        _logger.LogWarning("[DB Health] Missing stored procedures: {Procedures}", string.Join(", ", missingProcedures));
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "[DB Health] SQL exception during health validation.");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "[DB Health] Unexpected exception during health validation.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private static async Task<bool> TableExistsAsync(SqlConnection conn, string tableName, CancellationToken ct)
        {
            using var cmd = new SqlCommand("SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @name", conn)
            { Parameters = { new("@name", SqlDbType.NVarChar, 128) { Value = tableName } } };
            var result = await cmd.ExecuteScalarAsync(ct);
            return result != null;
        }

        private static async Task<bool> ProcedureExistsAsync(SqlConnection conn, string procName, CancellationToken ct)
        {
            using var cmd = new SqlCommand("SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE='PROCEDURE' AND ROUTINE_NAME = @name", conn)
            { Parameters = { new("@name", SqlDbType.NVarChar, 128) { Value = procName } } };
            var result = await cmd.ExecuteScalarAsync(ct);
            return result != null;
        }
    }
}
