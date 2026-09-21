using System.Configuration;
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
        optionsBuilder.UseFirebird(ConfigurationManager.ConnectionStrings["GarsonDB"].ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}