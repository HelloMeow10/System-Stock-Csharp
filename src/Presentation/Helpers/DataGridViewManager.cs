using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Contracts;
using BusinessLogic.Services;
using SharedKernel;

namespace Presentation.Helpers
{
    public class DataGridViewManager
    {
        private readonly IUserService _userService;
        private readonly DataGridView _dataGridView;
        private List<UserDto> _allUsers;
        private readonly List<int> _dirtyUserIds = new List<int>();

        public DataGridViewManager(IUserService userService, DataGridView dataGridView)
        {
            _userService = userService;
            _dataGridView = dataGridView;
            _allUsers = new List<UserDto>();

            _dataGridView.CellEndEdit += DgvUsuarios_CellEndEdit;
        }

        public async void LoadUsers()
        {
            try
            {
                var pagedUsers = await _userService.GetUsersAsync(new PaginationParams { PageSize = 1000 }); // Assuming max 1000 users for simplicity
                _allUsers = pagedUsers.Items;
                _dataGridView.DataSource = new List<UserDto>(_allUsers);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error");
            }
        }

        public void FilterUsers(string searchText)
        {
            var filteredUsers = _allUsers.Where(u =>
                (u.Username?.ToLower() ?? "").Contains(searchText.ToLower().Trim()) ||
                (u.NombreCompleto?.ToLower() ?? "").Contains(searchText.ToLower().Trim())
            ).ToList();
            _dataGridView.DataSource = filteredUsers;
        }

        private void DgvUsuarios_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var userDto = (UserDto)_dataGridView.Rows[e.RowIndex].DataBoundItem;
                if (!_dirtyUserIds.Contains(userDto.IdUsuario))
                {
                    _dirtyUserIds.Add(userDto.IdUsuario);
                }
            }
        }

        public void AddDirtyUserId(int id)
        {
            if (!_dirtyUserIds.Contains(id))
            {
                _dirtyUserIds.Add(id);
            }
        }

        public async void SaveChanges()
        {
            try
            {
                if (_dataGridView.DataSource is List<UserDto> userDtos)
                {
                    var usersToUpdate = userDtos.Where(u => _dirtyUserIds.Contains(u.IdUsuario)).ToList();
                    foreach (var userDto in usersToUpdate)
                    {
                        var updateUserRequest = new UpdateUserRequest
                        {
                            Nombre = userDto.Nombre,
                            Apellido = userDto.Apellido,
                            Correo = userDto.Correo,
                            IdRol = userDto.IdRol,
                            CambioContrasenaObligatorio = userDto.CambioContrasenaObligatorio,
                            FechaExpiracion = userDto.FechaExpiracion,
                            Habilitado = userDto.Habilitado
                        };
                        await _userService.UpdateUserAsync(userDto.IdUsuario, updateUserRequest);
                    }

                    if (usersToUpdate.Any())
                    {
                        MessageBox.Show("Cambios guardados exitosamente.", "Éxito");
                    }
                    else
                    {
                        MessageBox.Show("No hay cambios para guardar.", "Información");
                    }

                    _dirtyUserIds.Clear();
                    LoadUsers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar cambios: {ex.Message}", "Error");
            }
        }

        public async void DeleteSelectedUser()
        {
            if (_dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un usuario para eliminar.", "Advertencia");
                return;
            }

            var selectedRow = _dataGridView.SelectedRows[0];
            var userDto = (UserDto)selectedRow.DataBoundItem;

            var confirmResult = MessageBox.Show($"¿Está seguro de que desea eliminar al usuario '{userDto.Username}'?",
                                                 "Confirmar Eliminación",
                                                 MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    await _userService.DeleteUserAsync(userDto.IdUsuario);
                    MessageBox.Show("Usuario eliminado exitosamente.", "Éxito");
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar usuario: {ex.Message}", "Error");
                }
            }
        }
    }
}
