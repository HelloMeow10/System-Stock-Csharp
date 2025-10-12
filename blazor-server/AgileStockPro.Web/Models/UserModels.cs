using System.ComponentModel.DataAnnotations;

namespace AgileStockPro.Web.Models;

public class SecurityPolicy
{
    public int MinLength { get; set; } = 8;
    public int QuestionsCount { get; set; } = 2; // 2 – 3 – 5
    public bool RequireUpperLower { get; set; } = true;
    public bool RequireNumber { get; set; } = true;
    public bool RequireSpecial { get; set; } = true;
    public bool Require2FA { get; set; } = false; // via email simulation
    public bool PreventReuse { get; set; } = true; // keep history
    public bool CheckPersonalData { get; set; } = true; // not contain name/last name/birthdate
    public int MaxFailedAttempts { get; set; } = 5;
    public int LockoutMinutes { get; set; } = 5;
}

public class SecurityQuestion
{
    [Required]
    public string Question { get; set; } = string.Empty;
    [Required]
    public string AnswerHash { get; set; } = string.Empty; // SHA256(answer)
}

public class PasswordHistoryEntry
{
    public string Hash { get; set; } = string.Empty; // SHA256(username + password)
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}

public class AppUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Username { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? Birthdate { get; set; }
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }

    // Stored as SHA256(username + password)
    public string PasswordHash { get; set; } = string.Empty;
    public bool MustChangePassword { get; set; } = true;

    public List<SecurityQuestion> SecurityQuestions { get; set; } = new();
    public List<PasswordHistoryEntry> PasswordHistory { get; set; } = new();

    // Lockout/Attempts (frontend-only)
    public int FailedAttempts { get; set; }
    public DateTime? LockoutUntil { get; set; }
}

public static class SecurityQuestionsCatalog
{
    public static readonly string[] Default = new[]
    {
        "¿Cuál fue el nombre de tu primera mascota?",
        "¿En qué ciudad naciste?",
        "¿Cuál es tu comida favorita?",
        "¿Cómo se llamaba tu escuela primaria?",
        "¿Cuál es el segundo nombre de tu madre?"
    };
}
