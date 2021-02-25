using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ZCrypto.BLL.EF
{
    public partial class zcryptoContext : DbContext
    {
        public zcryptoContext()
        {
        }

        public zcryptoContext(DbContextOptions<zcryptoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Buy> Buys { get; set; }
        public virtual DbSet<Coin> Coins { get; set; }
        public virtual DbSet<CoinBlacklistIntegration> CoinBlacklistIntegrations { get; set; }
        public virtual DbSet<ReportedExchangeRate> ReportedExchangeRates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Database=zcrypto;Username=postgres;Password=SECRET:)");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "English_United States.1252");

            modelBuilder.Entity<Buy>(entity =>
            {
                entity.ToTable("buy");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.CoinId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("coin_id");

                entity.Property(e => e.Count)
                    .HasPrecision(10, 2)
                    .HasColumnName("count");

                entity.Property(e => e.CreatedTime)
                    .HasColumnType("date")
                    .HasColumnName("created_time")
                    .HasDefaultValueSql("CURRENT_DATE");

                entity.Property(e => e.ExchangeRatePln)
                    .HasPrecision(10, 2)
                    .HasColumnName("exchange_rate_pln");

                entity.HasOne(d => d.Coin)
                    .WithMany(p => p.Buys)
                    .HasForeignKey(d => d.CoinId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("buy_coin_id_fkey");
            });

            modelBuilder.Entity<Coin>(entity =>
            {
                entity.ToTable("coin");

                entity.Property(e => e.Id)
                    .HasMaxLength(255)
                    .HasColumnName("id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsNew).HasColumnName("is_new");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Rank).HasColumnName("rank");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("symbol");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<CoinBlacklistIntegration>(entity =>
            {
                entity.ToTable("coin_blacklist_integration");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CoinId)
                    .HasMaxLength(255)
                    .HasColumnName("coin_id");

                entity.Property(e => e.Reason)
                    .HasMaxLength(255)
                    .HasColumnName("reason");

                entity.HasOne(d => d.Coin)
                    .WithMany(p => p.CoinBlacklistIntegrations)
                    .HasForeignKey(d => d.CoinId)
                    .HasConstraintName("coin_blacklist_integration_coin_id_fkey");
            });

            modelBuilder.Entity<ReportedExchangeRate>(entity =>
            {
                entity.ToTable("reported_exchange_rate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApiUpdateTime)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("api_update_time");

                entity.Property(e => e.BuyId).HasColumnName("buy_id");

                entity.Property(e => e.ExchangeDiff).HasColumnName("exchange_diff");

                entity.Property(e => e.PlnExchange).HasColumnName("pln_exchange");

                entity.HasOne(d => d.Buy)
                    .WithMany(p => p.ReportedExchangeRates)
                    .HasForeignKey(d => d.BuyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reported_exchange_rate_buy_id_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
