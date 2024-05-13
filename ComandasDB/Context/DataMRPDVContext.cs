using ComandasDB;
using System.Data.Entity;

public partial class DataMRPDVContext : DbContext
{
    public DataMRPDVContext()
        : base("name=DataMRPDV")
    {
    }

    public virtual DbSet<ItensPreVenda> ItensPreVendas { get; set; }
    public virtual DbSet<PreVenda> PreVendas { get; set; }
    public virtual DbSet<Parametros> Parametros { get; set; }
    public virtual DbSet<Pdvs> Pdvs { get; set; }

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
