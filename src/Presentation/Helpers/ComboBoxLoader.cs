using System;
using System.Linq;
using System.Windows.Forms;
using BusinessLogic.Services;

namespace Presentation.Helpers
{
    public class ComboBoxLoader
    {
        private readonly IReferenceDataService _referenceService;

        public ComboBoxLoader(IReferenceDataService referenceService)
        {
            _referenceService = referenceService;
        }

        public void LoadTiposDoc(ComboBox comboBox)
        {
            var tiposDoc = _referenceService.GetTiposDoc();
            if (tiposDoc == null || !tiposDoc.Any())
            {
                MessageBox.Show("No se encontraron tipos de documento.", "Advertencia");
                comboBox.DataSource = null;
                return;
            }
            comboBox.DataSource = tiposDoc;
            comboBox.DisplayMember = "Nombre";
            comboBox.ValueMember = "IdTipoDoc";
        }

        public void LoadProvincias(ComboBox comboBox)
        {
            var provincias = _referenceService.GetProvincias();
            if (provincias != null && provincias.Any())
            {
                comboBox.DataSource = provincias;
                comboBox.DisplayMember = "Nombre";
                comboBox.ValueMember = "IdProvincia";
            }
        }

        public void LoadGeneros(ComboBox comboBox)
        {
            var generos = _referenceService.GetGeneros();
            if (generos == null || !generos.Any())
            {
                MessageBox.Show("No se encontraron géneros.", "Advertencia");
                comboBox.DataSource = null;
                return;
            }
            comboBox.DataSource = generos;
            comboBox.DisplayMember = "Nombre";
            comboBox.ValueMember = "IdGenero";
        }

        public void LoadRoles(ComboBox comboBox)
        {
            var roles = _referenceService.GetRoles();
            if (roles == null || !roles.Any())
            {
                MessageBox.Show("No se encontraron roles.", "Advertencia");
                comboBox.DataSource = null;
                return;
            }
            comboBox.DataSource = roles;
            comboBox.DisplayMember = "Nombre";
            comboBox.ValueMember = "IdRol";
        }

        public void LoadPartidos(ComboBox comboBox, int provinciaId)
        {
            var partidos = _referenceService.GetPartidosByProvinciaId(provinciaId);
            if (partidos != null && partidos.Any())
            {
                comboBox.DataSource = partidos;
                comboBox.DisplayMember = "Nombre";
                comboBox.ValueMember = "IdPartido";
                comboBox.Enabled = true;
            }
            else
            {
                comboBox.DataSource = null;
                comboBox.Enabled = false;
            }
        }

        public void LoadLocalidades(ComboBox comboBox, int partidoId)
        {
            var localidades = _referenceService.GetLocalidadesByPartidoId(partidoId);
            if (localidades != null && localidades.Any())
            {
                comboBox.DataSource = localidades;
                comboBox.DisplayMember = "Nombre";
                comboBox.ValueMember = "IdLocalidad";
                comboBox.Enabled = true;
            }
            else
            {
                comboBox.DataSource = null;
                comboBox.Enabled = false;
            }
        }
    }
}
