using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopliObrociApp.Models;

public class RArtikli
{
    [Key] [Column("ID_ARTIKLA")] public long IdArtikla { get; set; }
}