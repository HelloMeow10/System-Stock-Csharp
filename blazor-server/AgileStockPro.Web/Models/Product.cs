namespace AgileStockPro.Web.Models;

public class Product
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public int Stock { get; set; }
    public int MinStock { get; set; }
    public int MaxStock { get; set; }
    public string Status { get; set; } = string.Empty;
}
