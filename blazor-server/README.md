# AgileStockPro.Web (Blazor Server)

Servidor web 100% .NET (sin Node) para ver la app en navegador.

## Ejecutar

1. Requisitos: .NET SDK 9.0
2. Desde la carpeta `blazor-server/AgileStockPro.Web`:

```powershell
# Restaurar paquetes y ejecutar
dotnet run
```

La app abrirá (según `launchSettings.json`) en http://localhost:5173.

## Notas
- Este proyecto es independiente del MAUI Blazor Hybrid y sirve la UI por HTTP.
- Se usa Microsoft.FluentUI.AspNetCore.Components igual que en el proyecto MAUI.
- Puedes portar gradualmente páginas y servicios desde `blazor-hybrid/AgileStockPro.App`.
