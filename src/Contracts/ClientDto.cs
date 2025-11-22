namespace Contracts
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string CuitDni { get; set; } = string.Empty;
        public int IdFormaPago { get; set; }
        public decimal LimiteCredito { get; set; }
        public decimal Descuento { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class CreateClientRequest
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string CuitDni { get; set; } = string.Empty;
        public int IdFormaPago { get; set; }
        public decimal LimiteCredito { get; set; }
        public decimal Descuento { get; set; }
        public string Estado { get; set; } = "Activo";
    }

    public class UpdateClientRequest : CreateClientRequest
    {
        public int Id { get; set; }
    }
}
