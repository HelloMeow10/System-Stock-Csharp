using AgileStockPro.App.Models;

namespace AgileStockPro.App.Services;

public class ScrapService
{
    private readonly List<Scrap> _scraps = new()
    {
        new Scrap("SCR-0125", new DateOnly(2024, 3, 15), "Pintura Latex 20L - Lote A442", 8, "Vencido", 12400m, "Admin"),
        new Scrap("SCR-0124", new DateOnly(2024, 3, 14), "Cable UTP Cat6 - Lote B223", 15, "Dañado", 8900m, "Almacén1"),
        new Scrap("SCR-0123", new DateOnly(2024, 3, 13), "Cemento Portland 50kg - Lote C556", 12, "Humedad", 4200m, "Admin"),
        new Scrap("SCR-0122", new DateOnly(2024, 3, 12), "Luminaria LED 18W - Lote D789", 6, "Defecto Fabricación", 3800m, "Almacén2"),
    };

    public Task<List<Scrap>> ObtenerScrapsAsync()
    {
        return Task.FromResult(_scraps);
    }
}