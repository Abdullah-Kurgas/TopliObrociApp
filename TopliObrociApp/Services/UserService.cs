using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TopliObrociApp.Configuration;
using TopliObrociApp.Models;

namespace TopliObrociApp.Services;

public class UserService
{
    private readonly string _filePath = AppSettings.UsersFilePath;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public List<User> GetUsers()
    {
        if (!File.Exists(_filePath)) throw new FileNotFoundException("users.json nije pronađen.", _filePath);
        var json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json)) return [];

        return JsonSerializer.Deserialize<List<User>>(json, _jsonOptions) ?? [];
    }

    public async Task AddUserAsync(User user)
    {
        var users = GetUsers();

        if (users.Any(x => x.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Korisnik '{user.Username}' već postoji.");

        var nextId = users.Count == 0 ? 1 : users.Max(x => x.Id) + 1;

        var newUser = new User
        {
            Id = nextId,
            GarsonId = user.GarsonId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            Role = user.Role,
            IsActive = user.IsActive
        };

        users.Add(newUser);
        await SaveUsersAsync(users);
    }

    private async Task SaveUsersAsync(List<User> users)
    {
        var json = JsonSerializer.Serialize(users, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }
}