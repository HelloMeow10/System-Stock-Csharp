using System.Security.Cryptography;
using System.Text;

namespace BusinessLogic.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public byte[] Hash(string username, string password)
        {
            // Use Argon2id for modern security
            // Requirement: Use username in the process. We use it as the salt.
            // In a perfect world, we'd use a random salt, but we follow the requirement to "attach username".
            var salt = Encoding.UTF8.GetBytes(username);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            using (var argon2 = new Konscious.Security.Cryptography.Argon2id(passwordBytes))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = 8; // 8 cores
                argon2.MemorySize = 65536; // 64 MB
                argon2.Iterations = 4;

                return argon2.GetBytes(32); // 32 bytes hash
            }
        }
    }
}
