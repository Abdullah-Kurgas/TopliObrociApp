using TopliObrociApp.Enums;

namespace TopliObrociApp.Models;

public class User
{
    public int Id { get; set; }

    public int GarsonId { get; set; }

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public Role Role { get; set; }

    public bool IsActive { get; set; }
}