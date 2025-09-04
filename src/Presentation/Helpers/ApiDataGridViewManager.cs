using Presentation.ApiClient;
using System.Windows.Forms;
using System.Threading.Tasks;
using BusinessLogic.Models;
using System.Collections.Generic;
using System.Linq;

namespace Presentation.Helpers
{
    public class ApiDataGridViewManager
    {
        private readonly ApiClient.ApiClient _apiClient;
        private readonly DataGridView _dataGridView;
        private List<UserDto> _allUsers = new List<UserDto>();
        private readonly List<int> _dirtyUserIds = new List<int>();

        public ApiDataGridViewManager(ApiClient.ApiClient apiClient, DataGridView dataGridView)
        {
            _apiClient = apiClient;
            _dataGridView = dataGridView;
        }

        public async Task LoadUsers()
        {
            _allUsers = await _apiClient.GetUsersAsync();
            _dataGridView.DataSource = new List<UserDto>(_allUsers);
        }

        public void FilterUsers(string searchText)
        {
            var filteredUsers = _allUsers.Where(u =>
                u.Username.ToLower().Contains(searchText.ToLower())
            ).ToList();
            _dataGridView.DataSource = filteredUsers;
        }

        public async Task SaveChanges()
        {
            var usersToUpdate = _allUsers.Where(u => _dirtyUserIds.Contains(u.IdUsuario)).ToList();
            foreach (var user in usersToUpdate)
            {
                await _apiClient.UpdateUserAsync(user.IdUsuario, user);
            }
            _dirtyUserIds.Clear();
            await LoadUsers();
        }

        public async Task DeleteSelectedUser()
        {
            if (_dataGridView.SelectedRows.Count > 0)
            {
                var selectedUser = (UserDto)_dataGridView.SelectedRows[0].DataBoundItem;
                await _apiClient.DeleteUserAsync(selectedUser.IdUsuario);
                await LoadUsers();
            }
        }

        public void AddDirtyUserId(int userId)
        {
            if (!_dirtyUserIds.Contains(userId))
            {
                _dirtyUserIds.Add(userId);
            }
        }
    }
}
