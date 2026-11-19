using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CArtikli
{
    [Key] [Column("ID_ARTIKLA")] public long? IdArtikla { get; set; }

    [Column("SIF_ARTIKLA")] public string SifArtikla { get; set; }

    [Column("NAZIV")] public string Naziv { get; set; }

    [Column("NAZIV_NOR")] public string NazivNor { get; set; }

    [Column("ID_STOPE")] public long? IdStope { get; set; }

    [Column("ID_JED_MERE")] public long? IdJedMere { get; set; }

    [Column("BAR_CODE")] public string BarCode { get; set; }

    [Column("KLJUCNE_RECI")] public string KljucneReci { get; set; }

    [Column("AGREGACIONI_ARTIKAL")] public int? AgregacioniArtikal { get; set; }

    [Column("AGR_ISTA_STOPA")] public int? AgrIstaStopa { get; set; }

    [Column("AGR_ISTA_JED_MERE")] public int? AgrIstaJedMere { get; set; }

    [Column("FISKALNI_NAZIV")] public string FiskalniNaziv { get; set; }

    [Column("FISKALNI_NAZIV_NOR")] public string FiskalniNazivNor { get; set; }

    [Column("AKTIVAN")] public int? Aktivan { get; set; }

    [Column("ID_GRUPE_ARTIKLA")] public long? IdGrupeArtikla { get; set; }

    [Column("PRODAJE_SE")] public int? ProdajeSe { get; set; }

    [Column("ID_PROMETNE_GRUPE")] public long? IdPrometneGrupe { get; set; }

    [Column("POSLEDNJA_NAB_CENA")] public decimal? PoslednjaNabCena { get; set; }

    [Column("DATUM_POSLEDNJE_PROMENE")] public DateTime? DatumPoslednjePromene { get; set; }

    [Column("ID_NABAVNE_STOPE")] public long? IdNabavneStope { get; set; }

    [Column("NABAVLJA_SE")] public int? NabavljaSe { get; set; }

    [Column("ID_ARTIKAL_REVISION")] public long? IdArtikalRevision { get; set; }

    [Column("KALULATIVNA_NABAVNA_CENA")] // (u bazi piše KALULATIVNA)
    public decimal? KalulativnaNabavnaCena { get; set; }

    [Column("OSTALI_TROSKOVI")] public decimal? OstaliTroskovi { get; set; }

    [Column("PLANSKA_NABAVNA_CENA")] public decimal? PlanskaNabavnaCena { get; set; }

    [Column("PROCENAT_TROSKA")] public decimal? ProcenatTroska { get; set; }

    [Column("NABAVNA_CENA_SA_TROSKOVIMA")] public decimal? NabavnaCenaSaTroskovima { get; set; }

    [Column("ID_TROSKOVNE_GRUPE")] public long? IdTroskovneGrupe { get; set; }

    [Column("PLU")] public int? Plu { get; set; }

    [Column("EXTERNAL_KEY")] public string ExternalKey { get; set; }

    [Column("ID_REVIZIJE")] public long? IdRevizije { get; set; }

    [Column("ID_KOMITENTA_NABAVKE")] public long? IdKomitentaNabavke { get; set; }

    [Column("COMDATA_VRSTA_USLUGE")] public long? ComdataVrstaUsluge { get; set; }

    [Column("COMDATA_VRSTA_USLUGE_PPL")] public long? ComdataVrstaUslugePpl { get; set; }

    [Column("ID_VRSTE_ARTIKLA")] public long? IdVrsteArtikla { get; set; }

    [Column("ID_MODIFIKATOR_GRUPE")] public long? IdModifikatorGrupe { get; set; }

    [Column("KORISTI_MODIFIKATORE")] public int? KoristiModifikatore { get; set; }

    [Column("POSLEDNJA_NAB_CENA_PDV")] public decimal? PoslednjaNabCenaPdv { get; set; }

    [Column("NABAVNA_CENA_SA_TROSKOVIMA_PDV")]
    public decimal? NabavnaCenaSaTroskovimaPdv { get; set; }

    [Column("PLANSKA_NABAVNA_CENA_PDV")] public decimal? PlanskaNabavnaCenaPdv { get; set; }

    [Column("USLUGA")] public int? Usluga { get; set; }

    [Column("OBAVEZNA_PROMENA_CENE")] public int? ObaveznaPromenaCene { get; set; }

    [Column("ID_GRUPE_KOMPLETNOSTI")] public long? IdGrupeKompletnosti { get; set; }

    [Column("PAKET")] public int? Paket { get; set; }

    [Column("DATUM_PROMENE_MIS")] public DateTime? DatumPromeneMis { get; set; }

    [Column("ID_STOPE_ZA_PONETI")] public long? IdStopeZaPoneti { get; set; }

    [Column("PRIVREMENO_NEAKTIVAN")] public int? PrivremenoNeaktivan { get; set; }

    [Column("GC_ID_RECEPTA")] public string GcIdRecepta { get; set; }

    [Column("GC_SREDNJA_NAB_CENA")] public decimal? GcSrednjaNabCena { get; set; }

    [Column("ID_JED_MERE_ALT")] public long? IdJedMereAlt { get; set; }

    [Column("DOMACI_PROIZVOD")] public int? DomaciProizvod { get; set; }

    [Column("MENI_ARTIKAL")] public int? MeniArtikal { get; set; }

    [Column("RUCNA_IZMENA_POS_NAB_CENE")] public int? RucnaIzmenaPosNabCene { get; set; }

    [Column("COMDATA_VRSTA_USLUGE_3")] public long? ComdataVrstaUsluge3 { get; set; }

    [Column("COMDATA_VRSTA_USLUGE_4")] public long? ComdataVrstaUsluge4 { get; set; }

    [Column("COMDATA_VRSTA_USLUGE_5")] public long? ComdataVrstaUsluge5 { get; set; }

    [Column("NE_STAMPAJ_NALOG")] public int? NeStampajNalog { get; set; }
}