using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Cats_Cafe_Accounting_System.Utilities
{
    public class ApplicationDbContext : DbContext
    {
        private static IConfiguration? _configuration { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public static ApplicationDbContext CreateDatabase()
        {
            if (_configuration == null)
            {
                return CreateInMemoryDatabase();
            }
            else
            {
                return CreateMySqlDatabase();
            }
        }

        public static ApplicationDbContext CreateInMemoryDatabase()
        {
            var con = _configuration;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            return new ApplicationDbContext(options, null);
        }

        public static ApplicationDbContext CreateMySqlDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;

            return new ApplicationDbContext(options, _configuration);
        }

        public DbSet<PetModel> Pets { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<JobModel> Jobs { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<VisitorModel> Visitors { get; set; }
        public DbSet<TicketModel> Tickets { get; set; }
        public DbSet<VisitLogEntryModel> VisitLogEntries { get; set; }
    }
}