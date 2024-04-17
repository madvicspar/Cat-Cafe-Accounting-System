using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cats_Cafe_Accounting_System.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Cats_Cafe_Accounting_System.Models;

namespace Cats_Cafe_Accounting_System.ViewModels.Tests
{
    [TestClass()]
    public class EmployeesViewModelTests
    {
        private ServiceProvider _serviceProvider;
        private static ApplicationDbContext _dbContext;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _dbContext = ApplicationDbContext.CreateInMemoryDatabase();

            _dbContext.Genders.Add(new Gender { Title = "женский" });
            _dbContext.Genders.Add(new Gender { Title = "мужской" });

            _dbContext.Jobs.Add(new JobModel { Title = "дворник", Rate = 100f });
            _dbContext.Jobs.Add(new JobModel { Title = "продавец", Rate = 200f });

            _dbContext.SaveChanges();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _dbContext.Dispose();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var services = new ServiceCollection();
            services.AddSingleton(_dbContext);
            services.AddSingleton(new EmployeesViewModel(_dbContext));
            _serviceProvider = services.BuildServiceProvider();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            foreach (var employee in _dbContext.Employees)
            {
                _dbContext.Employees.Remove(employee);
            }
            _dbContext.SaveChanges();
        }

        [TestMethod()]
        public void ExecuteAddEmployeeCommand_ShouldAddNewEmployee()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var employeesViewModel = scope.ServiceProvider.GetRequiredService<EmployeesViewModel>(); // Используем Dependency Injection

                // Arrange
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;

                // Act
                employeesViewModel.ExecuteAddEmployeeCommand();

