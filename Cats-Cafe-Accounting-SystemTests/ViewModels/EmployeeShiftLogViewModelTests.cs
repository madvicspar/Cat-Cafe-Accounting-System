using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cats_Cafe_Accounting_System.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cats_Cafe_Accounting_System.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Cats_Cafe_Accounting_System.Models;
using NPOI.SS.Formula.Functions;
using Cats_Cafe_Accounting_System.RegularClasses;

namespace Cats_Cafe_Accounting_System.ViewModels.Tests
{
    [TestClass()]
    public class EmployeeShiftLogViewModelTests
    {
        private ServiceProvider _serviceProvider;
        private static ApplicationDbContext _dbContext;

        //[ClassInitialize]
        //public static void ClassInitialize(TestContext context)
        //{
        //    _dbContext = ApplicationDbContext.CreateInMemoryDatabase();

        //    _dbContext.Jobs.Add(new JobModel { Title = "дворник", Rate = 100f });
        //    _dbContext.Jobs.Add(new JobModel { Title = "продавец", Rate = 200f });

        //    _dbContext.Genders.Add(new Gender { Title = "женский" });
        //    _dbContext.Genders.Add(new Gender { Title = "мужской" });

        //    _dbContext.SaveChanges();
        //}

        //[ClassCleanup]
        //public static void ClassCleanup()
        //{
        //    _dbContext.Dispose();
        //}

        //[TestInitialize]
        //public void TestInitialize()
        //{
        //    var services = new ServiceCollection();
        //    services.AddSingleton(_dbContext);
        //    services.AddSingleton(new EmployeeShiftLogViewModel(_dbContext));
        //    _serviceProvider = services.BuildServiceProvider();
        //}

        //[TestCleanup]
        //public void TestCleanup()
        //{
        //    foreach (var entry in _dbContext.EmployeeShiftLogEntries)
        //    {
        //        _dbContext.EmployeeShiftLogEntries.Remove(entry);
        //    }
        //    _dbContext.SaveChanges();
        //}

        public EmployeeModel GenerateEmployee()
        {
            var salt = HashingHelper.GenerateSalt();
            var visitorToAdd = new EmployeeModel
            {
                FirstName = "Test",
                LastName = "Test",
                Pathronymic = "Test",
                GenderId = 1,
                Gender = _dbContext.Genders.First(g => g.Id == 1),
                PhoneNumber = "Test",
                Birthday = DateTime.Today,
                ContractNumber = "Test",
                JobId = 1,
                Job = _dbContext.Jobs.First(g => g.Id == 1),
                Username = "Test",
                Salt = salt,
                Password = Encoding.UTF8.GetBytes("Test")
            };
            _dbContext.Employees.Add(visitorToAdd.Clone() as EmployeeModel);
            _dbContext.SaveChanges();
            visitorToAdd.Id = _dbContext.Employees.First(p => p.LastName + p.FirstName + p.Pathronymic == visitorToAdd.LastName + visitorToAdd.FirstName + visitorToAdd.Pathronymic).Id;
            return visitorToAdd;
        }

        //[TestMethod()]
        //public void ExecuteAddEntryCommand_ShouldAddNewEntry()
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var employeeShiftLogEntriesViewModel = scope.ServiceProvider.GetRequiredService<EmployeeShiftLogViewModel>(); // Используем Dependency Injection

        //        // Arrange
        //        employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test";
        //        employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Employee = GenerateEmployee();

        //        // Act
        //        employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();

        //        // Assert
        //        Assert.AreEqual(1, employeeShiftLogEntriesViewModel.Items.Count);
        //        Assert.AreEqual(_dbContext.EmployeeShiftLogEntries.Count(), 1);
        //        Assert.AreEqual(employeeShiftLogEntriesViewModel.Numbers.Count(), 1);
        //        Assert.AreEqual(employeeShiftLogEntriesViewModel.FilterNumbers.Count(), 1);
        //        Assert.AreEqual(employeeShiftLogEntriesViewModel.FilterItems.Count(), 2);
        //    }
        //}

