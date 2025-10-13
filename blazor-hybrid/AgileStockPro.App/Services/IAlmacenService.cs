using AgileStockPro.App.Models;

namespace AgileStockPro.App.Services;

public interface IAlmacenService
{
    Task<List<Almacen>> GetAlmacenesAsync();
}