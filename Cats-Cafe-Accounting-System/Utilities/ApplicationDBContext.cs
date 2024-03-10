using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Cats_Cafe_Accounting_System.Utilities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        public DbSet<PetModel> Pets { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<JobModel> Jobs { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<VisitorModel> Visitors { get; set; }
        public DbSet<TicketModel> Tickets { get; set; }
        public DbSet<VisitLogEntryModel> VisitLogEntries { get; set;}
    }
}