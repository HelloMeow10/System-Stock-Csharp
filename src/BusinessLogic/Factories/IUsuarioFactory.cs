using System.Threading.Tasks;
using Contracts;
using DataAccess.Entities;

namespace BusinessLogic.Factories
{
    public interface IUsuarioFactory
    {
        Task<(Usuario Usuario, string PlainPassword)> Create(UserRequest request);
        Task<(Usuario Usuario, string PlainPassword)> CreateV2(UserRequestV2 request);
    }
}
