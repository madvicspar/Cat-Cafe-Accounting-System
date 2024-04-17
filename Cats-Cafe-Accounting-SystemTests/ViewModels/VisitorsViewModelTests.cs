using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cats_Cafe_Accounting_System.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Cats_Cafe_Accounting_System.ViewModels.Tests
{
    [TestClass()]
    public class VisitorsViewModelTests
    {
        private ServiceProvider _serviceProvider;
        private static ApplicationDbContext _dbContext;

        [ExcludeFromCodeCoverage]
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _dbContext = ApplicationDbContext.CreateInMemoryDatabase();

            if (_dbContext.Genders.Count() != 2)
            {
                _dbContext.Genders.Add(new Gender { Title = "женский" });
                _dbContext.Genders.Add(new Gender { Title = "мужской" });
            }

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
            services.AddSingleton(new VisitorsViewModel(_dbContext));
            _serviceProvider = services.BuildServiceProvider();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            foreach (var visitor in _dbContext.Visitors)
            {
                _dbContext.Visitors.Remove(visitor);
            }
            _dbContext.SaveChanges();
        }

        [TestMethod()]
        public void ExecuteAddVisitorCommand_ShouldAddNewVisitor()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var visitorsViewModel = scope.ServiceProvider.GetRequiredService<VisitorsViewModel>(); // Используем Dependency Injection

                // Arrange
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;

                // Act
                visitorsViewModel.ExecuteAddVisitorCommand();

                // Assert
                Assert.AreEqual(1, visitorsViewModel.Items.Count);
                Assert.AreEqual(_dbContext.Visitors.Count(), 1);
                Assert.AreEqual(visitorsViewModel.FilterLastNames.Count(), 0);
                Assert.AreEqual(visitorsViewModel.FilterItems.Count(), 2);
            }
        }

        [TestMethod()]
        public void ExecuteSearchLastNameCommandTest()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var visitorsViewModel = scope.ServiceProvider.GetRequiredService<VisitorsViewModel>();

                // Arranges and Acts
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test1";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test2";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test3";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                visitorsViewModel.SearchLastName = "Tes";
                visitorsViewModel.ExecuteSearchLastNameCommand();
                // Assert
                Assert.AreEqual(3, visitorsViewModel.FilterItems.Count - 2);
            }
        }

        [TestMethod()]
        public void ExecuteSearchLastNameCommandTest_EmptySearchText()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var visitorsViewModel = scope.ServiceProvider.GetRequiredService<VisitorsViewModel>();

                // Arranges and Acts
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test1";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                visitorsViewModel.SearchLastName = "Поиск по имени";
                visitorsViewModel.ExecuteSearchLastNameCommand();
                // Assert
                Assert.AreEqual("Поиск по имени", visitorsViewModel.SearchLastName);
            }
        }

        [TestMethod()]
        public void ExecuteUpdateVisitorCommandTest()
        {
            var visitorsViewModel = _serviceProvider.GetRequiredService<VisitorsViewModel>();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 2].Item.LastName = "TestUpdated";
            visitorsViewModel.ExecuteUpdateVisitorCommand(visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 2].Item);
            Assert.AreEqual("TestUpdated", _dbContext.Visitors.Last().LastName);
        }

        [TestMethod()]
        public void ExecuteDeleteVisitorCommandTest()
        {
            var visitorsViewModel = _serviceProvider.GetRequiredService<VisitorsViewModel>();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.ExecuteDeleteVisitorCommand(_dbContext.Visitors.First());
            Assert.AreEqual(_dbContext.Visitors.Count(), 0);
            Assert.AreEqual(visitorsViewModel.LastNames.Count(), 0);
            Assert.AreEqual(visitorsViewModel.FilterLastNames.Count(), 0);
            Assert.AreEqual(visitorsViewModel.FilterItems.Count(), 1);
        }
        [TestMethod()]
        public void ExecuteDeleteVisitorCommandTest_NoElements()
        {
            var visitorsViewModel = _serviceProvider.GetRequiredService<VisitorsViewModel>();
            visitorsViewModel.ExecuteDeleteVisitorCommand(visitorsViewModel.FilterItems.Last().Item);
            Assert.AreEqual(_dbContext.Visitors.Count(), 0);
            Assert.AreEqual(visitorsViewModel.LastNames.Count(), 0);
            Assert.AreEqual(visitorsViewModel.FilterLastNames.Count(), 0);
            Assert.AreEqual(visitorsViewModel.FilterItems.Count(), 1);
        }
        [TestMethod()]
        public void ExecuteDeleteManyVisitorsCommandTest()
        {
            var visitorsViewModel = _serviceProvider.GetRequiredService<VisitorsViewModel>();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test1";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test1";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test2";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test2";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test3";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test3";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.FilterItems[1].IsSelected = true;
            visitorsViewModel.FilterItems[3].IsSelected = true;
            visitorsViewModel.ExecuteDeleteManyVisitorCommand();
            Assert.AreEqual(2, _dbContext.Visitors.Count());
            Assert.AreEqual(3, visitorsViewModel.FilterItems.Count());
        }

        [TestMethod()]
        public void ExecuteDeleteManyVisitorsCommandTest_NoElements()
        {
            var visitorsViewModel = _serviceProvider.GetRequiredService<VisitorsViewModel>();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test2";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test2";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.FilterItems[1].IsSelected = true;
            visitorsViewModel.FilterItems[2].IsSelected = true;
            visitorsViewModel.ExecuteDeleteManyVisitorCommand();
            Assert.AreEqual(1, _dbContext.Visitors.Count());
            Assert.AreEqual(2, visitorsViewModel.FilterItems.Count());
        }

        [TestMethod()]
        public void ExecuteChangeSelectionCommandTest()
        {
            var visitorsViewModel = _serviceProvider.GetRequiredService<VisitorsViewModel>();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test2";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test2";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.ExecuteChangeSelectionCommand(true);
            Assert.AreEqual(true, visitorsViewModel.FilterItems[0].IsSelected);
        }

        [TestMethod()]
        public void ExecuteChangeLastNameSelectionCommandTest()
        {
            var visitorsViewModel = _serviceProvider.GetRequiredService<VisitorsViewModel>();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test2";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test2";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.ExecuteChangeLastNameSelectionCommand(true);
            Assert.AreEqual(false, false);
        }

        [TestMethod()]
        public void ExecuteChangeGenderSelectionCommandTest()
        {
            var visitorsViewModel = _serviceProvider.GetRequiredService<VisitorsViewModel>();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test2";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test2";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.ExecuteChangeGenderSelectionCommand(true);
            Assert.AreEqual(false, visitorsViewModel.FilterGenders[0].IsSelected);
        }

        [TestMethod()]
        public void ExecuteChangeFirstNameSelectionCommandTest()
        {
            var visitorsViewModel = _serviceProvider.GetRequiredService<VisitorsViewModel>();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
            visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
            visitorsViewModel.ExecuteAddVisitorCommand();
            visitorsViewModel.ExecuteChangeFirstNameSelectionCommand(true);
            Assert.AreEqual(false, visitorsViewModel.FilterFirstNames[0].IsSelected);
        }

        [TestMethod()]
        public void ExecuteChangeFieldCommandTest_FirstName()
        {
            var visitorsViewModel = _serviceProvider.GetRequiredService<VisitorsViewModel>();
            visitorsViewModel.ExecuteChangeFieldCommand("По имени");
            Assert.AreEqual(visitorsViewModel.Field, VisitorsViewModel.Fields.firstName);
        }

        [TestMethod()]
        public void ExecuteChangeFieldCommandTest_Default()
        {
            var visitorsViewModel = _serviceProvider.GetRequiredService<VisitorsViewModel>();
            visitorsViewModel.ExecuteChangeFieldCommand("abrakadabra");
            Assert.AreEqual(visitorsViewModel.Field, VisitorsViewModel.Fields.firstName);
        }

        [TestMethod()]
        public void ExecuteSearchGenderCommandTest()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var visitorsViewModel = scope.ServiceProvider.GetRequiredService<VisitorsViewModel>();

                // Arranges and Acts
                visitorsViewModel.SearchGender = "жен";
                visitorsViewModel.ExecuteSearchGenderCommand();
                // Assert
                Assert.AreEqual(1, visitorsViewModel.FilterGenders.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchGenderCommandTest_EmptySearchText()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var visitorsViewModel = scope.ServiceProvider.GetRequiredService<VisitorsViewModel>();

                // Arranges and Acts
                visitorsViewModel.SearchGender = "Поиск по полу";
                visitorsViewModel.ExecuteSearchGenderCommand();
                // Assert
                Assert.AreEqual(2, visitorsViewModel.FilterGenders.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchGenderCommandTest_NoElements()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var visitorsViewModel = scope.ServiceProvider.GetRequiredService<VisitorsViewModel>();

                // Arranges and Acts
                visitorsViewModel.SearchGender = "gender";
                visitorsViewModel.ExecuteSearchGenderCommand();
                // Assert
                Assert.AreEqual(0, visitorsViewModel.FilterGenders.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchFirstNameCommandTest()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var visitorsViewModel = scope.ServiceProvider.GetRequiredService<VisitorsViewModel>();

                // Arranges and Acts
                visitorsViewModel.SearchFirstName = "Te";
                visitorsViewModel.ExecuteSearchFirstNameCommand();
                // Assert
                Assert.AreEqual(0, visitorsViewModel.FilterFirstNames.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchFirstNameCommandTest_EmptySearchText()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var visitorsViewModel = scope.ServiceProvider.GetRequiredService<VisitorsViewModel>();

                // Arranges and Acts
                visitorsViewModel.SearchFirstName = "Поиск по имени";
                visitorsViewModel.ExecuteSearchFirstNameCommand();
                // Assert
                Assert.AreEqual(0, visitorsViewModel.FilterFirstNames.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchFirstNameCommandTest_NoElements()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var visitorsViewModel = scope.ServiceProvider.GetRequiredService<VisitorsViewModel>();

                // Arranges and Acts
                visitorsViewModel.SearchFirstName = "firstName";
                visitorsViewModel.ExecuteSearchFirstNameCommand();
                // Assert
                Assert.AreEqual(0, visitorsViewModel.FilterFirstNames.Count);
            }
        }
        [TestMethod()]
        public void ExecuteSearchCommandTest_ByLastName()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var visitorsViewModel = scope.ServiceProvider.GetRequiredService<VisitorsViewModel>();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test1";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test12";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test3";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test4";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                // Arranges and Acts
                visitorsViewModel.Field = VisitorsViewModel.Fields.lastName;
                visitorsViewModel.SearchText = "Test1";
                visitorsViewModel.ExecuteSearchCommand();
                // Assert
                Assert.AreEqual(1, visitorsViewModel.FilterItems.Count);
            }
        }
        [TestMethod]
        public void ExecuteSearchCommandTest_ByFirstName()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var visitorsViewModel = scope.ServiceProvider.GetRequiredService<VisitorsViewModel>();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test1";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test1";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test12";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test12";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test3";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test3";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test4";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test4";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                // Arranges and Acts
                visitorsViewModel.Field = VisitorsViewModel.Fields.firstName;
                visitorsViewModel.SearchText = "Test1";
                visitorsViewModel.ExecuteSearchCommand();
                // Assert
                Assert.AreEqual(1, visitorsViewModel.FilterItems.Count);
            }
        }

        [TestMethod]
        public void ExecuteSearchCommandTest_Empty()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var visitorsViewModel = scope.ServiceProvider.GetRequiredService<VisitorsViewModel>();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.LastName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.FirstName = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Pathronymic = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.PhoneNumber = "Test";
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Gender = _dbContext.Genders.First(g => g.Id == 1);
                visitorsViewModel.FilterItems[visitorsViewModel.FilterItems.Count - 1].Item.Birthday = DateTime.Today;
                visitorsViewModel.ExecuteAddVisitorCommand();
                // Arranges and Acts
                visitorsViewModel.Field = VisitorsViewModel.Fields.lastName;
                visitorsViewModel.ExecuteSearchLastNameCommand();
                // Assert
                Assert.AreEqual(4, visitorsViewModel.FilterItems.Count - 1);
            }
        }
    }
}