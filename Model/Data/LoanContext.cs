using FinalProjectLoans.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectLoans.Model.Data
{
    public class LoanContext : DbContext

    {
        public DbSet<Accountant> Accountants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=LoansDB;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=False");
        }
    }
}
