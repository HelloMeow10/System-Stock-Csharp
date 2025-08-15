using System.Security.Cryptography;
using System.Text;

namespace BusinessLogic.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public byte[] Hash(string username, string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + username;
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            }
        }
    }
}
