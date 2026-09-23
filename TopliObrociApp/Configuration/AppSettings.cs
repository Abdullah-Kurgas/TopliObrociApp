using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace TopliObrociApp.Configuration;

public static class AppSettings
{
    private const string ServerIp = "192.168.0.5";
    private const string ShareName = "TopliObrociDB";
    private const string FileName = "users.json";

    private static string MacMountPoint => $"/Volumes/{ShareName}";

    public static string UsersFilePath
    {
        get
        {
            if (OperatingSystem.IsWindows()) return $@"\\{ServerIp}\{ShareName}\{FileName}";

            if (OperatingSystem.IsMacOS()) return Path.Combine(MacMountPoint, FileName);

            throw new PlatformNotSupportedException("Operativni sistem nije podržan.");
        }
    }

    public static async Task<bool> EnsureUsersShareMountedAsync()
    {
        if (!OperatingSystem.IsMacOS()) return true;
        if (IsShareMounted()) return true;

        try
        {
            Directory.CreateDirectory(MacMountPoint);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/sbin/mount_smbfs",
                    Arguments = $"//{ServerIp}/{ShareName} \"{MacMountPoint}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode == 0) return IsShareMounted();

            Console.WriteLine($"SMB mount error: {error}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Greška prilikom mountanja: {ex}");
            return false;
        }
    }

    private static bool IsShareMounted()
    {
        return Directory.Exists(MacMountPoint) && Directory.GetFileSystemEntries(MacMountPoint).Length > 0;
    }
}