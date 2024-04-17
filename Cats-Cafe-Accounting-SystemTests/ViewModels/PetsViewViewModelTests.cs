using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cats_Cafe_Accounting_System.ViewModels.Tests
{
    [TestClass()]
    public class PetsViewViewModelTests
    {
        private ServiceProvider _serviceProvider;
        private static ApplicationDbContext _dbContext;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _dbContext = ApplicationDbContext.CreateInMemoryDatabase();

            _dbContext.Genders.Add(new Gender { Title = "женский" });
            _dbContext.Genders.Add(new Gender { Title = "мужской" });

            _dbContext.Statuses.Add(new Status { Title = "числится" });
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
                Assert.AreEqual(_dbContext.Pets.Count(), 1);
                Assert.AreEqual(petsViewModel.Names.Count(), 1);
                Assert.AreEqual(petsViewModel.FilterNames.Count(), 1);
                Assert.AreEqual(petsViewModel.FilterItems.Count(), 2);
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

        [TestMethod()]
        public void ExecuteUpdatePetCommandTest()
        {
            var petsViewModel = _serviceProvider.GetRequiredService<PetsViewModel>();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 2].Item.Name = "TestUpdated";
            petsViewModel.ExecuteUpdatePetCommand(petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 2].Item);
            Assert.AreEqual("TestUpdated", _dbContext.Pets.Last().Name);
            Assert.AreEqual("TestUpdated", petsViewModel.Names.Last().Item.Name);
            Assert.AreEqual("TestUpdated", petsViewModel.FilterNames.Last().Item.Name);
        }

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
            Assert.AreEqual(petsViewModel.FilterItems.Count(), 1);
        }
        [TestMethod()]
        public void ExecuteDeletePetCommandTest_NoElements()
        {
            var petsViewModel = _serviceProvider.GetRequiredService<PetsViewModel>();
            petsViewModel.ExecuteDeletePetCommand(petsViewModel.FilterItems.Last().Item);
            Assert.AreEqual(_dbContext.Pets.Count(), 0);
            Assert.AreEqual(petsViewModel.Names.Count(), 0);
            Assert.AreEqual(petsViewModel.FilterNames.Count(), 0);
            Assert.AreEqual(petsViewModel.FilterItems.Count(), 1);
        }
        [TestMethod()]
        public void ExecuteDeleteManyPetsCommandTest()
        {
            var petsViewModel = _serviceProvider.GetRequiredService<PetsViewModel>();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test1";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test1";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test2";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test2";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test3";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test3";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test4";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test4";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.FilterItems[1].IsSelected = true;
            petsViewModel.FilterItems[3].IsSelected = true;
            petsViewModel.ExecuteDeleteManyPetCommand();
            Assert.AreEqual(2, _dbContext.Pets.Count());
            Assert.AreEqual(2, petsViewModel.Names.Count());
            Assert.AreEqual(2, petsViewModel.FilterNames.Count());
            Assert.AreEqual(3, petsViewModel.FilterItems.Count());
        }

        [TestMethod()]
        public void ExecuteDeleteManyPetsCommandTest_NoElements()
        {
            var petsViewModel = _serviceProvider.GetRequiredService<PetsViewModel>();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test1";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test1";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test2";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test2";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.FilterItems[1].IsSelected = true;
            petsViewModel.FilterItems[2].IsSelected = true;
            petsViewModel.ExecuteDeleteManyPetCommand();
            Assert.AreEqual(1, _dbContext.Pets.Count());
            Assert.AreEqual(1, petsViewModel.Names.Count());
            Assert.AreEqual(1, petsViewModel.FilterNames.Count());
            Assert.AreEqual(2, petsViewModel.FilterItems.Count());
        }

        [TestMethod()]
        public void ExecuteChangeSelectionCommandTest()
        {
            var petsViewModel = _serviceProvider.GetRequiredService<PetsViewModel>();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test1";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test1";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test2";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test2";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.ExecuteChangeSelectionCommand(true);
            Assert.AreEqual(true, petsViewModel.FilterItems[0].IsSelected);
        }

        [TestMethod()]
        public void ExecuteChangeNameSelectionCommandTest()
        {
            var petsViewModel = _serviceProvider.GetRequiredService<PetsViewModel>();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test1";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test1";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test2";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test2";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.ExecuteChangeNameSelectionCommand(true);
            Assert.AreEqual(false, petsViewModel.FilterNames[0].IsSelected);
        }

        [TestMethod()]
        public void ExecuteChangeGenderSelectionCommandTest()
        {
            var petsViewModel = _serviceProvider.GetRequiredService<PetsViewModel>();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test1";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test1";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test2";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test2";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.ExecuteChangeGenderSelectionCommand(true);
            Assert.AreEqual(false, petsViewModel.FilterGenders[0].IsSelected);
        }

        [TestMethod()]
        public void ExecuteChangeStatusSelectionCommandTest()
        {
            var petsViewModel = _serviceProvider.GetRequiredService<PetsViewModel>();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test1";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test1";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test2";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test2";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.ExecuteChangeStatusSelectionCommand(true);
            Assert.AreEqual(false, petsViewModel.FilterStatuses[0].IsSelected);
        }

        [TestMethod()]
        public void ExecuteChangeBreedSelectionCommandTest()
        {
            var petsViewModel = _serviceProvider.GetRequiredService<PetsViewModel>();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test1";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test1";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test2";
            petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test2";
            petsViewModel.ExecuteAddPetCommand();
            petsViewModel.ExecuteChangeBreedSelectionCommand(true);
            Assert.AreEqual(false, petsViewModel.FilterBreeds[0].IsSelected);
        }

        [TestMethod()]
        public void ExecuteChangeFieldCommandTest_Name()
        {
            var petsViewModel = _serviceProvider.GetRequiredService<PetsViewModel>();
            petsViewModel.ExecuteChangeFieldCommand("По имени");
            Assert.AreEqual(petsViewModel.Field, PetsViewModel.Fields.name);
        }

        [TestMethod()]
        public void ExecuteChangeFieldCommandTest_Breed()
        {
            var petsViewModel = _serviceProvider.GetRequiredService<PetsViewModel>();
            petsViewModel.ExecuteChangeFieldCommand("По породе");
            Assert.AreEqual(petsViewModel.Field, PetsViewModel.Fields.breed);
        }

        [TestMethod()]
        public void ExecuteChangeFieldCommandTest_Default()
        {
            var petsViewModel = _serviceProvider.GetRequiredService<PetsViewModel>();
            petsViewModel.ExecuteChangeFieldCommand("abrakadabra");
            Assert.AreEqual(petsViewModel.Field, PetsViewModel.Fields.name);
        }

        [TestMethod()]
        public void ExecuteSearchGenderCommandTest()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();

                // Arranges and Acts
                petsViewModel.SearchGender = "жен";
                petsViewModel.ExecuteSearchGenderCommand();
                // Assert
                Assert.AreEqual(1, petsViewModel.FilterGenders.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchGenderCommandTest_EmptySearchText()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();

                // Arranges and Acts
                petsViewModel.SearchGender = "Поиск по полу";
                petsViewModel.ExecuteSearchGenderCommand();
                // Assert
                Assert.AreEqual(2, petsViewModel.FilterGenders.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchGenderCommandTest_NoElements()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();

                // Arranges and Acts
                petsViewModel.SearchGender = "gender";
                petsViewModel.ExecuteSearchGenderCommand();
                // Assert
                Assert.AreEqual(0, petsViewModel.FilterGenders.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchStatusCommandTest()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();

                // Arranges and Acts
                petsViewModel.SearchStatus = "не";
                petsViewModel.ExecuteSearchStatusCommand();
                // Assert
                Assert.AreEqual(1, petsViewModel.FilterStatuses.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchStatusCommandTest_EmptySearchText()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();

                // Arranges and Acts
                petsViewModel.SearchStatus = "Поиск по статусу";
                petsViewModel.ExecuteSearchStatusCommand();
                // Assert
                Assert.AreEqual(2, petsViewModel.FilterStatuses.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchStatusCommandTest_NoElements()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();

                // Arranges and Acts
                petsViewModel.SearchStatus = "status";
                petsViewModel.ExecuteSearchStatusCommand();
                // Assert
                Assert.AreEqual(0, petsViewModel.FilterStatuses.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchBreedCommandTest()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();

                // Arranges and Acts
                petsViewModel.SearchBreed = "мейн";
                petsViewModel.ExecuteSearchBreedCommand();
                // Assert
                Assert.AreEqual(1, petsViewModel.FilterBreeds.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchBreedCommandTest_EmptySearchText()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();

                // Arranges and Acts
                petsViewModel.SearchBreed = "Поиск по породе";
                petsViewModel.ExecuteSearchBreedCommand();
                // Assert
                Assert.AreEqual(2, petsViewModel.FilterBreeds.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchBreedCommandTest_NoElements()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();

                // Arranges and Acts
                petsViewModel.SearchBreed = "breed";
                petsViewModel.ExecuteSearchBreedCommand();
                // Assert
                Assert.AreEqual(0, petsViewModel.FilterBreeds.Count);
            }
        }
        [TestMethod()]
        public void ExecuteSearchCommandTest_ByName()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test1";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test1";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test12";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test2";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test3";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test3";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test4";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test4";
                petsViewModel.ExecuteAddPetCommand();
                // Arranges and Acts
                petsViewModel.Field = PetsViewModel.Fields.name;
                petsViewModel.SearchText = "Test1";
                petsViewModel.ExecuteSearchCommand();
                // Assert
                Assert.AreEqual(2, petsViewModel.FilterItems.Count - 1);
            }
        }
        [TestMethod]
        public void ExecuteSearchCommandTest_ByBreed()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test1";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test1";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Breed = _dbContext.Breeds.Last();
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test12";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test2";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Breed = _dbContext.Breeds.Last();
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test3";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test3";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test4";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test4";
                petsViewModel.ExecuteAddPetCommand();
                // Arranges and Acts
                petsViewModel.Field = PetsViewModel.Fields.breed;
                petsViewModel.SearchText = _dbContext.Breeds.Last().Title.Substring(5);
                petsViewModel.ExecuteSearchCommand();
                // Assert
                Assert.AreEqual(2, petsViewModel.FilterItems.Count - 1);
            }
        }

        [TestMethod]
        public void ExecuteSearchCommandTest_Empty()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test1";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test1";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Breed = _dbContext.Breeds.Last();
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test12";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test2";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Breed = _dbContext.Breeds.Last();
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test3";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test3";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test4";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test4";
                petsViewModel.ExecuteAddPetCommand();
                // Arranges and Acts
                petsViewModel.Field = PetsViewModel.Fields.name;
                petsViewModel.SearchText = "Поиск по имени";
                petsViewModel.ExecuteSearchCommand();
                // Assert
                Assert.AreEqual(4, petsViewModel.FilterItems.Count - 1);
            }
        }
    }
}