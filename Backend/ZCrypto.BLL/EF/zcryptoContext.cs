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
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<LowPriceSymbolScan> LowPriceSymbolScans { get; set; }
        public virtual DbSet<LowPriceSymbolScanAlert> LowPriceSymbolScanAlerts { get; set; }
        public virtual DbSet<ReportedExchangeRate> ReportedExchangeRates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=secret;Database=zcrypto;Username=postgres;Password=secret");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.UTF-8");

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
                    .HasPrecision(13, 5)
                    .HasColumnName("count");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ExchangeRatePln)
                    .HasPrecision(13, 5)
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

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("log");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(8046)
                    .HasColumnName("message");
            });

            modelBuilder.Entity<LowPriceSymbolScan>(entity =>
            {
                entity.ToTable("low_price_symbol_scan");

                entity.HasIndex(e => e.Symbol, "UQ_Symbol")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LastKlines).HasColumnName("last_klines");

                entity.Property(e => e.PercentThreshold).HasColumnName("percent_threshold");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasColumnName("symbol");
            });

            modelBuilder.Entity<LowPriceSymbolScanAlert>(entity =>
            {
                entity.ToTable("low_price_symbol_scan_alert");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActiveDuration).HasColumnName("active_duration");

                entity.Property(e => e.LowPriceSymbolScanId).HasColumnName("low_price_symbol_scan_id");

                entity.Property(e => e.PercentLow)
                    .HasPrecision(10, 3)
                    .HasColumnName("percent_low");

                entity.HasOne(d => d.LowPriceSymbolScan)
                    .WithMany(p => p.LowPriceSymbolScanAlerts)
                    .HasForeignKey(d => d.LowPriceSymbolScanId)
                    .HasConstraintName("low_price_symbol_scan_alert_low_price_symbol_scan_id_fkey");
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
