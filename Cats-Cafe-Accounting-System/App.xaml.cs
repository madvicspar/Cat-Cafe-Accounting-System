using Cats_Cafe_Accounting_System.ViewModels;
using Cats_Cafe_Accounting_System.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Cats_Cafe_Accounting_System
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? Container { get; protected set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            //InitializeDependencies();

            //if (Container is null)
            //    throw new Exception(message: "smth ewnt wrong during initialising DI container");

            //OpenAuthorizationWindow();

            //base.OnStartup(e);
            InitContainer();
            if (Container is null)
                throw new Exception(message: "smth ewnt wrong during initialising DI container");
            MainWindowViewModel? MainVM = Container.GetService<MainWindowViewModel>() as MainWindowViewModel;
            var window = Container.GetService(typeof(MainWindow)) as MainWindow;
            if (window is null)
                throw new Exception(message: "smth went wrong during initialising DI container. MainWindow is missing");
            window.DataContext = MainVM;

            var authorizationViewModel = Container.GetService<AuthorizationViewModel>();
            var authorizationWindow = Container.GetService<AuthorizationView>();

            authorizationWindow.DataContext = authorizationViewModel;
            authorizationWindow.Closed += (sender, args) => OpenMainWindow();
            authorizationWindow.Show();

            base.OnStartup(e);
        }
        private static void InitializeDependencies()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<AuthorizationViewModel>();
            services.AddSingleton<AuthorizationView>();
            services.AddSingleton<PetsViewModel>();
            services.AddSingleton<NavigationViewModel>();
            Container = services.BuildServiceProvider();
        }

        private void OpenAuthorizationWindow()
        {
            var authorizationViewModel = Container.GetService<AuthorizationViewModel>();
            var authorizationWindow = Container.GetService<AuthorizationView>();

            authorizationWindow.DataContext = authorizationViewModel;
            authorizationWindow.Closed += (sender, args) => OpenMainWindow();
            authorizationWindow.Show();
        }

        private void OpenMainWindow()
        {
            //var mainViewModel = Container.GetService<MainWindowViewModel>();
            //var mainWindow = Container.GetService<MainWindow>();

            //mainWindow.DataContext = mainViewModel;
            //mainWindow.Show();

            var mainViewModel = new MainWindowViewModel(Container.GetService<NavigationViewModel>());
            var mainView = new MainWindow { DataContext = mainViewModel };
            mainView.Show();
        }
        private static void InitContainer()
        {
            ServiceCollection services = new();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<AuthorizationViewModel>();
            services.AddSingleton<AuthorizationView>();
            services.AddSingleton<NavigationViewModel>();
            services.AddSingleton<PetsViewModel>();
            services.AddSingleton<VisitorsViewModel>();
            Container = services.BuildServiceProvider();
        }
    }
}
