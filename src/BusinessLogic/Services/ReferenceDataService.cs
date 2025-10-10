using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Exceptions;
using Contracts;
using DataAccess.Repositories;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly ILogger<ReferenceDataService> _logger;

        public ReferenceDataService(IReferenceDataRepository referenceDataRepository, ILogger<ReferenceDataService> logger)
        {
            _referenceDataRepository = referenceDataRepository ?? throw new ArgumentNullException(nameof(referenceDataRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private async Task<T> ExecuteServiceOperationAsync<T>(Func<Task<T>> operation, string operationName)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during {OperationName}", operationName);
                throw new BusinessLogicException($"An unexpected error occurred during {operationName}.", ex);
            }
        }

        public async Task<List<TipoDocDto>> GetTiposDocAsync() => await ExecuteServiceOperationAsync(async () =>
        {
            var data = await _referenceDataRepository.GetAllTiposDocAsync();
            return data.Select(t => new TipoDocDto { IdTipoDoc = t.IdTipoDoc, Nombre = t.Nombre }).ToList();
        }, "getting all document types");

        public async Task<List<ProvinciaDto>> GetProvinciasAsync() => await ExecuteServiceOperationAsync(async () =>
        {
            var data = await _referenceDataRepository.GetAllProvinciasAsync();
            return data.Select(p => new ProvinciaDto { IdProvincia = p.IdProvincia, Nombre = p.Nombre }).ToList();
        }, "getting all provinces");

        public async Task<List<PartidoDto>> GetPartidosByProvinciaIdAsync(int provinciaId) => await ExecuteServiceOperationAsync(async () =>
        {
            var data = await _referenceDataRepository.GetPartidosByProvinciaIdAsync(provinciaId);
            return data.Select(p => new PartidoDto { IdPartido = p.IdPartido, Nombre = p.Nombre, IdProvincia = p.IdProvincia }).ToList();
        }, "getting partidos by provincia");

        public async Task<List<LocalidadDto>> GetLocalidadesByPartidoIdAsync(int partidoId) => await ExecuteServiceOperationAsync(async () =>
        {
            var data = await _referenceDataRepository.GetLocalidadesByPartidoIdAsync(partidoId);
            return data.Select(l => new LocalidadDto { IdLocalidad = l.IdLocalidad, Nombre = l.Nombre, IdPartido = l.IdPartido }).ToList();
        }, "getting localidades by partido");

        public async Task<List<GeneroDto>> GetGenerosAsync() => await ExecuteServiceOperationAsync(async () =>
        {
            var data = await _referenceDataRepository.GetAllGenerosAsync();
            return data.Select(g => new GeneroDto { IdGenero = g.IdGenero, Nombre = g.Nombre }).ToList();
        }, "getting all genders");

        public async Task<List<RolDto>> GetRolesAsync() => await ExecuteServiceOperationAsync(async () =>
        {
            var data = await _referenceDataRepository.GetAllRolesAsync();
            return data.Select(r => new RolDto { IdRol = r.IdRol, Nombre = r.Nombre }).ToList();
        }, "getting all roles");
    }
}