using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        
        public DbSet<Property> Properties { get; set; }
        public DbSet<SalesTransaction> SalesTransactions { get; set; }
        public DbSet<BusinessPartner> BusinessPartners { get; set; }
        public DbSet<SalesProponent> SalesProponents { get; set; }
        public DbSet<ReservationFee> ReservationFees { get; set; }
        public DbSet<CreditReview> CreditReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesTransaction>()
                .HasOne(st => st.Properties)
                .WithMany(p => p.SalesTransaction)
                .HasForeignKey(st => st.PropertyId);
            modelBuilder.Entity<SalesTransaction>()
                .HasOne(st => st.BusinessPartner)
                .WithMany(bp => bp.SalesTransaction)
                .HasForeignKey(st => st.BusinessPartnerId);
            modelBuilder.Entity<SalesTransaction>()
                .HasOne(st => st.SalesProponent)
                .WithMany(sp => sp.SalesTransaction)
                .HasForeignKey(st => st.SalesProponentsId);
            modelBuilder.Entity<SalesTransaction>()
                .HasOne(st => st.ReservationFee)
                .WithOne(rf => rf.SalesTransaction)
                .HasForeignKey<SalesTransaction>(st => st.ReservationFeeId);
            modelBuilder.Entity<SalesTransaction>()
                .HasOne(st => st.CreditReview)
                .WithMany(cr => cr.SalesTransaction)
                .HasForeignKey(bp => bp.CreditReviewId);
        }
    }
    }

