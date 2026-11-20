using System;
using System.Linq;

namespace TopliObrociApp.Enums;

public static class PoslovnicaParser
{
    public static Poslovnica? Parse(string? opis)
    {
        if (string.IsNullOrWhiteSpace(opis)) return null;
        var markerIndex = opis.IndexOf("br.", StringComparison.OrdinalIgnoreCase);

        if (markerIndex < 0) return null;

        var ostatak = opis[(markerIndex + 3)..].Trim();
        var sifra = ostatak
            .Split(['/', ' '], StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault();

        if (sifra == null) return null;
        if (Enum.TryParse<Poslovnica>(sifra, true, out var poslovnica)) return poslovnica;

        return null;
    }
}