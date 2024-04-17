using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cats_Cafe_Accounting_System.ViewModels.Tests
{
    [TestClass()]
    public class JobsViewModelTests
    {
        private ServiceProvider _serviceProvider;
        private static ApplicationDbContext _dbContext;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _dbContext = ApplicationDbContext.CreateInMemoryDatabase();

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
            services.AddSingleton(new JobsViewModel(_dbContext));
            _serviceProvider = services.BuildServiceProvider();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            foreach (var job in _dbContext.Jobs)
            {
                _dbContext.Jobs.Remove(job);
            }
            _dbContext.SaveChanges();
        }

        [TestMethod()]
        public void ExecuteAddJobCommand_ShouldAddNewJob()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var jobsViewModel = scope.ServiceProvider.GetRequiredService<JobsViewModel>(); // Используем Dependency Injection

                // Arrange
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test";
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;

                // Act
                jobsViewModel.ExecuteAddJobCommand();

                // Assert
                Assert.AreEqual(1, jobsViewModel.Items.Count);
                Assert.AreEqual(_dbContext.Jobs.Count(), 1);
                Assert.AreEqual(jobsViewModel.Titles.Count(), 1);
                Assert.AreEqual(jobsViewModel.FilterTitles.Count(), 1);
                Assert.AreEqual(jobsViewModel.FilterItems.Count(), 2);
            }
        }

        [TestMethod()]
        public void ExecuteSearchTitleCommandTest()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var jobsViewModel = scope.ServiceProvider.GetRequiredService<JobsViewModel>();

                // Arranges and Acts
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test";
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
                jobsViewModel.ExecuteAddJobCommand();
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test2";
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
                jobsViewModel.ExecuteAddJobCommand();
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Title";
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
                jobsViewModel.ExecuteAddJobCommand();
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test3";
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
                jobsViewModel.ExecuteAddJobCommand();
                jobsViewModel.SearchTitle = "Tes";
                jobsViewModel.ExecuteSearchTitleCommand();
                // Assert
                Assert.AreEqual(3, jobsViewModel.FilterTitles.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchTitleCommandTest_EmptySearchText()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var jobsViewModel = scope.ServiceProvider.GetRequiredService<JobsViewModel>();

                // Arranges and Acts
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test";
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
                jobsViewModel.ExecuteAddJobCommand();
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test2";
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
                jobsViewModel.ExecuteAddJobCommand();
                jobsViewModel.SearchTitle = "Поиск по должности";
                jobsViewModel.ExecuteSearchTitleCommand();
                // Assert
                Assert.AreEqual("Поиск по должности", jobsViewModel.SearchTitle);
            }
        }

        [TestMethod()]
        public void ExecuteUpdateJobCommandTest()
        {
            var jobsViewModel = _serviceProvider.GetRequiredService<JobsViewModel>();
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test";
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
            jobsViewModel.ExecuteAddJobCommand();
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 2].Item.Title = "TestUpdated";
            jobsViewModel.ExecuteUpdateJobCommand(jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 2].Item);
            Assert.AreEqual("TestUpdated", _dbContext.Jobs.Last().Title);
            Assert.AreEqual("TestUpdated", jobsViewModel.Titles.Last().Item.Title);
            Assert.AreEqual("TestUpdated", jobsViewModel.FilterTitles.Last().Item.Title);
        }

        [TestMethod()]
        public void ExecuteDeleteJobCommandTest()
        {
            var jobsViewModel = _serviceProvider.GetRequiredService<JobsViewModel>();
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test";
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
            jobsViewModel.ExecuteAddJobCommand();
            jobsViewModel.ExecuteDeleteJobCommand(_dbContext.Jobs.First());
            Assert.AreEqual(_dbContext.Jobs.Count(), 0);
            Assert.AreEqual(jobsViewModel.Titles.Count(), 0);
            Assert.AreEqual(jobsViewModel.FilterTitles.Count(), 0);
            Assert.AreEqual(jobsViewModel.FilterItems.Count(), 1);
        }
        [TestMethod()]
        public void ExecuteDeleteJobCommandTest_NoElements()
        {
            var jobsViewModel = _serviceProvider.GetRequiredService<JobsViewModel>();
            jobsViewModel.ExecuteDeleteJobCommand(jobsViewModel.FilterItems.Last().Item);
            Assert.AreEqual(_dbContext.Jobs.Count(), 0);
            Assert.AreEqual(jobsViewModel.Titles.Count(), 0);
            Assert.AreEqual(jobsViewModel.FilterTitles.Count(), 0);
            Assert.AreEqual(jobsViewModel.FilterItems.Count(), 1);
        }
        [TestMethod()]
        public void ExecuteDeleteManyJobsCommandTest()
        {
            var jobsViewModel = _serviceProvider.GetRequiredService<JobsViewModel>();
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test1";
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
            jobsViewModel.ExecuteAddJobCommand();
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test2";
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
            jobsViewModel.ExecuteAddJobCommand();
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test3";
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
            jobsViewModel.ExecuteAddJobCommand();
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test4";
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
            jobsViewModel.ExecuteAddJobCommand();
            jobsViewModel.FilterItems[1].IsSelected = true;
            jobsViewModel.FilterItems[3].IsSelected = true;
            jobsViewModel.ExecuteDeleteManyJobsCommand();
            Assert.AreEqual(2, _dbContext.Jobs.Count());
            Assert.AreEqual(2, jobsViewModel.Titles.Count());
            Assert.AreEqual(2, jobsViewModel.FilterTitles.Count());
            Assert.AreEqual(3, jobsViewModel.FilterItems.Count());
        }

        [TestMethod()]
        public void ExecuteDeleteManyJobsCommandTest_NoElements()
        {
            var jobsViewModel = _serviceProvider.GetRequiredService<JobsViewModel>();
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test1";
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
            jobsViewModel.ExecuteAddJobCommand();
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test2";
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
            jobsViewModel.ExecuteAddJobCommand();
            jobsViewModel.FilterItems[1].IsSelected = true;
            jobsViewModel.FilterItems[2].IsSelected = true;
            jobsViewModel.ExecuteDeleteManyJobsCommand();
            Assert.AreEqual(1, _dbContext.Jobs.Count());
            Assert.AreEqual(1, jobsViewModel.Titles.Count());
            Assert.AreEqual(1, jobsViewModel.FilterTitles.Count());
            Assert.AreEqual(2, jobsViewModel.FilterItems.Count());
        }

        [TestMethod()]
        public void ExecuteChangeSelectionCommandTest()
        {
            var jobsViewModel = _serviceProvider.GetRequiredService<JobsViewModel>();
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test1";
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
            jobsViewModel.ExecuteAddJobCommand();
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test2";
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
            jobsViewModel.ExecuteAddJobCommand();
            jobsViewModel.ExecuteChangeSelectionCommand(true);
            Assert.AreEqual(true, jobsViewModel.FilterItems[0].IsSelected);
        }

        [TestMethod()]
        public void ExecuteChangeTitleSelectionCommandTest()
        {
            var jobsViewModel = _serviceProvider.GetRequiredService<JobsViewModel>();
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test1";
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
            jobsViewModel.ExecuteAddJobCommand();
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test2";
            jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
            jobsViewModel.ExecuteAddJobCommand();
            jobsViewModel.ExecuteChangeTitleSelectionCommand(true);
            Assert.AreEqual(false, jobsViewModel.FilterTitles[0].IsSelected);
        }

        [TestMethod()]
        public void ExecuteSearchCommandTest_ByTitle()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var jobsViewModel = scope.ServiceProvider.GetRequiredService<JobsViewModel>();
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test1";
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
                jobsViewModel.ExecuteAddJobCommand();
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test12";
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
                jobsViewModel.ExecuteAddJobCommand();
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test3";
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
                jobsViewModel.ExecuteAddJobCommand();
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Title = "Test4";
                jobsViewModel.FilterItems[jobsViewModel.FilterItems.Count - 1].Item.Rate = 0;
                jobsViewModel.ExecuteAddJobCommand();
                // Arranges and Acts
                jobsViewModel.SearchText = "Test1";
                jobsViewModel.ExecuteSearchCommand();
                // Assert
                Assert.AreEqual(2, jobsViewModel.FilterItems.Count - 1);
            }
        }
    }
}