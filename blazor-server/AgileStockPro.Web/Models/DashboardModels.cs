using System;
using System.Collections.Generic;

namespace AgileStockPro.Web.Models;

public class StatsCardModel
{
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string Value { get; set; } = "0";
    public string? Delta { get; set; }
    public string? DeltaAria { get; set; }
    public string? Icon { get; set; }
    public string Accent { get; set; } = "accent"; // e.g., accent / success / warning / danger
}

public class RecentActivityModel
{
    public DateTime When { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = "info"; // purchase, sale, stock, alert
}

public class TopProductModel
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Sold { get; set; }
    public int Stock { get; set; }
}
