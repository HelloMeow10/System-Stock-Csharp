using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace DataAccess.Entities
{
    [Table("personas")]
    public class Persona
    {
        [Key]
        [Column("id_persona")]
        public int IdPersona { get; set; }

        [Required]
        [Column("legajo")]
        public int Legajo { get; private set; }

        [Required]
        [Column("nombre")]
        [StringLength(30)]
        public string Nombre { get; private set; } = null!;

        [Required]
        [Column("apellido")]
        [StringLength(30)]
        public string Apellido { get; private set; } = null!;

        [NotMapped]
        public string NombreCompleto => $"{Nombre} {Apellido}";

        [Column("id_tipo_doc")]
        public int IdTipoDoc { get; private set; }

        [Required]
        [Column("num_doc")]
        [StringLength(20)]
        public string NumDoc { get; private set; } = null!;

        [Column("fecha_nacimiento")]
        public DateTime? FechaNacimiento { get; private set; }

        [Column("cuil")]
        [StringLength(15)]
        public string? Cuil { get; private set; }

        [Column("calle")]
        [StringLength(50)]
        public string? Calle { get; private set; }

        [Column("altura")]
        [StringLength(30)]
        public string? Altura { get; private set; }

        [Column("id_localidad")]
        public int IdLocalidad { get; private set; }

        [Column("id_genero")]
        public int IdGenero { get; private set; }

        [Column("correo")]
        [StringLength(100)]
        public string? Correo { get; private set; }

        [Column("celular")]
        [StringLength(30)]
        public string? Celular { get; private set; }

        [Column("fecha_ingreso")]
        public DateTime FechaIngreso { get; private set; }

        [ForeignKey("IdTipoDoc")]
        public virtual TipoDoc? TipoDoc { get; set; }

        [ForeignKey("IdLocalidad")]
        public virtual Localidad? Localidad { get; set; }

        [ForeignKey("IdGenero")]
        public virtual Genero? Genero { get; set; }

        private Persona() { } // EF Core constructor

        public Persona(int legajo, string nombre, string apellido, int idTipoDoc, string numDoc, DateTime? fechaNacimiento, string? cuil, string? calle, string? altura, int idLocalidad, int idGenero, string? correo, string? celular, DateTime fechaIngreso)
        {
            Legajo = legajo;
            Nombre = nombre;
            Apellido = apellido;
            IdTipoDoc = idTipoDoc;
            NumDoc = numDoc;
            FechaNacimiento = fechaNacimiento;
            Cuil = cuil;
            Calle = calle;
            Altura = altura;
            IdLocalidad = idLocalidad;
            IdGenero = idGenero;
            Correo = correo;
            Celular = celular;
            FechaIngreso = fechaIngreso;
        }

        public void Update(int legajo, string nombre, string apellido, int idTipoDoc, string numDoc, DateTime? fechaNacimiento, string? cuil, string? calle, string? altura, int idLocalidad, int idGenero, string? correo, string? celular, DateTime fechaIngreso)
        {
            Legajo = legajo;
            Nombre = nombre;
            Apellido = apellido;
            IdTipoDoc = idTipoDoc;
            NumDoc = numDoc;
            FechaNacimiento = fechaNacimiento;
            Cuil = cuil;
            Calle = calle;
            Altura = altura;
            IdLocalidad = idLocalidad;
            IdGenero = idGenero;
            Correo = correo;
            Celular = celular;
            FechaIngreso = fechaIngreso;
        }
    }
}