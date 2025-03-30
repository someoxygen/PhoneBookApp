using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ContactService.UnitTests.Services;

public class ContactInformationServiceTests
{
    private readonly Mock<IContactInformationRepository> _repositoryMock;
    private readonly ContactInformationService _service;

    public ContactInformationServiceTests()
    {
        _repositoryMock = new Mock<IContactInformationRepository>();
        _service = new ContactInformationService(_repositoryMock.Object);
    }

    [Fact]
    public async Task AddContactInformationAsync_Should_Call_Repository_Method()
    {
        // Arrange
        var contactInformation = new ContactInformation
        {
            Type = ContactType.PhoneNumber,
            Content = "+905551112233",
            PersonId = Guid.NewGuid()
        };

        // Act
        await _service.AddContactInformationAsync(contactInformation);

        // Assert
        _repositoryMock.Verify(repo => repo.AddAsync(contactInformation), Times.Once);
    }

    [Fact]
    public async Task GetByPersonIdAsync_Should_Return_ContactInformations()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var contactInformations = new List<ContactInformation>
        {
            new() { Type = ContactType.PhoneNumber, Content = "+905551112233", PersonId = personId },
            new() { Type = ContactType.EmailAddress, Content = "test@example.com", PersonId = personId }
        };

        _repositoryMock.Setup(repo => repo.GetByPersonIdAsync(personId))
                       .ReturnsAsync(contactInformations);

        // Act
        var result = await _service.GetByPersonIdAsync(personId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(contactInformations);
    }

    [Fact]
    public async Task RemoveContactInformationAsync_Should_Call_Repository_Method()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        // Act
        await _service.RemoveContactInformationAsync(contactId);

        // Assert
        _repositoryMock.Verify(repo => repo.RemoveAsync(contactId), Times.Once);
    }
    [Fact]
    public async Task RemoveContactInformationAsync_Should_Call_Repository_Remove()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        // Act
        await _service.RemoveContactInformationAsync(contactId);

        // Assert
        _repositoryMock.Verify(x => x.RemoveAsync(contactId), Times.Once);
    }
    [Fact]
    public async Task GetByPersonIdAsync_Should_Return_Contacts()
    {
        var personId = Guid.NewGuid();
        var expectedContacts = new List<ContactInformation>
    {
        new ContactInformation { PersonId = personId, Content = "5555555" }
    };

        _repositoryMock.Setup(x => x.GetByPersonIdAsync(personId))
                       .ReturnsAsync(expectedContacts);

        var result = await _service.GetByPersonIdAsync(personId);

        result.Should().BeEquivalentTo(expectedContacts);
    }

}
