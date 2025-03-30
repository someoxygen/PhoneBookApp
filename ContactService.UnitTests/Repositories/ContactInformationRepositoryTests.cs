using System.Threading;
using ContactService.Domain.Entities;
using ContactService.Infrastructure.Mongo.Context;
using ContactService.Infrastructure.Mongo.Repositories;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace ContactService.UnitTests.Repositories;

public class ContactInformationRepositoryTests
{
    private readonly ContactInformationRepository _repository;
    private readonly Mock<IMongoCollection<ContactInformation>> _collectionMock;
    private readonly Mock<IMongoContext> _mongoContextMock;

    public ContactInformationRepositoryTests()
    {
        _collectionMock = new Mock<IMongoCollection<ContactInformation>>();
        _mongoContextMock = new Mock<IMongoContext>();

        // MongoContext.ContactInformations özelliğini mockluyoruz
        _mongoContextMock.Setup(x => x.ContactInformations).Returns(_collectionMock.Object);

        _repository = new ContactInformationRepository(_mongoContextMock.Object);
    }

    [Fact]
    public async Task AddAsync_Should_Call_InsertOneAsync()
    {
        var contact = new ContactInformation();

        await _repository.AddAsync(contact);

        _collectionMock.Verify(x =>
            x.InsertOneAsync(contact, null, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_Should_Call_DeleteOneAsync()
    {
        var id = Guid.NewGuid();

        await _repository.RemoveAsync(id);

        _collectionMock.Verify(x =>
            x.DeleteOneAsync(It.IsAny<FilterDefinition<ContactInformation>>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetByPersonIdAsync_Should_Call_Find_With_Filter()
    {
        var personId = Guid.NewGuid();

        var contactList = new List<ContactInformation>
    {
        new ContactInformation { PersonId = personId }
    };

        var cursorMock = new Mock<IAsyncCursor<ContactInformation>>();
        cursorMock.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);

        cursorMock.SetupGet(c => c.Current).Returns(contactList);

        _collectionMock.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<ContactInformation>>(),
                It.IsAny<FindOptions<ContactInformation, ContactInformation>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursorMock.Object);

        var result = await _repository.GetByPersonIdAsync(personId);

        Assert.NotNull(result);
        Assert.Single(result);
    }

}
