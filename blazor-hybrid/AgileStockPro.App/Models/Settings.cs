namespace AgileStockPro.App.Models
{
    public class DatabaseSettings
    {
        public string Server { get; set; } = "localhost\\SQLEXPRESS";
        public string Database { get; set; } = "StockProDB";
        public string BackupPath { get; set; } = "C:\\StockPro\\Backups";
    }

    public class AlertSettings
    {
        public int ExpiryAlertDays { get; set; } = 30;
        public string NotificationEmail { get; set; } = "admin@stockpro.com";
        public bool NotifyCriticalStock { get; set; } = true;
        public bool NotifyExpiredProducts { get; set; } = true;
    }

    public class User
    {
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class DocumentationSettings
    {
        public string CompanyName { get; set; } = "StockPro SA";
        public string Cuit { get; set; } = "30-12345678-9";
        public string InvoicePrefix { get; set; } = "FC-";
        public string LogoPath { get; set; } = "C:\\StockPro\\Assets\\logo.png";
    }

    public class SystemInfo
    {
        public string Version { get; set; } = "1.0.0";
        public string LastUpdate { get; set; } = "15/03/2024";
        public string UsedSpace { get; set; } = "2.4 GB";
    }

    public class SecuritySettings
    {
        public bool RequireStrongPassword { get; set; } = true;
        public int AutoLogoutMinutes { get; set; } = 30;
        public bool EnableAuditLog { get; set; } = true;
    }

    public class Settings
    {
        public DatabaseSettings Database { get; set; } = new();
        public AlertSettings Alerts { get; set; } = new();
        public List<User> Users { get; set; } = new()
        {
            new User { Name = "Administrador", Role = "Admin", IsActive = true },
            new User { Name = "Usuario Ventas 1", Role = "Ventas", IsActive = true },
            new User { Name = "Usuario Compras 1", Role = "Compras", IsActive = true },
            new User { Name = "Usuario Almacén 1", Role = "Almacén", IsActive = false },
        };
        public DocumentationSettings Documentation { get; set; } = new();
        public SystemInfo System { get; set; } = new();
        public SecuritySettings Security { get; set; } = new();
    }
}