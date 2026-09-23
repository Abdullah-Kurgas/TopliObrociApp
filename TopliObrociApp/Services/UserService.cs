using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TopliObrociApp.Configuration;
using TopliObrociApp.Enums;
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

    public void AddUser(int garsonId, string username, string password, Role role)
    {
        var users = GetUsers();

        if (users.Any(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Korisnik sa tim username-om već postoji.");

        var newId = users.Count == 0 ? 1 : users.Max(x => x.Id) + 1;

        var user = new User
        {
            Id = newId,
            GarsonId = garsonId,
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = Role.Administrator,
            IsActive = true
        };

        users.Add(user);
        SaveUsers(users);
    }

    private void SaveUsers(List<User> users)
    {
        var json = JsonSerializer.Serialize(users, _jsonOptions);
        File.WriteAllText(_filePath, json);
    }
}