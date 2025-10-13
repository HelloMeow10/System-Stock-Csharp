namespace AgileStockPro.App.Models;

public class RecentActivityItemModel
{
    public string? Action { get; set; }
    public string? Detail { get; set; }
    public string? Time { get; set; }
}

public class TopProductModel
{
    public string? Name { get; set; }
    public int Units { get; set; }
    public string? Revenue { get; set; }
}

public class TopSupplierModel
{
    public string? Name { get; set; }
    public int Orders { get; set; }
    public string? Amount { get; set; }
}

public class TopCustomerModel
{
    public string? Name { get; set; }
    public int Orders { get; set; }
    public string? Amount { get; set; }
}