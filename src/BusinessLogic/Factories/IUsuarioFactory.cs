using System.Threading.Tasks;
using BusinessLogic.Models;
using DataAccess.Entities;

namespace BusinessLogic.Factories
{
    public interface IUsuarioFactory
    {
        Task<(Usuario Usuario, string PlainPassword)> Create(UserRequest request);
    }
}
