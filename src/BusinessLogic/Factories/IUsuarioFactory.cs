using BusinessLogic.Models;
using DataAccess.Entities;

namespace BusinessLogic.Factories
{
    public interface IUsuarioFactory
    {
        (Usuario Usuario, string PlainPassword) Create(UserRequest request);
    }
}
