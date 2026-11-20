using System.ComponentModel;
using System.Reflection;

namespace TopliObrociApp.Enums;

public static class PoslovnicaExtensions
{
    public static string GetNaziv(this Poslovnica poslovnica)
    {
        var field = typeof(Poslovnica).GetField(poslovnica.ToString());
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

        return attribute?.Description ?? poslovnica.ToString();
    }
}