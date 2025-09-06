using System.Collections.Generic;

namespace BusinessLogic.Models
{
    public class ChangePasswordRequest
    {
        public string Username { get; set; } = string.Empty;
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class RecoverPasswordRequest
    {
        public string Username { get; set; } = string.Empty;
        public Dictionary<int, string> Answers { get; set; } = new Dictionary<int, string>();
    }
}
