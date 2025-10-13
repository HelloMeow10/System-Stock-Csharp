using AgileStockPro.App.Models;

namespace AgileStockPro.App.Services;

public class SupplierService : ISupplierService
{
    public Task<List<Supplier>> GetSuppliersAsync()
    {
        var suppliers = new List<Supplier>
        {
            new(
                Id: "PRV-001",
                Name: "Ferretería Central SA",
                Cuit: "30-12345678-9",
                Phone: "+54 11 4567-8900",
                Email: "ventas@ferreteriacentral.com",
                Address: "Av. Corrientes 1234, CABA",
                Categories: new List<string> { "Ferretería", "Construcción" },
                Discount: "15%",
                DeliveryTime: "48hs"
            ),
            new(
                Id: "PRV-002",
                Name: "Distribuidora del Norte",
                Cuit: "30-98765432-1",
                Phone: "+54 11 4321-0987",
                Email: "contacto@distnorte.com",
                Address: "San Martín 567, Vicente López",
                Categories: new List<string> { "Electricidad", "Iluminación" },
                Discount: "10%",
                DeliveryTime: "72hs"
            ),
            new(
                Id: "PRV-003",
                Name: "Pinturas y Revestimientos",
                Cuit: "30-55555555-5",
                Phone: "+54 11 5555-5555",
                Email: "info@pinturasyrev.com",
                Address: "Rivadavia 890, CABA",
                Categories: new List<string> { "Pinturería" },
                Discount: "20%",
                DeliveryTime: "24hs"
            ),
        };
        return Task.FromResult(suppliers);
    }
}