using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("politicas_seguridad")]
    public class PoliticaSeguridad
    {
        [Key]
        [Column("id_politica")]
        public int IdPolitica { get; private set; }

        [Column("mayus_y_minus")]
        public bool MayusYMinus { get; private set; }

        [Column("letras_y_numeros")]
        public bool LetrasYNumeros { get; private set; }

        [Column("caracter_especial")]
        public bool CaracterEspecial { get; private set; }

        [Column("autenticacion_2fa")]
        public bool Autenticacion2FA { get; private set; }

        [Column("no_repetir_anteriores")]
        public bool NoRepetirAnteriores { get; private set; }

        [Column("sin_datos_personales")]
        public bool SinDatosPersonales { get; private set; }

        [Column("min_caracteres")]
        public int MinCaracteres { get; private set; }

        [Column("cant_preguntas")]
        public int CantPreguntas { get; private set; }

        private PoliticaSeguridad() { } // EF Core constructor

        public PoliticaSeguridad(int idPolitica, bool mayusYMinus, bool letrasYNumeros, bool caracterEspecial, bool autenticacion2FA, bool noRepetirAnteriores, bool sinDatosPersonales, int minCaracteres, int cantPreguntas)
        {
            IdPolitica = idPolitica;
            MayusYMinus = mayusYMinus;
            LetrasYNumeros = letrasYNumeros;
            CaracterEspecial = caracterEspecial;
            Autenticacion2FA = autenticacion2FA;
            NoRepetirAnteriores = noRepetirAnteriores;
            SinDatosPersonales = sinDatosPersonales;
            MinCaracteres = minCaracteres;
            CantPreguntas = cantPreguntas;
        }

        public void Update(bool mayusYMinus, bool letrasYNumeros, bool caracterEspecial, bool autenticacion2FA, bool noRepetirAnteriores, bool sinDatosPersonales, int minCaracteres, int cantPreguntas)
        {
            if (minCaracteres < 4)
            {
                throw new ArgumentException("La longitud mínima de caracteres no puede ser menor que 4.", nameof(minCaracteres));
            }
            if (cantPreguntas < 1)
            {
                throw new ArgumentException("La cantidad de preguntas de seguridad no puede ser menor que 1.", nameof(cantPreguntas));
            }

            MayusYMinus = mayusYMinus;
            LetrasYNumeros = letrasYNumeros;
            CaracterEspecial = caracterEspecial;
            Autenticacion2FA = autenticacion2FA;
            NoRepetirAnteriores = noRepetirAnteriores;
            SinDatosPersonales = sinDatosPersonales;
            MinCaracteres = minCaracteres;
            CantPreguntas = cantPreguntas;
        }
    }
}