namespace AgileStockPro.Web.Models;

public class Product
{
    // Database numeric primary key (Productos.id_producto)
    public int DbId { get; set; }
    // Business/code identifier (Productos.codigo)
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public int Stock { get; set; }
    public int MinStock { get; set; }
    public int MaxStock { get; set; }
    public string Status { get; set; } = string.Empty;
    public string UnidadMedida { get; set; } = string.Empty;
    public decimal Peso { get; set; }
    public decimal Volumen { get; set; }
    public int PuntoReposicion { get; set; }
    public int DiasVencimiento { get; set; }
    public bool LoteObligatorio { get; set; }
    public bool ControlVencimiento { get; set; }
}
