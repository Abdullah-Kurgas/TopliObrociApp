namespace TopliObrociApp.DTOs;

public class RacunStavkaDTO
{
    public long? IdArtikla { get; set; }
    public string? NazivArtikla { get; set; }
    public decimal Kolicina { get; set; }
    public decimal Iznos { get; set; }
    public decimal IznosSaPopustom { get; set; }
    public int? Storno { get; set; }

    public string Status => Storno == 1 ? "Storno" : "";
}