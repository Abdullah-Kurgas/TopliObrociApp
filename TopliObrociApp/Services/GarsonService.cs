using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TopliObrociApp.Data;
using TopliObrociApp.DTOs;
using TopliObrociApp.Models;

namespace TopliObrociApp.Services;

public class GarsonService
{
    private readonly GarsonDbContext _dbContext = new();

    public async Task<List<RReprezentiKartice>> GetRacuniAsync(DateTime mjesec, long idReprezenta)
    {
        var pocetak = new DateTime(mjesec.Year, mjesec.Month, 1);
        var kraj = pocetak.AddMonths(1);

        var kartice = await _dbContext.R_REPREZENTI_KARTICE
            .AsNoTracking()
            .Where(x =>
                x.IdReprezenta == idReprezenta &&
                x.Datum >= pocetak &&
                x.Datum < kraj)
            .OrderByDescending(x => x.Datum)
            .ToListAsync();

        var stornoIdIdDokumentaVeze = kartice
            .Where(x => x.Opis != null &&
                        x.Opis.StartsWith("STORNO:"))
            .Select(x => x.IdDokumentaVeze)
            .ToHashSet();

        var racuni = kartice
            .Where(x => x.Opis == null ||
                        !x.Opis.StartsWith("STORNO:"))
            .ToList();

        foreach (var racun in racuni.Where(racun => stornoIdIdDokumentaVeze.Contains(racun.IdDokumentaVeze)))
            racun.Storno = 1;

        return racuni;
    }

    public async Task<decimal> GetUkupnoAsync(DateTime mjesec, long idReprezenta)
    {
        var pocetak = new DateTime(mjesec.Year, mjesec.Month, 1);
        var kraj = pocetak.AddMonths(1);

        return await _dbContext.R_REPREZENTI_KARTICE
            .AsNoTracking()
            .Where(x =>
                x.IdReprezenta == idReprezenta &&
                x.Datum >= pocetak &&
                x.Datum < kraj)
            .SumAsync(x => x.Iznos ?? 0);
    }

    public async Task<List<CReprezent>> GetReprezenteByNameAsync(string search)
    {
        return await _dbContext.C_REPREZENTI
            .Where(x =>
                EF.Functions.Like(x.Ime.ToLower(), $"%{search.ToLower()}%") &&
                x.Aktivan == 1)
            .Take(10)
            .ToListAsync();
    }

    public async Task<List<RacunStavkaDTO>> GetStavkeRacunaAsync(
        long idRacuna)
    {
        return await _dbContext.R_RACUNI_STAVKE
            .AsNoTracking()
            .Where(x => x.IdRacuna == idRacuna)
            .Join(
                _dbContext.C_ARTIKLI.AsNoTracking(),
                stavka => stavka.IdArtikla,
                artikal => artikal.IdArtikla,
                (stavka, artikal) => new RacunStavkaDTO
                {
                    IdArtikla = stavka.IdArtikla,
                    NazivArtikla = artikal.Naziv,
                    Kolicina = stavka.Kolicina ?? 0,
                    IznosSaPopustom = stavka.IznosSaPopustom ?? 0,
                    Storno = stavka.Storno ?? 0
                })
            .ToListAsync();
    }
}