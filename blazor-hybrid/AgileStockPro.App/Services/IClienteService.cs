using AgileStockPro.App.Models;

namespace AgileStockPro.App.Services;

public interface IClienteService
{
    Task<List<Cliente>> GetClientesAsync();
}