        //[TestMethod()]
        //public void ExecuteSearchNumberCommandTest()
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var employeeShiftLogEntriesViewModel = scope.ServiceProvider.GetRequiredService<EmployeeShiftLogViewModel>();

        //        // Arranges and Acts
        //        employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test";
        //        employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Employee.ContractNumber = "Test";
        //        employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //        employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test2";
        //        employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //        employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Number";
        //        employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //        employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test3";
        //        employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //        employeeShiftLogEntriesViewModel.SearchNumber = "Tes";
        //        employeeShiftLogEntriesViewModel.ExecuteSearchNumberCommand();
        //        // Assert
        //        Assert.AreEqual(3, employeeShiftLogEntriesViewModel.FilterNumbers.Count);
        //    }
        //}

        //[TestMethod()]
        //public void ExecuteSearchNumberCommandTest_EmptySearchText()
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var employeeShiftLogEntriesViewModel = scope.ServiceProvider.GetRequiredService<EmployeeShiftLogViewModel>();

        //        // Arranges and Acts
        //        employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test";
        //        employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //        employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test2";
        //        employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //        employeeShiftLogEntriesViewModel.SearchNumber = "Поиск по должности";
        //        employeeShiftLogEntriesViewModel.ExecuteSearchNumberCommand();
        //        // Assert
        //        Assert.AreEqual("Поиск по должности", employeeShiftLogEntriesViewModel.SearchNumber);
        //    }
        //}

        //[TestMethod()]
        //public void ExecuteUpdateEntryCommandTest()
        //{
        //    var employeeShiftLogEntriesViewModel = _serviceProvider.GetRequiredService<EmployeeShiftLogViewModel>();
        //    employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test";
        //    employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //    employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 2].Item.Comments = "TestUpdated";
        //    employeeShiftLogEntriesViewModel.ExecuteUpdateEntryCommand(employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 2].Item);
        //    Assert.AreEqual("TestUpdated", _dbContext.EmployeeShiftLogEntries.Last().Comments);
        //    Assert.AreEqual("TestUpdated", employeeShiftLogEntriesViewModel.Numbers.Last().Item.Comments);
        //    Assert.AreEqual("TestUpdated", employeeShiftLogEntriesViewModel.FilterNumbers.Last().Item.Comments);
        //}

        //[TestMethod()]
        //public void ExecuteDeleteEntryCommandTest()
        //{
        //    var employeeShiftLogEntriesViewModel = _serviceProvider.GetRequiredService<EmployeeShiftLogViewModel>();
        //    employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test";
        //    employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //    employeeShiftLogEntriesViewModel.ExecuteDeleteEntryCommand(_dbContext.EmployeeShiftLogEntries.First());
        //    Assert.AreEqual(_dbContext.EmployeeShiftLogEntries.Count(), 0);
        //    Assert.AreEqual(employeeShiftLogEntriesViewModel.Numbers.Count(), 0);
        //    Assert.AreEqual(employeeShiftLogEntriesViewModel.FilterNumbers.Count(), 0);
        //    Assert.AreEqual(employeeShiftLogEntriesViewModel.FilterItems.Count(), 1);
        //}
        //[TestMethod()]
        //public void ExecuteDeleteEntryCommandTest_NoElements()
        //{
        //    var employeeShiftLogEntriesViewModel = _serviceProvider.GetRequiredService<EmployeeShiftLogViewModel>();
        //    employeeShiftLogEntriesViewModel.ExecuteDeleteEntryCommand(employeeShiftLogEntriesViewModel.FilterItems.Last().Item);
        //    Assert.AreEqual(_dbContext.EmployeeShiftLogEntries.Count(), 0);
        //    Assert.AreEqual(employeeShiftLogEntriesViewModel.Numbers.Count(), 0);
        //    Assert.AreEqual(employeeShiftLogEntriesViewModel.FilterNumbers.Count(), 0);
        //    Assert.AreEqual(employeeShiftLogEntriesViewModel.FilterItems.Count(), 1);
        //}
        //[TestMethod()]
        //public void ExecuteDeleteManyEmployeeShiftLogCommandTest()
        //{
        //    var employeeShiftLogEntriesViewModel = _serviceProvider.GetRequiredService<EmployeeShiftLogViewModel>();
        //    employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test1";
        //    employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //    employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test2";
        //    employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //    employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test3";
        //    employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //    employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test4";
        //    employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //    employeeShiftLogEntriesViewModel.FilterItems[1].IsSelected = true;
        //    employeeShiftLogEntriesViewModel.FilterItems[3].IsSelected = true;
        //    employeeShiftLogEntriesViewModel.ExecuteDeleteManyEntryCommand();
        //    Assert.AreEqual(2, _dbContext.EmployeeShiftLogEntries.Count());
        //    Assert.AreEqual(2, employeeShiftLogEntriesViewModel.Numbers.Count());
        //    Assert.AreEqual(2, employeeShiftLogEntriesViewModel.FilterNumbers.Count());
        //    Assert.AreEqual(3, employeeShiftLogEntriesViewModel.FilterItems.Count());
        //}

