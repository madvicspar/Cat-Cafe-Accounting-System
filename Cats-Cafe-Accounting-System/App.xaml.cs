using Cats_Cafe_Accounting_System.Utilities;
using Cats_Cafe_Accounting_System.ViewModels;
using Cats_Cafe_Accounting_System.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Cats_Cafe_Accounting_System
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OpenMainWindow()
        {
            var mainViewModel = new MainWindowViewModel(Container.GetService<NavigationViewModel>(), _dbContext);
            var mainView = new MainWindow { DataContext = mainViewModel };

            var personalAreaViewModel = Container.GetService<PersonalAreaViewModel>();
            personalAreaViewModel.Employee = Data.user;

            mainView.Show();
        }

        public static IServiceProvider? Container { get; protected set; }
        public static IConfiguration configuration { get; protected set; }

        public static ApplicationDbContext _dbContext { get; protected set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(ConfigurationManager.AppSettings.AllKeys.ToDictionary(key => key, key => ConfigurationManager.AppSettings[key]))
                .Build();

            var connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            var dbContext = new ApplicationDbContext(optionsBuilder.Options, configuration);
            _dbContext = ApplicationDbContext.CreateDatabase();

            InitContainer();
            InitializeApplication();
        }
        private static void InitContainer()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            ServiceCollection services = new();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddDbContext<ApplicationDbContext>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<AuthorizationViewModel>();
            services.AddSingleton<AuthorizationView>();
            services.AddSingleton<NavigationViewModel>();
            services.AddSingleton(new PetsViewModel(_dbContext));
            services.AddSingleton<VisitorsViewModel>(new VisitorsViewModel(_dbContext));
            services.AddSingleton<JobsViewModel>(new JobsViewModel(_dbContext));
            services.AddSingleton<EmployeesViewModel>();
            services.AddSingleton<TicketsViewModel>();
            services.AddSingleton<PetTransferLogViewModel>();
            services.AddSingleton<EmployeeShiftLogViewModel>();
            services.AddSingleton<VisitLogViewModel>(new VisitLogViewModel(_dbContext));
            services.AddSingleton<PersonalAreaViewModel>(new PersonalAreaViewModel(_dbContext));
            services.AddSingleton<IncomesViewModel>(new IncomesViewModel(_dbContext));
            services.AddSingleton<PopularPetsViewModel>(new PopularPetsViewModel(_dbContext));
            Container = services.BuildServiceProvider();
        }

        private void InitializeApplication()
        {
            if (Container is null)
            {
                throw new Exception("Something went wrong during initializing DI container");
            }

            var mainViewModel = new MainWindowViewModel(Container.GetService<NavigationViewModel>(), _dbContext);
            var window = Container.GetService(typeof(MainWindow)) as MainWindow;
            var mainView = new MainWindow() { DataContext = mainViewModel };

            var personalAreaViewModel = Container.GetService<PersonalAreaViewModel>();
            personalAreaViewModel.Employee = Data.user;

            var authorizationViewModel = new AuthorizationViewModel(_dbContext);
            var authorizationWindow = Container.GetService<AuthorizationView>();

            authorizationWindow.DataContext = authorizationViewModel;
            authorizationWindow.Closed += (sender, args) => OpenMainWindow();
            authorizationWindow.Show();
        }
    }
}
