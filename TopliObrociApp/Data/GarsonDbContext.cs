using Microsoft.EntityFrameworkCore;
using TopliObrociApp.Models;

namespace TopliObrociApp.Data;

public class GarsonDbContext : DbContext
{
    public DbSet<CReprezent> C_REPREZENTI { get; set; }
    // public DbSet<Objekat> C_OBJEKTI { get; set; }

    public DbSet<RReprezentiKartice> R_REPREZENTI_KARTICE { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString =
            @"User=mbauser;Password=CzBruVpGHi;Database=C:\Program Files (x86)\2D Soft\Garson BackOffice 6\Database\BACKOFFICE - Slatkoislano.FDB;DataSource=192.168.0.32;Port=3054;Dialect=3;Charset=UTF8";

        optionsBuilder.UseFirebird(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}