using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TopliObrociApp.Configuration;

public static class AppSettings
{
    private const string ServerIp = "192.168.0.5";
    private const string ShareName = "TopliObrociDB";
    private const string FileName = "users.json";

    private static string MacMountPoint => Path.Combine("/Volumes", ShareName);

    public static string UsersFilePath
    {
        get
        {
            if (OperatingSystem.IsWindows()) return $@"\\{ServerIp}\{ShareName}\{FileName}";

            return OperatingSystem.IsMacOS()
                ? Path.Combine(MacMountPoint, FileName)
                : throw new PlatformNotSupportedException("Operativni sistem nije podržan.");
        }
    }

    public static async Task<bool> EnsureUsersShareMountedAsync()
    {
        if (!OperatingSystem.IsMacOS()) return true;
        if (IsShareMounted()) return true;

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "/usr/bin/osascript",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            psi.ArgumentList.Add("-e");
            psi.ArgumentList.Add($"mount volume \"smb://{ServerIp}/{ShareName}\"");

            using var process = new Process();
            process.StartInfo = psi;
            process.Start();

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            var errorTask = process.StandardError.ReadToEndAsync(cts.Token);
            var outputTask = process.StandardOutput.ReadToEndAsync(cts.Token);

            Console.Write("test");
            try
            {
                await process.WaitForExitAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                try
                {
                    process.Kill(true);
                }
                catch
                {
                    /* ignore */
                }

                Console.WriteLine("SMB mount timed out (possibly waiting on a credentials dialog).");
                return false;
            }

            var error = await errorTask;
            await outputTask;

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
        try
        {
            return File.Exists(Path.Combine(MacMountPoint, FileName));
        }
        catch
        {
            return false;
        }
    }
}