                // Assert
                Assert.AreEqual(1, employeesViewModel.Items.Count);
                Assert.AreEqual(_dbContext.Employees.Count(), 1);
                Assert.AreEqual(employeesViewModel.FilterLastNames.Count(), 0);
                Assert.AreEqual(employeesViewModel.FilterItems.Count(), 2);
            }
        }

        [TestMethod()]
        public void ExecuteSearchLastNameCommandTest()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var employeesViewModel = scope.ServiceProvider.GetRequiredService<EmployeesViewModel>();

                // Arranges and Acts
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test1";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test2";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test3";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                employeesViewModel.SearchLastName = "Tes";
                employeesViewModel.ExecuteSearchLastNameCommand();
                // Assert
                Assert.AreEqual(3, employeesViewModel.FilterItems.Count-2);
            }
        }

        [TestMethod()]
        public void ExecuteSearchLastNameCommandTest_EmptySearchText()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var employeesViewModel = scope.ServiceProvider.GetRequiredService<EmployeesViewModel>();

                // Arranges and Acts
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test1";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                employeesViewModel.SearchLastName = "Поиск по имени";
                employeesViewModel.ExecuteSearchLastNameCommand();
                // Assert
                Assert.AreEqual("Поиск по имени", employeesViewModel.SearchLastName);
            }
        }

        [TestMethod()]
        public void ExecuteUpdateEmployeeCommandTest()
        {
            var employeesViewModel = _serviceProvider.GetRequiredService<EmployeesViewModel>();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 2].Item.LastName = "TestUpdated";
            employeesViewModel.ExecuteUpdateEmployeeCommand(employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 2].Item);
            Assert.AreEqual("TestUpdated", _dbContext.Employees.Last().LastName);
        }

        [TestMethod()]
        public void ExecuteDeleteEmployeeCommandTest()
        {
            var employeesViewModel = _serviceProvider.GetRequiredService<EmployeesViewModel>();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.ExecuteDeleteEmployeeCommand(_dbContext.Employees.First());
            Assert.AreEqual(_dbContext.Employees.Count(), 0);
            Assert.AreEqual(employeesViewModel.LastNames.Count(), 0);
            Assert.AreEqual(employeesViewModel.FilterLastNames.Count(), 0);
            Assert.AreEqual(employeesViewModel.FilterItems.Count(), 1);
        }
        [TestMethod()]
        public void ExecuteDeleteEmployeeCommandTest_NoElements()
        {
            var employeesViewModel = _serviceProvider.GetRequiredService<EmployeesViewModel>();
            employeesViewModel.ExecuteDeleteEmployeeCommand(employeesViewModel.FilterItems.Last().Item);
            Assert.AreEqual(_dbContext.Employees.Count(), 0);
            Assert.AreEqual(employeesViewModel.LastNames.Count(), 0);
            Assert.AreEqual(employeesViewModel.FilterLastNames.Count(), 0);
            Assert.AreEqual(employeesViewModel.FilterItems.Count(), 1);
        }
        [TestMethod()]
        public void ExecuteDeleteManyEmployeesCommandTest()
        {
            var employeesViewModel = _serviceProvider.GetRequiredService<EmployeesViewModel>();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test1";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test1";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test2";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test2";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test3";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test3";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.FilterItems[1].IsSelected = true;
            employeesViewModel.FilterItems[3].IsSelected = true;
            employeesViewModel.ExecuteDeleteManyEmployeeCommand();
            Assert.AreEqual(2, _dbContext.Employees.Count());
            Assert.AreEqual(3, employeesViewModel.FilterItems.Count());
        }

        [TestMethod()]
        public void ExecuteDeleteManyEmployeesCommandTest_NoElements()
        {
            var employeesViewModel = _serviceProvider.GetRequiredService<EmployeesViewModel>();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test2";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test2";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.FilterItems[1].IsSelected = true;
            employeesViewModel.FilterItems[2].IsSelected = true;
            employeesViewModel.ExecuteDeleteManyEmployeeCommand();
            Assert.AreEqual(1, _dbContext.Employees.Count());
            Assert.AreEqual(2, employeesViewModel.FilterItems.Count());
        }

        [TestMethod()]
        public void ExecuteChangeSelectionCommandTest()
        {
            var employeesViewModel = _serviceProvider.GetRequiredService<EmployeesViewModel>();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test2";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test2";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.ExecuteChangeSelectionCommand(true);
            Assert.AreEqual(true, employeesViewModel.FilterItems[0].IsSelected);
        }

        [TestMethod()]
        public void ExecuteChangeLastNameSelectionCommandTest()
        {
            var employeesViewModel = _serviceProvider.GetRequiredService<EmployeesViewModel>();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test2";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test2";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.ExecuteChangeLastNameSelectionCommand(true);
            Assert.AreEqual(false, false);
        }

        [TestMethod()]
        public void ExecuteChangeGenderSelectionCommandTest()
        {
            var employeesViewModel = _serviceProvider.GetRequiredService<EmployeesViewModel>();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test2";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test2";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.ExecuteChangeGenderSelectionCommand(true);
            Assert.AreEqual(false, employeesViewModel.FilterGenders[0].IsSelected);
        }

        [TestMethod()]
        public void ExecuteChangeFirstNameSelectionCommandTest()
        {
            var employeesViewModel = _serviceProvider.GetRequiredService<EmployeesViewModel>();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
            employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            employeesViewModel.ExecuteAddEmployeeCommand();
            employeesViewModel.ExecuteChangeFirstNameSelectionCommand(true);
            Assert.AreEqual(false, employeesViewModel.FilterFirstNames[0].IsSelected);
        }

        [TestMethod()]
        public void ExecuteChangeFieldCommandTest_FirstName()
        {
            var employeesViewModel = _serviceProvider.GetRequiredService<EmployeesViewModel>();
            employeesViewModel.ExecuteChangeFieldCommand("По имени");
            Assert.AreEqual(employeesViewModel.Field, EmployeesViewModel.Fields.firstName);
        }

        [TestMethod()]
        public void ExecuteChangeFieldCommandTest_Default()
        {
            var employeesViewModel = _serviceProvider.GetRequiredService<EmployeesViewModel>();
            employeesViewModel.ExecuteChangeFieldCommand("abrakadabra");
            Assert.AreEqual(employeesViewModel.Field, EmployeesViewModel.Fields.firstName);
        }

        [TestMethod()]
        public void ExecuteSearchGenderCommandTest()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var employeesViewModel = scope.ServiceProvider.GetRequiredService<EmployeesViewModel>();

                // Arranges and Acts
                employeesViewModel.SearchGender = "жен";
                employeesViewModel.ExecuteSearchGenderCommand();
                // Assert
                Assert.AreEqual(1, employeesViewModel.FilterGenders.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchGenderCommandTest_EmptySearchText()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var employeesViewModel = scope.ServiceProvider.GetRequiredService<EmployeesViewModel>();

                // Arranges and Acts
                employeesViewModel.SearchGender = "Поиск по полу";
                employeesViewModel.ExecuteSearchGenderCommand();
                // Assert
                Assert.AreEqual(2, employeesViewModel.FilterGenders.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchGenderCommandTest_NoElements()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var employeesViewModel = scope.ServiceProvider.GetRequiredService<EmployeesViewModel>();

                // Arranges and Acts
                employeesViewModel.SearchGender = "gender";
                employeesViewModel.ExecuteSearchGenderCommand();
                // Assert
                Assert.AreEqual(0, employeesViewModel.FilterGenders.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchJobCommandTest()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var employeesViewModel = scope.ServiceProvider.GetRequiredService<EmployeesViewModel>();

                // Arranges and Acts
                employeesViewModel.SearchJob = "дв";
                employeesViewModel.ExecuteSearchJobCommand();
                // Assert
                Assert.AreEqual(1, employeesViewModel.FilterJobs.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchJobCommandTest_EmptySearchText()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var employeesViewModel = scope.ServiceProvider.GetRequiredService<EmployeesViewModel>();

                // Arranges and Acts
                employeesViewModel.SearchJob = "Поиск по должности";
                employeesViewModel.ExecuteSearchJobCommand();
                // Assert
                Assert.AreEqual(2, employeesViewModel.FilterJobs.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchJobCommandTest_NoElements()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var employeesViewModel = scope.ServiceProvider.GetRequiredService<EmployeesViewModel>();

                // Arranges and Acts
                employeesViewModel.SearchJob = "job";
                employeesViewModel.ExecuteSearchJobCommand();
                // Assert
                Assert.AreEqual(0, employeesViewModel.FilterJobs.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchFirstNameCommandTest()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var employeesViewModel = scope.ServiceProvider.GetRequiredService<EmployeesViewModel>();

                // Arranges and Acts
                employeesViewModel.SearchFirstName = "Te";
                employeesViewModel.ExecuteSearchFirstNameCommand();
                // Assert
                Assert.AreEqual(0, employeesViewModel.FilterFirstNames.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchFirstNameCommandTest_EmptySearchText()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var employeesViewModel = scope.ServiceProvider.GetRequiredService<EmployeesViewModel>();

                // Arranges and Acts
                employeesViewModel.SearchFirstName = "Поиск по имени";
                employeesViewModel.ExecuteSearchFirstNameCommand();
                // Assert
                Assert.AreEqual(0, employeesViewModel.FilterFirstNames.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchFirstNameCommandTest_NoElements()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var employeesViewModel = scope.ServiceProvider.GetRequiredService<EmployeesViewModel>();

                // Arranges and Acts
                employeesViewModel.SearchFirstName = "firstName";
                employeesViewModel.ExecuteSearchFirstNameCommand();
                // Assert
                Assert.AreEqual(0, employeesViewModel.FilterFirstNames.Count);
            }
        }
        [TestMethod()]
        public void ExecuteSearchCommandTest_ByLastName()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var employeesViewModel = scope.ServiceProvider.GetRequiredService<EmployeesViewModel>();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test1";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test12";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test3";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test4";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                // Arranges and Acts
                employeesViewModel.Field = EmployeesViewModel.Fields.lastName;
                employeesViewModel.SearchText = "Test1";
                employeesViewModel.ExecuteSearchCommand();
                // Assert
                Assert.AreEqual(1, employeesViewModel.FilterItems.Count);
            }
        }
        [TestMethod]
        public void ExecuteSearchCommandTest_ByFirstName()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var employeesViewModel = scope.ServiceProvider.GetRequiredService<EmployeesViewModel>();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test1";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test1";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test12";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test12";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test3";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test3";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test4";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test4";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                // Arranges and Acts
                employeesViewModel.Field = EmployeesViewModel.Fields.firstName;
                employeesViewModel.SearchText = "Test1";
                employeesViewModel.ExecuteSearchCommand();
                // Assert
                Assert.AreEqual(1, employeesViewModel.FilterItems.Count);
            }
        }

        [TestMethod]
        public void ExecuteSearchCommandTest_Empty()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var employeesViewModel = scope.ServiceProvider.GetRequiredService<EmployeesViewModel>();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.LastName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.ContractNumber = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Username = "Test";
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Job = _dbContext.Jobs.First(g => g.Id == 1);
                employeesViewModel.FilterItems[employeesViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                employeesViewModel.ExecuteAddEmployeeCommand();
                // Arranges and Acts
                employeesViewModel.Field = EmployeesViewModel.Fields.lastName;
                employeesViewModel.ExecuteSearchLastNameCommand();
                // Assert
                Assert.AreEqual(4, employeesViewModel.FilterItems.Count - 1);
            }
        }
    }
}