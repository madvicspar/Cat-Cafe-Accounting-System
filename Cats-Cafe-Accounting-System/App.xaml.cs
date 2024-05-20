using Cats_Cafe_Accounting_System.Utilities;
using Cats_Cafe_Accounting_System.ViewModels;
using Cats_Cafe_Accounting_System.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Cats_Cafe_Accounting_System
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class App : Application
    {
        private static void OpenMainWindow()
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
            CheckImages();
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

        private void CheckImages()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string destinationFolder = Path.Combine(baseDirectory, "Images");
            string sourceFolder = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(baseDirectory)))), "Images");

            CopyFilesFromSourceToDestination(sourceFolder, destinationFolder);

            string usersPhotoSourceFolder = Path.Combine(sourceFolder, "UsersPhoto");
            string usersPhotoDestinationFolder = Path.Combine(destinationFolder, "UsersPhoto");

            CopyFilesFromSourceToDestination(usersPhotoSourceFolder, usersPhotoDestinationFolder);
        }

        static void CopyFilesFromSourceToDestination(string sourceFolder, string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            var files = Directory.GetFiles(sourceFolder);

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destinationFilePath = Path.Combine(destinationFolder, fileName);
                File.Copy(file, destinationFilePath, true);
            }
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
            services.AddSingleton<EmployeesViewModel>(new EmployeesViewModel(_dbContext));
            services.AddSingleton<TicketsViewModel>(new TicketsViewModel(_dbContext));
            services.AddSingleton<PetTransferLogViewModel>(new PetTransferLogViewModel(_dbContext));
            services.AddSingleton<EmployeeShiftLogViewModel>(new EmployeeShiftLogViewModel(_dbContext));
            services.AddSingleton<VisitLogViewModel>(new VisitLogViewModel(_dbContext));
            services.AddSingleton<PersonalAreaViewModel>(new PersonalAreaViewModel(_dbContext));
            services.AddSingleton<IncomesViewModel>(new IncomesViewModel(_dbContext));
            services.AddSingleton<PopularPetsViewModel>(new PopularPetsViewModel(_dbContext));
            services.AddSingleton<AdmirerandomCatsViewModel>();
            Container = services.BuildServiceProvider();
        }

        public static void InitializeApplication()
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
            //var authorizationWindow = Container.GetService<AuthorizationView>();
            var authorizationWindow = new AuthorizationView();

            authorizationWindow.DataContext = authorizationViewModel;
            authorizationWindow.Closed += (sender, args) => OpenMainWindow();
            authorizationWindow.Show();
        }
    }
}
