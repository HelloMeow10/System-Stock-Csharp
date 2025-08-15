namespace BusinessLogic.Security
{
    public interface IPasswordHasher
    {
        byte[] Hash(string username, string password);
    }
}
