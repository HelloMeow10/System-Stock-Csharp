using AgileStockPro.Web.Models;

namespace AgileStockPro.Web.Services;

public class AppDataService : IAppDataService
{
    public async Task<IReadOnlyList<Supplier>> GetSuppliersAsync(CancellationToken ct = default)
    {
        await Task.Delay(50, ct);
        return new List<Supplier>
        {
            new() { Id = 1, Name = "Tech Supplies SA", Email = "contacto@techsupplies.com", Phone = "+54 11 5555-0001", Province="CABA", City="Buenos Aires", Address="Av. Siempre Viva 123", Cuit="30-12345678-9", PaymentTermDays=30, IvaCondition="Responsable Inscripto", Contacts = new(){ new(){ Name="Laura Pérez", Role="Ventas", Email="lperez@techsupplies.com", Phone="+54 11 5555-0002" } } },
            new() { Id = 2, Name = "Distribuidora Norte", Email = "ventas@norte.com", Phone = "+54 381 444-2211", Province="Tucumán", City="San Miguel", Address="Belgrano 450", Cuit="30-87654321-0", PaymentTermDays=45, IvaCondition="Monotributo", Contacts = new(){ new(){ Name="Carlos Gómez", Role="Compras", Email="cgomez@norte.com", Phone="+54 381 444-2212" } } },
        };
    }

    public async Task<IReadOnlyList<Customer>> GetCustomersAsync(CancellationToken ct = default)
    {
        await Task.Delay(50, ct);
        return new List<Customer>
        {
            new() { Id = 1, Name = "ACME SRL", Email = "compras@acme.com", Phone = "+54 11 5555-0333" },
            new() { Id = 2, Name = "Mercado Central", Email = "mc@market.com", Phone = "+54 261 421-2020" },
        };
    }

    public async Task<IReadOnlyList<Almacen>> GetWarehousesAsync(CancellationToken ct = default)
    {
        await Task.Delay(50, ct);
        return new List<Almacen>
        {
            new() { Id = 1, Name = "Central", Location = "CABA" },
            new() { Id = 2, Name = "Norte", Location = "Tucumán" },
        };
    }

    public async Task<IReadOnlyList<StockItem>> GetStockAsync(CancellationToken ct = default)
    {
        await Task.Delay(50, ct);
        return new List<StockItem>
        {
            new() { Id = 1, Product = "Mouse Inalámbrico", Warehouse = "Central", Quantity = 42, Min = 10, Max = 100 },
            new() { Id = 2, Product = "Teclado Mecánico", Warehouse = "Central", Quantity = 18, Min = 5, Max = 60 },
            new() { Id = 3, Product = "Cable USB-C", Warehouse = "Norte", Quantity = 12, Min = 20, Max = 200 },
        };
    }

    public async Task<IReadOnlyList<Movimiento>> GetMovementsAsync(CancellationToken ct = default)
    {
        await Task.Delay(50, ct);
        var now = DateTime.Now;
        return new List<Movimiento>
        {
            new() { Id = 1, Date = now.AddHours(-2), Type = "Entrada", Product = "Mouse Inalámbrico", Quantity = 24, From = "Proveedor", To = "Central" },
            new() { Id = 2, Date = now.AddHours(-1), Type = "Salida", Product = "Cable USB-C", Quantity = 5, From = "Central", To = "Pedido #50012" },
            new() { Id = 3, Date = now.AddMinutes(-30), Type = "Transferencia", Product = "Teclado Mecánico", Quantity = 3, From = "Central", To = "Norte" },
        };
    }

    public async Task<IReadOnlyList<OrdenCompra>> GetPurchaseOrdersAsync(CancellationToken ct = default)
    {
        await Task.Delay(50, ct);
        return new List<OrdenCompra>
        {
            new() { Id = 1001, Supplier = "Tech Supplies SA", Date = DateTime.Today.AddDays(-2), Status = "Pendiente", Total = 7980m },
            new() { Id = 1002, Supplier = "Distribuidora Norte", Date = DateTime.Today.AddDays(-1), Status = "Recibida", Total = 5120m },
        };
    }

    public async Task<IReadOnlyList<Pedido>> GetOrdersAsync(CancellationToken ct = default)
    {
        await Task.Delay(50, ct);
        return new List<Pedido>
        {
            new() { Id = 50012, Customer = "ACME SRL", Date = DateTime.Today.AddDays(-1), Status = "Preparando", Total = 340m },
            new() { Id = 50013, Customer = "Mercado Central", Date = DateTime.Today, Status = "Enviado", Total = 790m },
        };
    }

    public async Task<IReadOnlyList<Factura>> GetInvoicesAsync(CancellationToken ct = default)
    {
        await Task.Delay(50, ct);
        return new List<Factura>
        {
            new() { Id = 9001, Customer = "ACME SRL", Date = DateTime.Today, Total = 340m, Status = "Emitida" },
            new() { Id = 9002, Customer = "Mercado Central", Date = DateTime.Today.AddDays(-2), Total = 1200m, Status = "Pagada" },
        };
    }

    public async Task<IReadOnlyList<Alerta>> GetAlertsAsync(CancellationToken ct = default)
    {
        await Task.Delay(50, ct);
        return new List<Alerta>
        {
            new() { Date = DateTime.Now.AddHours(-2), Message = "Bajo stock en Cable USB-C", Severity = "warning" },
            new() { Date = DateTime.Now.AddHours(-1), Message = "OC 1002 recibida", Severity = "info" },
        };
    }

    public async Task<IReadOnlyList<ScrapItem>> GetScrapAsync(CancellationToken ct = default)
    {
        await Task.Delay(50, ct);
        return new List<ScrapItem>
        {
            new() { Id = 1, Product = "Cable USB-C", Quantity = 2, Reason = "Daño", Date = DateTime.Today.AddDays(-1) },
            new() { Id = 2, Product = "Mouse Inalámbrico", Quantity = 1, Reason = "Defecto", Date = DateTime.Today.AddDays(-3) },
        };
    }
}
