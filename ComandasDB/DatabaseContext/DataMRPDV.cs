using System.Data.Entity;

namespace DatabaseScaffold
{
    public partial class DataMRPDV : DbContext
    {
        public DataMRPDV()
            : base("name=DataMRPDV")
        {
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
}
