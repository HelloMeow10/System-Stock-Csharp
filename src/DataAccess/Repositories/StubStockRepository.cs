using Contracts;
using System.Collections.Concurrent;

namespace DataAccess.Repositories;

/// <summary>
/// In-memory stub for stock operations. Useful for development and multi-platform demos without a DB.
/// </summary>
public class StubStockRepository : IStockRepository
{
    private readonly ConcurrentBag<StockMovementDto> _movements = new();
    private readonly ConcurrentBag<ScrapReportItemDto> _scrap = new();
    private readonly List<ScrapReasonDto> _reasons = new()
    {
        // A few typical combinations
        new ScrapReasonDto(1, Dano: true,  Vencido: false, Obsoleto: false, MalaCalidad: false),
        new ScrapReasonDto(2, Dano: false, Vencido: true,  Obsoleto: false, MalaCalidad: false),
        new ScrapReasonDto(3, Dano: false, Vencido: false, Obsoleto: true,  MalaCalidad: false),
        new ScrapReasonDto(4, Dano: false, Vencido: false, Obsoleto: false, MalaCalidad: true),
        new ScrapReasonDto(5, Dano: false, Vencido: false, Obsoleto: false, MalaCalidad: false),
    };

    private int _nextMovementId = 1000;
    private int _nextScrapId = 2000;
    private readonly object _lock = new();

    public StubStockRepository()
    {
        // Seed a richer dataset across the last 120 days
        var rnd = new Random(42);
        var today = DateTime.Today;

        int id = 1;
        for (int d = 0; d < 120; d++)
        {
            var date = today.AddDays(-d).AddHours(rnd.Next(8, 18)).AddMinutes(rnd.Next(0, 60));
            // Between 0 and 5 movements per day
            int movementsCount = rnd.Next(0, 6);
            for (int i = 0; i < movementsCount; i++)
            {
                var prodId = rnd.Next(1, 201);
                var qty = rnd.Next(1, 50);
                _movements.Add(new StockMovementDto(
                    Id: id++,
                    Fecha: date.AddMinutes(i * 5),
                    Usuario: $"Usuario {rnd.Next(1, 6)}",
                    Producto: $"Producto {prodId}",
                    TipoMovimiento: "Ingreso",
                    Cantidad: qty
                ));
            }

            // Occasional scrap per day
            if (rnd.NextDouble() < 0.35)
            {
                var prodId = rnd.Next(1, 201);
                var qty = rnd.Next(1, 10);
                var reason = _reasons[rnd.Next(_reasons.Count)];
                _scrap.Add(new ScrapReportItemDto(
                    Id: id++,
                    Fecha: date.AddMinutes(60 + rnd.Next(0, 120)),
                    Usuario: $"Usuario {rnd.Next(1, 6)}",
                    Producto: $"Producto {prodId}",
                    Motivo: BuildMotivo(reason),
                    Cantidad: qty
                ));
            }
        }
    }

    public Task<IEnumerable<StockMovementDto>> GetMovementsAsync(DateOnly from, DateOnly to, CancellationToken ct = default)
    {
        var start = from.ToDateTime(TimeOnly.MinValue);
        var end = to.ToDateTime(TimeOnly.MaxValue);
        var data = _movements.Where(m => m.Fecha >= start && m.Fecha <= end)
                             .OrderByDescending(m => m.Fecha)
                             .AsEnumerable();
        return Task.FromResult(data);
    }

    public Task<IEnumerable<ScrapReportItemDto>> GetScrapAsync(DateOnly from, DateOnly to, CancellationToken ct = default)
    {
        var start = from.ToDateTime(TimeOnly.MinValue);
        var end = to.ToDateTime(TimeOnly.MaxValue);
        var data = _scrap.Where(s => s.Fecha >= start && s.Fecha <= end)
                         .OrderByDescending(s => s.Fecha)
                         .AsEnumerable();
        return Task.FromResult(data);
    }

    public Task<IEnumerable<ScrapReasonDto>> GetScrapReasonsAsync(CancellationToken ct = default)
        => Task.FromResult<IEnumerable<ScrapReasonDto>>(_reasons);

    public Task IngresoMercaderiaAsync(IngresoMercaderiaRequest request, CancellationToken ct = default)
    {
        var now = DateTime.Now;
        int id;
        lock (_lock) { id = ++_nextMovementId; }
        _movements.Add(new StockMovementDto(
            Id: id,
            Fecha: now,
            Usuario: $"Usuario {request.UsuarioId}",
            Producto: $"Producto {request.ProductoId}",
            TipoMovimiento: "Ingreso",
            Cantidad: request.Cantidad
        ));
        return Task.CompletedTask;
    }

    public Task CreateScrapAsync(CreateScrapRequest request, CancellationToken ct = default)
    {
        var now = DateTime.Now;
        int id;
        lock (_lock) { id = ++_nextScrapId; }
        var motivo = BuildMotivo(_reasons.FirstOrDefault(r => r.Id == request.MotivoScrapId));
        _scrap.Add(new ScrapReportItemDto(
            Id: id,
            Fecha: now,
            Usuario: $"Usuario {request.UsuarioId}",
            Producto: $"Producto {request.ProductoId}",
            Motivo: motivo,
            Cantidad: request.Cantidad
        ));
        return Task.CompletedTask;
    }

    private static string BuildMotivo(ScrapReasonDto? r)
    {
        if (r is null) return "Otro";
        var parts = new List<string>();
        if (r.Dano) parts.Add("Daño");
        if (r.Vencido) parts.Add("Vencido");
        if (r.Obsoleto) parts.Add("Obsoleto");
        if (r.MalaCalidad) parts.Add("Mala calidad");
        return parts.Count == 0 ? "Otro" : string.Join(", ", parts);
    }

    public Task<IEnumerable<StockItemDto>> GetStockAsync(CancellationToken ct = default)
    {
        // Stub implementation: return empty list or dummy data
        return Task.FromResult(Enumerable.Empty<StockItemDto>());
    }
}
