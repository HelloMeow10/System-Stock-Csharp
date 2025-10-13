using AgileStockPro.App.Models;

namespace AgileStockPro.App.Services;

public class ClienteService : IClienteService
{
    public Task<List<Cliente>> GetClientesAsync()
    {
        var clientes = new List<Cliente>
        {
            new("CLI-001", "Construcciones del Sur", "30-98765432-1", "+54 11 4444-5555", "contacto@conssur.com", "Av. del Libertador 2500, CABA", 500000, 485000, 38),
            new("CLI-002", "Obras Civiles SA", "30-11223344-5", "+54 11 6666-7777", "admin@obrasciviles.com", "San Martín 1800, Martínez", 350000, 320000, 29),
            new("CLI-003", "Instalaciones Industriales", "30-55667788-9", "+54 11 8888-9999", "ventas@instind.com", "Ruta 8 Km 42, Pilar", 800000, 742000, 24)
        };
        return Task.FromResult(clientes);
    }
}