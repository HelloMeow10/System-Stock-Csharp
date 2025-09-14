using System.Threading.Tasks;
using Contracts;
using DataAccess.Entities;

namespace BusinessLogic.Factories
{
    public interface IUsuarioFactory
    {
        Task<(Usuario Usuario, string PlainPassword)> Create(UserRequest request);
    }
}
