using TopliObrociApp.Enums;

namespace TopliObrociApp.Models;

public class User
{
    public int Id { get; init; }
    public int GarsonId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string PasswordHash { get; init; } = string.Empty;
    public Role Role { get; init; }
    public bool IsActive { get; init; }

    public string FullName => $"{FirstName} {LastName}";
    public string Initial => string.IsNullOrEmpty(FirstName) ? "?" : FirstName[0].ToString().ToUpper();
}