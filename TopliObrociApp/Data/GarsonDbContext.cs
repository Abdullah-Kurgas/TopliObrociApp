using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace TopliObrociApp.Data;

public class GarsonDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseFirebird(ConfigurationManager.ConnectionStrings["GarsonDB"].ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    // public DbSet<Reprezent> C_REPREZENTI { get; set; }
    // public DbSet<Objekat> C_OBJEKTI { get; set; }
    //
    // public DbSet<ReprezentR> R_REPREZENTI { get; set; }
    // public DbSet<ReprezentiKarticeR> R_REPREZENTI_KARTICE { get; set; }
}