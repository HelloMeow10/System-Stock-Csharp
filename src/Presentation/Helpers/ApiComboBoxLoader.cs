using Presentation.ApiClient;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Presentation.Helpers
{
    public class ApiComboBoxLoader
    {
        private readonly ApiClient.ApiClient _apiClient;

        public ApiComboBoxLoader(ApiClient.ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task LoadTiposDoc(ComboBox comboBox)
        {
            var data = await _apiClient.GetTiposDocAsync();
            comboBox.DataSource = data;
            comboBox.DisplayMember = "Descripcion";
            comboBox.ValueMember = "IdTipoDoc";
        }

        public async Task LoadProvincias(ComboBox comboBox)
        {
            var data = await _apiClient.GetProvinciasAsync();
            comboBox.DataSource = data;
            comboBox.DisplayMember = "Nombre";
            comboBox.ValueMember = "IdProvincia";
        }

        public async Task LoadPartidos(ComboBox comboBox, int provinciaId)
        {
            var data = await _apiClient.GetPartidosAsync(provinciaId);
            comboBox.DataSource = data;
            comboBox.DisplayMember = "Nombre";
            comboBox.ValueMember = "IdPartido";
            comboBox.Enabled = true;
        }

        public async Task LoadLocalidades(ComboBox comboBox, int partidoId)
        {
            var data = await _apiClient.GetLocalidadesAsync(partidoId);
            comboBox.DataSource = data;
            comboBox.DisplayMember = "Nombre";
            comboBox.ValueMember = "IdLocalidad";
            comboBox.Enabled = true;
        }

        public async Task LoadGeneros(ComboBox comboBox)
        {
            var data = await _apiClient.GetGenerosAsync();
            comboBox.DataSource = data;
            comboBox.DisplayMember = "Descripcion";
            comboBox.ValueMember = "IdGenero";
        }

        public async Task LoadRoles(ComboBox comboBox)
        {
            var data = await _apiClient.GetRolesAsync();
            comboBox.DataSource = data;
            comboBox.DisplayMember = "Nombre";
            comboBox.ValueMember = "IdRol";
        }
    }
}
