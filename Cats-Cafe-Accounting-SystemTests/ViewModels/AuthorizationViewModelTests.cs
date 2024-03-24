using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using Cats_Cafe_Accounting_System.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cats_Cafe_Accounting_System.ViewModels.Tests
{
    [TestClass()]
    public class AuthorizationViewModelTests
    {
        private ServiceProvider _serviceProvider;
        private ApplicationDbContext _dbContext;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();

            // Лучше использовать фейковый DbContext для тестирования
            _dbContext = ApplicationDbContext.CreateInMemoryDatabase();

            // Добавление фейковых данных в таблицы Genders, Breeds, Statuses
            _dbContext.Genders.Add(new Gender { Title = "мужской" });
            _dbContext.Genders.Add(new Gender { Title = "женский" });

            _dbContext.Statuses.Add(new Status { Title = "чилится" });
            _dbContext.Statuses.Add(new Status { Title = "не числится" });

            _dbContext.Breeds.Add(new Breed { Title = "сиамская", Id = "SIA" });
            _dbContext.Breeds.Add(new Breed { Title = "мейн-кун", Id = "MNC" });

            _dbContext.SaveChanges();

            services.AddSingleton(_dbContext);
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<AuthorizationViewModel>();
            services.AddSingleton<AuthorizationView>();
            services.AddSingleton<NavigationViewModel>();
            services.AddSingleton(new PetsViewModel(_dbContext));
            services.AddSingleton<VisitorsViewModel>();
            services.AddSingleton<JobsViewModel>(new JobsViewModel(_dbContext));
            services.AddSingleton<EmployeesViewModel>();
            services.AddSingleton<TicketsViewModel>();
            services.AddSingleton<PetTransferLogViewModel>();
            services.AddSingleton<EmployeeShiftLogViewModel>();
            services.AddSingleton<VisitLogViewModel>(new VisitLogViewModel(_dbContext));
            services.AddSingleton<PersonalAreaViewModel>(new PersonalAreaViewModel(_dbContext));
            services.AddSingleton<IncomesViewModel>(new IncomesViewModel(_dbContext));

            _serviceProvider = services.BuildServiceProvider();

        }

        [TestMethod()]
        public void ExecuteAddPetCommand_ShouldAddNewPet()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>(); // Используем Dependency Injection

                // Arrange
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test";

                // Act
                petsViewModel.ExecuteAddPetCommand();

                // Assert
                Assert.IsTrue(petsViewModel.Items.Count == 1);
            }
        }
    }
}