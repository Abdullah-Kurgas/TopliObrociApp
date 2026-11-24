using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using TopliObrociApp.Models;

namespace TopliObrociApp.Services;

public class UpdateService
{
    private const string UpdatePath = @"\\srv02\TopliObrociDB\version.json";

    public async Task<UpdateInfo?> CheckForUpdateAsync()
    {
        try
        {
            if (!File.Exists(UpdatePath)) return null;

            var json = await File.ReadAllTextAsync(UpdatePath);
            var update = JsonSerializer.Deserialize<UpdateInfo>(json);

            if (update == null || !Version.TryParse(update.Version, out var latestVersion)) return null;
            var currentVersion = Assembly.GetEntryAssembly()?.GetName().Version ?? new Version(1, 0, 0);

            return latestVersion > currentVersion ? update : null;
        }
        catch
        {
            return null;
        }
    }
}