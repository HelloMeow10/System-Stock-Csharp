namespace AgileStockPro.App.Models;

public record CompraStats(
    int ProductosActivos,
    int Proveedores,
    int OrdenesDelMes,
    double OrdenesDelMesTrend
);