using DataAccess.Entities;
using System.Threading.Tasks;

namespace BusinessLogic.Factories
{
    public interface IUsuarioFactory
    {
        Task<(Usuario Usuario, string PlainPassword)> CreateAsync<TRequest>(TRequest request) where TRequest : class;
    }
}
