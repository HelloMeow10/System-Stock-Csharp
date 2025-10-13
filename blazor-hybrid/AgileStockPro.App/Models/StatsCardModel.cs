using Microsoft.FluentUI.AspNetCore.Components;

namespace AgileStockPro.App.Models
{
    public class StatsCardModel
    {
        public string Title { get; set; }
        public string Value { get; set; }
    public Icon Icon { get; set; }
        public TrendModel? Trend { get; set; }
        public string Variant { get; set; }
    }

    public class TrendModel
    {
        public int Value { get; set; }
        public bool IsPositive { get; set; }
    }
}