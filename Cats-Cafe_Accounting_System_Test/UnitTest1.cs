// Добавьте ссылки на необходимые библиотеки для тестирования (xUnit, Moq)
using Cats_Cafe_Accounting_System.ViewModels;
using Moq;
using System.Collections.Generic;
using Xunit;

// Пример теста для метода ExecuteAddPetCommand
public class PetsViewModelTests
{
    [Fact]
    public void ExecuteAddPetCommand_WhenCalled_AddsPetToDataContext()
    {
        // Arrange
        var mockDbContext = new Mock<YourDbContext>();
        var mockDbSet = new Mock<DbSet<PetModel>>();
        mockDbContext.Setup(x => x.Pets).Returns(mockDbSet.Object);

        var viewModel = new PetsViewModel(mockDbContext.Object);

        // Mock existing last pet
        viewModel.FilterItems.Add(new Elem<PetModel>(new PetModel
        {
            Name = "TestDog",
            Gender = new Gender { Id = 1, Name = "Male" },
            Status = new Status { Id = 1, Name = "Available" },
            Breed = new Breed { Id = 1, Name = "Labrador Retriever" },
            Birthday = DateTime.Today,
            CheckInDate = DateTime.Today,
            PassNumber = "123456789"
        });

        // Act
        viewModel.ExecuteAddPetCommand();

        // Assert
        mockDbSet.Verify(x => x.Add(It.IsAny<PetModel>()), Times.Once);
        mockDbContext.Verify(x => x.SaveChanges(), Times.Once);
        Assert.Contains(viewModel.Names, p => p.Item.Name == "TestDog"); // Check if the new pet is added to the Names collection
    }
}