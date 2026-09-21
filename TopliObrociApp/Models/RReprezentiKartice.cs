using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopliObrociApp.Models;

public class RReprezentiKartice
{
    [Key] [Column("ID_STAVKE")] public long IdStavke { get; init; }

    [Column("ID_REPREZENTA")] public long IdReprezenta { get; init; }

    [Column("DATUM")] public DateTime Datum { get; init; }

    [Column("ID_DOKUMENTA_VEZE")] public long? IdDokumentaVeze { get; init; }

    [Column("OPIS")] public string? Opis { get; init; }

    [Column("SIF_VRSTE_DOKUMENTA_VEZE")] public string? SifVrsteDokumentaVeze { get; init; }

    [Column("IZNOS")] public decimal? Iznos { get; init; }

    [Column("STORNO")] public int Storno { get; init; }

    [Column("ID_FISKALNOG_PERIODA")] public long IdFiskalnogPerioda { get; init; }
}