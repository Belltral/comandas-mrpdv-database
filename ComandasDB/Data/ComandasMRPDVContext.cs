using ComandasDB.Data.Internal;
using System;
using System.Data.Entity;
using System.IO;

public partial class ComandasMRPDVContext : DbContext
{
    public ComandasMRPDVContext()
    : base("name=ComandasMRPDV")
    {
        string dataDirectory = AppDomain.CurrentDomain.BaseDirectory;

        if (!File.Exists($@"{dataDirectory}\App_Data\ComandasMRPDV.MDF"))
        {
            try
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);

                Database.SetInitializer(new CreateDatabaseIfNotExists<ComandasMRPDVContext>());
                Database.Initialize(false);

                if (File.Exists($@"{dataDirectory}\App_Data\ComandasMRPDV.MDF"))
                {
                    RetrieveFromMRPDV.RetriveCurrentDataFromMrToComandasDatabase();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
            }
        }

    }

    public virtual DbSet<ItensPreVendas> ItensPreVendas { get; set; }
    public virtual DbSet<PreVendas> PreVendas { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItensPreVendas>()
            .Property(e => e.QTDE_IPRV)
            .HasPrecision(10, 3);

        modelBuilder.Entity<ItensPreVendas>()
            .Property(e => e.PRECO_IPRV)
            .HasPrecision(12, 3);

        modelBuilder.Entity<PreVendas>()
            .Property(e => e.VALOR_PRVD)
            .HasPrecision(12, 2);

        modelBuilder.Entity<PreVendas>()
            .Property(e => e.ACRESCIMO_PRVD)
            .HasPrecision(12, 2);

        modelBuilder.Entity<PreVendas>()
            .Property(e => e.TXENTR_PRVD)
            .HasPrecision(12, 2);

        modelBuilder.Entity<PreVendas>()
            .Property(e => e.DESCONTO_PRVD)
            .HasPrecision(12, 2);

        modelBuilder.Entity<PreVendas>()
            .Property(e => e.DESCPRMN_PRVD)
            .HasPrecision(12, 2);

        modelBuilder.Entity<PreVendas>()
            .Property(e => e.DESCPRAU_PRVD)
            .HasPrecision(12, 2);
    }
}
