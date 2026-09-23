using System;

namespace TopliObrociApp.Configuration;

public static class AppSettings
{
    private const string ServerIp = "192.168.0.5";
    private const string ShareName = "TopliObrociDB";
    private const string FileName = "users.json";

    public static string UsersFilePath
    {
        get
        {
            if (OperatingSystem.IsWindows()) return $@"\\{ServerIp}\{ShareName}\{FileName}";
            if (OperatingSystem.IsMacOS()) return $"/Volumes/{ShareName}/{FileName}";

            throw new PlatformNotSupportedException("Operativni sistem nije podržan.");
        }
    }
}