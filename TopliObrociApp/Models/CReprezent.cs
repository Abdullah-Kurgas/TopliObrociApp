using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopliObrociApp.Models;

public class CReprezent
{
    [Key] [Column("ID_REPREZENTA")] public long IdReprezenta { get; init; }

    [Column("SIF_REPREZENTA")] public string SifReprezenta { get; init; } = string.Empty;

    [Column("IME")] public string Ime { get; init; } = string.Empty;

    [Column("AKTIVAN")] public int Aktivan { get; init; }
}