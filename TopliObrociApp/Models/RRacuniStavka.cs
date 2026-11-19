using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class RRacuniStavka
{
    [Key] [Column("ID_STAVKE")] public long? IdStavke { get; set; }

    [Column("ID_RACUNA")] public long? IdRacuna { get; set; }

    [Column("ID_ARTIKLA")] public long? IdArtikla { get; set; }

    [Column("KOLICINA")] public decimal? Kolicina { get; set; }

    [Column("CENA")] public decimal? Cena { get; set; }

    [Column("CENA_SA_POPUSTOM")] public decimal? CenaSaPopustom { get; set; }

    [Column("STORNO")] public int? Storno { get; set; }

    [Column("IZNOS_SA_POPUSTOM")] public decimal? IznosSaPopustom { get; set; }
}