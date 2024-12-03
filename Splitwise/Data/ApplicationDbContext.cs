using Microsoft.EntityFrameworkCore;
using Splitwise.Models;

namespace Splitwise.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupDetail> GroupDetails { get; set; }
        public DbSet<ExpenseDetail> ExpenseDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Group>()
            //    .HasMany(g => g.Users)
            //    .UsingEntity(j => j.ToTable("GroupUser"));

            modelBuilder.Entity<Balance>()
                .Property(b => b.Amount)
                .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<ExpenseDetail>()
                .Property(b => b.Amount)
                .HasColumnType("decimal(18,2)");

            //modelBuilder.Entity<Expense>()
            //    .HasOne(u => u.ExpenseDetails)
            //    .WithOne(ed => ed.Expense)
            //    .HasForeignKey<ExpenseDetail>(ed => ed.ExpenseId);
        }
    }
}
