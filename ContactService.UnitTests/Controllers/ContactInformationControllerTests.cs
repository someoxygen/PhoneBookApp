using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ContactService.UnitTests.Controllers;

public class ContactInformationControllerTests
{
    private readonly Mock<IContactInformationService> _serviceMock;
    private readonly ContactInformationController _controller;

    public ContactInformationControllerTests()
    {
        _serviceMock = new Mock<IContactInformationService>();
        _controller = new ContactInformationController(_serviceMock.Object);
    }

    [Fact]
    public async Task Add_Should_Return_OkResult()
    {
        // Arrange
        var contactInformation = new ContactInformation
        {
            Type = ContactType.PhoneNumber,
            Content = "12345",
            PersonId = Guid.NewGuid()
        };

        _serviceMock
            .Setup(x => x.AddContactInformationAsync(It.IsAny<ContactInformation>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Add(contactInformation);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(contactInformation);

        _serviceMock.Verify(x => x.AddContactInformationAsync(contactInformation), Times.Once);
    }

    [Fact]
    public async Task Remove_Should_Return_NoContent()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        _serviceMock
            .Setup(x => x.RemoveContactInformationAsync(contactId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Remove(contactId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _serviceMock.Verify(x => x.RemoveContactInformationAsync(contactId), Times.Once);
    }
    [Fact]
    public async Task GetByPersonId_Should_Return_Ok_With_ContactInformations()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var contacts = new List<ContactInformation>
    {
        new() { PersonId = personId, Content = "12345" }
    };

        _serviceMock.Setup(s => s.GetByPersonIdAsync(personId)).ReturnsAsync(contacts);

        // Act
        var result = await _controller.GetByPersonId(personId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(contacts);
    }

}
