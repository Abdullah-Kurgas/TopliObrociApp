using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TopliObrociApp.Models;

namespace TopliObrociApp.Services;

public class AuthSessionStorage
{
    private readonly string _sessionFilePath;

    public AuthSessionStorage()
    {
        var appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "TopliObrociApp"
        );

        Directory.CreateDirectory(appDataPath);
        _sessionFilePath = Path.Combine(appDataPath, "session.json");
    }

    public async Task SaveSessionAsync(User user)
    {
        var json = JsonSerializer.Serialize(user);
        await File.WriteAllTextAsync(_sessionFilePath, json);
    }

    public async Task<User?> LoadSessionAsync()
    {
        if (!File.Exists(_sessionFilePath)) return null;

        var json = await File.ReadAllTextAsync(_sessionFilePath);

        return JsonSerializer.Deserialize<User>(json);
    }

    public Task ClearSessionAsync()
    {
        if (File.Exists(_sessionFilePath)) File.Delete(_sessionFilePath);

        return Task.CompletedTask;
    }
}