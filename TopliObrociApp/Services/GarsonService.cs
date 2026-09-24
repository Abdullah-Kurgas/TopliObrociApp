using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TopliObrociApp.Data;
using TopliObrociApp.Models;

namespace TopliObrociApp.Services;

public class GarsonService
{
    private readonly GarsonDbContext _dbContext = new();

    public async Task<List<RReprezentiKartice>> GetRacuniAsync(DateTime mjesec, long idReprezenta)
    {
        var pocetak = new DateTime(mjesec.Year, mjesec.Month, 1);
        var kraj = pocetak.AddMonths(1);

        return await _dbContext.R_REPREZENTI_KARTICE
            .AsNoTracking()
            .Where(x =>
                x.IdReprezenta == idReprezenta &&
                x.Datum >= pocetak &&
                x.Datum < kraj &&
                x.Storno == 0)
            .OrderByDescending(x => x.Datum)
            .ToListAsync();
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
                x.Datum < kraj &&
                x.Storno == 0)
            .SumAsync(x => x.Iznos ?? 0);
    }
}