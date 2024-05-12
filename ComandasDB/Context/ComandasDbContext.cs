using ComandasDB.Data.Internal;
using System;
using System.Data.Entity;
using System.IO;

namespace ComandasDB.Context
{
    public partial class ComandasDbContext : DbContext
    {
        public ComandasDbContext() : base("name=ComandasMRPDV")
        {
            string dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

            if (!File.Exists($@"{dataDirectory}\ComandasMRPDV.MDF"))
            {
                try
                {
                    AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);

                    Database.SetInitializer(new CreateDatabaseIfNotExists<ComandasDbContext>());
                    Database.Initialize(false);

                    if (File.Exists($@"{dataDirectory}\ComandasMRPDV.MDF"))
                    {
                        RetrieveFromMRPDV.RetriveFromMRToComandas();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        public virtual DbSet<ItensPreVenda> ItensPreVendas { get; set; }
        public virtual DbSet<PreVenda> PreVendas { get; set; }
        //public virtual DbSet<Parametros> Parametros { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItensPreVenda>()
                .Property(e => e.QTDE_IPRV)
                .HasPrecision(10, 3);

            modelBuilder.Entity<ItensPreVenda>()
                .Property(e => e.PRECO_IPRV)
                .HasPrecision(12, 3);

            modelBuilder.Entity<PreVenda>()
                .Property(e => e.VALOR_PRVD)
                .HasPrecision(12, 2);

            modelBuilder.Entity<PreVenda>()
                .Property(e => e.ACRESCIMO_PRVD)
                .HasPrecision(12, 2);

            modelBuilder.Entity<PreVenda>()
                .Property(e => e.TXENTR_PRVD)
                .HasPrecision(12, 2);

            modelBuilder.Entity<PreVenda>()
                .Property(e => e.DESCONTO_PRVD)
                .HasPrecision(12, 2);

            modelBuilder.Entity<PreVenda>()
                .Property(e => e.DESCPRMN_PRVD)
                .HasPrecision(12, 2);

            modelBuilder.Entity<PreVenda>()
                .Property(e => e.DESCPRAU_PRVD)
                .HasPrecision(12, 2);

            modelBuilder.Entity<PreVenda>()
                .Property(e => e.PEDIDOKDS_PRVD)
                .IsUnicode(false);
        }
    }
}
