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
using Cats_Cafe_Accounting_System.RegularClasses;

namespace Cats_Cafe_Accounting_System.ViewModels.Tests
{
    [TestClass()]
    public class TicketsViewModelTests
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

            _dbContext.Breeds.Add(new Breed { Title = "сиамская", Id = "SIMS" });
            _dbContext.Breeds.Add(new Breed { Title = "мейн-кун", Id = "MEIN" });

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
            services.AddSingleton(new TicketsViewModel(_dbContext));
            _serviceProvider = services.BuildServiceProvider();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            foreach (var ticket in _dbContext.Tickets)
            {
                _dbContext.Tickets.Remove(ticket);
            }
            _dbContext.SaveChanges();
        }

        public PetModel GeneratePet()
        {
            var visitorToAdd = new PetModel
            {
                Name = "Test",
                GenderId = 1,
                Gender = _dbContext.Genders.First(g => g.Id == 1),
                BreedId = "MNC",
                Breed = _dbContext.Breeds.First(g => g.Id == "MNC"),
                StatusId = 1,
                Status = _dbContext.Statuses.First(g => g.Id == 1),
                PassNumber = "Test",
                Birthday = DateTime.Today
            };
            _dbContext.Pets.Add(visitorToAdd.Clone() as PetModel);
            _dbContext.SaveChanges();
            visitorToAdd.Id = _dbContext.Pets.First(p => p.Name == visitorToAdd.Name).Id;
            return visitorToAdd;
        }

        [TestMethod()]
        public void ExecuteAddTicketCommand_ShouldAddNewTicket()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var ticketsViewModel = scope.ServiceProvider.GetRequiredService<TicketsViewModel>(); // Используем Dependency Injection

                // Arrange
                ticketsViewModel.FilterItems[ticketsViewModel.FilterItems.Count - 1].Item.Comments = "Test";
                ticketsViewModel.FilterItems[ticketsViewModel.FilterItems.Count - 1].Item.Pet = GeneratePet();

                // Act
                ticketsViewModel.ExecuteAddTicketCommand();

                // Assert
                Assert.AreEqual(1, ticketsViewModel.Items.Count);
                Assert.AreEqual(_dbContext.Tickets.Count(), 1);
                Assert.AreEqual(ticketsViewModel.FilterItems.Count(), 2);
            }
        }

        [TestMethod()]
        public void ExecuteUpdateTicketCommandTest()
        {
            var ticketsViewModel = _serviceProvider.GetRequiredService<TicketsViewModel>();
            ticketsViewModel.FilterItems[ticketsViewModel.FilterItems.Count - 1].Item.Comments = "Test";
            ticketsViewModel.ExecuteAddTicketCommand();
            ticketsViewModel.FilterItems[0].Item.Comments = "TestUpdated";
            ticketsViewModel.ExecuteUpdateTicketCommand(ticketsViewModel.FilterItems[0].Item);
            Assert.AreEqual("TestUpdated", _dbContext.Tickets.Last().Comments);
        }

        [TestMethod()]
        public void ExecuteDeleteTicketCommandTest()
        {
            var ticketsViewModel = _serviceProvider.GetRequiredService<TicketsViewModel>();
            ticketsViewModel.FilterItems[ticketsViewModel.FilterItems.Count - 1].Item.Comments = "Test";
            ticketsViewModel.ExecuteAddTicketCommand();
            ticketsViewModel.ExecuteDeleteTicketCommand(_dbContext.Tickets.First());
            Assert.AreEqual(0, _dbContext.Tickets.Count());
            Assert.AreEqual(2, ticketsViewModel.FilterItems.Count()-1);
        }
        [TestMethod()]
        public void ExecuteDeleteTicketCommandTest_NoElements()
        {
            var ticketsViewModel = _serviceProvider.GetRequiredService<TicketsViewModel>();
            ticketsViewModel.ExecuteDeleteTicketCommand(ticketsViewModel.FilterItems.Last().Item);
            Assert.AreEqual(_dbContext.Tickets.Count(), 0);
            Assert.AreEqual(ticketsViewModel.FilterItems.Count(), 2);
        }
        [TestMethod()]
        public void ExecuteDeleteManyTicketsCommandTest()
        {
            var ticketsViewModel = _serviceProvider.GetRequiredService<TicketsViewModel>();
            ticketsViewModel.FilterItems[ticketsViewModel.FilterItems.Count - 1].Item.Comments = "Test1";
            ticketsViewModel.ExecuteAddTicketCommand();
            ticketsViewModel.FilterItems[ticketsViewModel.FilterItems.Count - 1].Item.Comments = "Test2";
            ticketsViewModel.ExecuteAddTicketCommand();
            ticketsViewModel.FilterItems[ticketsViewModel.FilterItems.Count - 1].Item.Comments = "Test3";
            ticketsViewModel.ExecuteAddTicketCommand();
            ticketsViewModel.FilterItems[ticketsViewModel.FilterItems.Count - 1].Item.Comments = "Test4";
            ticketsViewModel.ExecuteAddTicketCommand();
            ticketsViewModel.FilterItems[1].IsSelected = true;
            ticketsViewModel.FilterItems[3].IsSelected = true;
            ticketsViewModel.ExecuteDeleteManyTicketCommand();
            Assert.AreEqual(2, _dbContext.Tickets.Count());
            Assert.AreEqual(4, ticketsViewModel.FilterItems.Count()-2);
        }

        [TestMethod()]
        public void ExecuteDeleteManyTicketsCommandTest_NoElements()
        {
            var ticketsViewModel = _serviceProvider.GetRequiredService<TicketsViewModel>();
            ticketsViewModel.FilterItems[ticketsViewModel.FilterItems.Count - 1].Item.Comments = "Test1";
            ticketsViewModel.ExecuteAddTicketCommand();
            ticketsViewModel.FilterItems[ticketsViewModel.FilterItems.Count - 1].Item.Comments = "Test2";
            ticketsViewModel.ExecuteAddTicketCommand();
            ticketsViewModel.FilterItems[1].IsSelected = true;
            ticketsViewModel.FilterItems[2].IsSelected = true;
            ticketsViewModel.ExecuteDeleteManyTicketCommand();
            Assert.AreEqual(1, _dbContext.Tickets.Count());
            Assert.AreEqual(3, ticketsViewModel.FilterItems.Count()-1);
        }

        [TestMethod()]
        public void ExecuteChangeSelectionCommandTest()
        {
            var ticketsViewModel = _serviceProvider.GetRequiredService<TicketsViewModel>();
            ticketsViewModel.FilterItems[ticketsViewModel.FilterItems.Count - 1].Item.Comments = "Test1";
            ticketsViewModel.ExecuteAddTicketCommand();
            ticketsViewModel.FilterItems[ticketsViewModel.FilterItems.Count - 1].Item.Comments = "Test2";
            ticketsViewModel.ExecuteAddTicketCommand();
            ticketsViewModel.ExecuteChangeSelectionCommand(true);
            Assert.AreEqual(true, ticketsViewModel.FilterItems[0].IsSelected);
        }
    }
}