        //[TestMethod()]
        //public void ExecuteDeleteManyEmployeeShiftLogCommandTest_NoElements()
        //{
        //    var employeeShiftLogEntriesViewModel = _serviceProvider.GetRequiredService<EmployeeShiftLogViewModel>();
        //    employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test1";
        //    employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //    employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test2";
        //    employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //    employeeShiftLogEntriesViewModel.FilterItems[1].IsSelected = true;
        //    employeeShiftLogEntriesViewModel.FilterItems[2].IsSelected = true;
        //    employeeShiftLogEntriesViewModel.ExecuteDeleteManyEntryCommand();
        //    Assert.AreEqual(1, _dbContext.EmployeeShiftLogEntries.Count());
        //    Assert.AreEqual(1, employeeShiftLogEntriesViewModel.Numbers.Count());
        //    Assert.AreEqual(1, employeeShiftLogEntriesViewModel.FilterNumbers.Count());
        //    Assert.AreEqual(2, employeeShiftLogEntriesViewModel.FilterItems.Count());
        //}

        //[TestMethod()]
        //public void ExecuteChangeSelectionCommandTest()
        //{
        //    var employeeShiftLogEntriesViewModel = _serviceProvider.GetRequiredService<EmployeeShiftLogViewModel>();
        //    employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test1";
        //    employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //    employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test2";
        //    employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //    employeeShiftLogEntriesViewModel.ExecuteChangeSelectionCommand(true);
        //    Assert.AreEqual(true, employeeShiftLogEntriesViewModel.FilterItems[0].IsSelected);
        //}

        //[TestMethod()]
        //public void ExecuteChangeNumberSelectionCommandTest()
        //{
        //    var employeeShiftLogEntriesViewModel = _serviceProvider.GetRequiredService<EmployeeShiftLogViewModel>();
        //    employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test1";
        //    employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //    employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test2";
        //    employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //    employeeShiftLogEntriesViewModel.ExecuteChangeNumberSelectionCommand(true);
        //    Assert.AreEqual(false, employeeShiftLogEntriesViewModel.FilterNumbers[0].IsSelected);
        //}

        //[TestMethod()]
        //public void ExecuteSearchCommandTest_ByNumber()
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var employeeShiftLogEntriesViewModel = scope.ServiceProvider.GetRequiredService<EmployeeShiftLogViewModel>();
        //        employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test1";
        //        employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //        employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test12";
        //        employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //        employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test3";
        //        employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //        employeeShiftLogEntriesViewModel.FilterItems[employeeShiftLogEntriesViewModel.FilterItems.Count - 1].Item.Comments = "Test4";
        //        employeeShiftLogEntriesViewModel.ExecuteAddEntryCommand();
        //        // Arranges and Acts
        //        employeeShiftLogEntriesViewModel.SearchNumber = "Test1";
        //        employeeShiftLogEntriesViewModel.ExecuteSearchCommand();
        //        // Assert
        //        Assert.AreEqual(2, employeeShiftLogEntriesViewModel.FilterItems.Count - 1);
        //    }
        //}
    }
}