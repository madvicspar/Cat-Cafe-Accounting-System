using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cats_Cafe_Accounting_System.ViewModels.Tests
{
    [TestClass()]
    public class AuthorizationViewModelTests
    {
        private ServiceProvider _serviceProvider;
        private static ApplicationDbContext _dbContext;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _dbContext = ApplicationDbContext.CreateInMemoryDatabase();
            _dbContext.Genders.Add(new Gender { Title = "женский" });
            _dbContext.Genders.Add(new Gender { Title = "мужской" });
            _dbContext.Statuses.Add(new Status { Title = "чилится" });
            _dbContext.Statuses.Add(new Status { Title = "не числится" });
            _dbContext.Breeds.Add(new Breed { Title = "сиамская", Id = "SMS" });
            _dbContext.Breeds.Add(new Breed { Title = "мейн-кун", Id = "MNC" });
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
            services.AddSingleton(new PetsViewModel(_dbContext));
            _serviceProvider = services.BuildServiceProvider();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            foreach (var pet in _dbContext.Pets)
            {
                _dbContext.Pets.Remove(pet);
            }
            _dbContext.SaveChanges();
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
                Assert.AreEqual(1, petsViewModel.Items.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchNameCommandTest()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();

                // Arranges and Acts
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test2";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test2";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Name";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Pass";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test3";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test3";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.SearchName = "Tes";
                petsViewModel.ExecuteSearchNameCommand();
                // Assert
                Assert.AreEqual(3, petsViewModel.FilterNames.Count);
            }
        }
        [TestMethod()]
        public void ExecuteSearchNameCommandTest_EmptySearchText()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();

                // Arranges and Acts
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test2";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test2";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.SearchName = "Поиск по имени";
                petsViewModel.ExecuteSearchNameCommand();
                // Assert
                Assert.AreEqual("Поиск по имени", petsViewModel.SearchName);
            }
        }

        //[TestMethod()]
        //public void UpdatePetTest()
        //{
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();

        //        // Arranges and Acts
        //        petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test";
        //        petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test";
        //        petsViewModel.ExecuteAddPetCommand();
        //        petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 2].Item.Name = "TestUpdated";
        //        // Assert
        //        Assert.AreEqual("TestUpdated", _dbContext.Pets.Last().Name);
        //        Assert.AreEqual("TestUpdated", petsViewModel.Names.Last().Item.Name);
        //        Assert.AreEqual("TestUpdated", petsViewModel.FilterNames.Last().Item.Name);
        //    }
        //}

        //[TestMethod()]
        //public void ExecuteUpdatePetCommandTest()
        //{
        //    var petsViewModel = _serviceProvider.GetRequiredService<PetsViewModel>();
        //    petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test";
        //    petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test";
        //    petsViewModel.ExecuteAddPetCommand();
        //    PetModel pet = (PetModel)petsViewModel.FilterItems[0].Item.Clone();
        //    pet.Name = "TestUpdated";
        //    petsViewModel.ExecuteUpdatePetCommand(pet);
        //    Assert.AreEqual("TestUpdated", _dbContext.Pets.Last().Name);
        //    Assert.AreEqual("TestUpdated", petsViewModel.Names.Last().Item.Name);
        //    Assert.AreEqual("TestUpdated", petsViewModel.FilterNames.Last().Item.Name);
        //}

        [TestMethod()]
        public void ExecuteDeletePetCommandTest()
        {
            var petsViewModel = _serviceProvider.GetRequiredService<PetsViewModel>();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.ExecuteDeletePetCommand(_dbContext.Pets.First());
            Assert.AreEqual(_dbContext.Pets.Count(), 0);
            Assert.AreEqual(petsViewModel.Names.Count(), 0);
            Assert.AreEqual(petsViewModel.FilterNames.Count(), 0);
        }


    }
}