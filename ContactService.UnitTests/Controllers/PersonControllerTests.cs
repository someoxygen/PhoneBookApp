using ContactService.API.Controllers;
using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ContactService.UnitTests.Controllers;

public class PersonControllerTests
{
    private readonly Mock<IPersonService> _personServiceMock;
    private readonly PersonController _controller;

    public PersonControllerTests()
    {
        _personServiceMock = new Mock<IPersonService>();
        _controller = new PersonController(_personServiceMock.Object);
    }

    [Fact]
    public async Task CreatePerson_Should_Return_CreatedResult()
    {
        var person = new Person { Name = "Test", Surname = "User" };

        _personServiceMock.Setup(x => x.CreatePersonAsync(person)).Returns(Task.CompletedTask);

        var result = await _controller.CreatePerson(person);

        result.Should().BeOfType<CreatedAtActionResult>();
        _personServiceMock.Verify(x => x.CreatePersonAsync(person), Times.Once);
    }

    [Fact]
    public async Task GetAllPersons_Should_Return_Ok_With_Persons()
    {
        var persons = new List<Person> { new Person() };
        _personServiceMock.Setup(s => s.GetAllPersonsAsync()).ReturnsAsync(persons);

        var result = await _controller.GetAllPersons();

        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(persons);
    }
    [Fact]
    public async Task GetPersonById_Should_Return_OkResult_When_PersonExists()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var person = new Person { Id = personId, Name = "Test", Surname = "User" };

        _personServiceMock.Setup(s => s.GetPersonByIdAsync(personId)).ReturnsAsync(person);

        // Act
        var result = await _controller.GetPersonById(personId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(person);
    }

    [Fact]
    public async Task GetPersonById_Should_Return_NotFound_When_PersonNotExists()
    {
        // Arrange
        var personId = Guid.NewGuid();
        _personServiceMock.Setup(s => s.GetPersonByIdAsync(personId)).ReturnsAsync((Person?)null);

        // Act
        var result = await _controller.GetPersonById(personId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeletePerson_Should_Return_NoContent()
    {
        // Arrange
        var personId = Guid.NewGuid();
        _personServiceMock.Setup(s => s.RemovePersonAsync(personId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeletePerson(personId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _personServiceMock.Verify(s => s.RemovePersonAsync(personId), Times.Once);
    }

}
