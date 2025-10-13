using Microsoft.FluentUI.AspNetCore.Components;

namespace AgileStockPro.App.Models.Dashboard;

public record TrendInfo(double Value, bool IsPositive);

public record StatsCardModel(
    string Title,
    string Value,
    Icon Icon,
    string Variant,
    TrendInfo? Trend = null
);

public record AlertModel(
    string ProductName,
    string Sku,
    int CurrentStock,
    int MinimumStock,
    string Status
);

public record RecentActivityModel(
    string Action,
    string Detail,
    string Time
);

public record TopProductModel(
    string Name,
    int Units,
    string Revenue
);

public record TopSupplierModel(
    string Name,
    int Orders,
    string Amount
);

public record DashboardStats(
    int TotalProducts,
    double TotalProductsTrend,
    int LowStockProducts,
    decimal MonthlySales,
    double MonthlySalesTrend,
    int PendingOrders
);

public record Transaction(
    string Id,
    DateTime Date,
    string Description,
    decimal Amount,
    string Type
);

public record TopClientModel(
    string Name,
    int Orders,
    string Amount
);