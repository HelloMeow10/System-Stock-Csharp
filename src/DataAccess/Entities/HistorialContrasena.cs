using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// src/DataAccess/Entities/HistorialContrasena.cs
namespace DataAccess.Entities
{
    public class HistorialContrasena
    {
        public int IdHistorial { get; set; }
        public int IdUsuario { get; set; }
        public byte[] ContrasenaScript { get; set; } = null!;
        public DateTime FechaCambio { get; set; }
        public Usuario? Usuario { get; set; }
    }
}