using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ContactService.UnitTests.Services;

public class PersonServiceTests
{
    private readonly Mock<IPersonRepository> _personRepositoryMock;
    private readonly PersonService _personService;

    public PersonServiceTests()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();
        _personService = new PersonService(_personRepositoryMock.Object);
    }

    [Fact]
    public async Task CreatePersonAsync_Should_Call_Repository_Method()
    {
        // Arrange
        var person = new Person { Name = "Ahmet", Surname = "Yılmaz", Company = "ABC Ltd" };

        // Act
        await _personService.CreatePersonAsync(person);

        // Assert
        _personRepositoryMock.Verify(repo => repo.CreateAsync(person), Times.Once);
    }

    [Fact]
    public async Task GetAllPersonsAsync_Should_Return_PersonList()
    {
        // Arrange
        var persons = new List<Person>
        {
            new Person { Name = "Ahmet", Surname = "Yılmaz" },
            new Person { Name = "Mehmet", Surname = "Kaya" }
        };

        _personRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(persons);

        // Act
        var result = await _personService.GetAllPersonsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(persons);
    }

    [Fact]
    public async Task GetPersonByIdAsync_Should_Return_Person()
    {
        var personId = Guid.NewGuid();
        var person = new Person { Id = personId };

        _personRepositoryMock.Setup(x => x.GetByIdAsync(personId)).ReturnsAsync(person);

        var result = await _personService.GetPersonByIdAsync(personId);

        result.Should().BeEquivalentTo(person);
    }
    [Fact]
    public async Task RemovePersonAsync_Should_Call_Repository_Remove()
    {
        var personId = Guid.NewGuid();

        await _personService.RemovePersonAsync(personId);

        _personRepositoryMock.Verify(x => x.RemoveAsync(personId), Times.Once);
    }


}
