using System.Threading;
using ContactService.Domain.Entities;
using ContactService.Infrastructure.Mongo.Context;
using ContactService.Infrastructure.Mongo.Repositories;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace ContactService.UnitTests.Repositories;

public class PersonRepositoryTests
{
    private readonly PersonRepository _repository;
    private readonly Mock<IMongoCollection<Person>> _collectionMock;
    private readonly Mock<IMongoContext> _mongoContextMock;

    public PersonRepositoryTests()
    {
        _collectionMock = new Mock<IMongoCollection<Person>>();
        _mongoContextMock = new Mock<IMongoContext>();

        _mongoContextMock.Setup(x => x.Persons).Returns(_collectionMock.Object);

        _repository = new PersonRepository(_mongoContextMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Call_InsertOneAsync()
    {
        var person = new Person();

        await _repository.CreateAsync(person);

        _collectionMock.Verify(x =>
            x.InsertOneAsync(person, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_Should_Call_DeleteOneAsync()
    {
        var id = Guid.NewGuid();

        await _repository.RemoveAsync(id);

        _collectionMock.Verify(x =>
            x.DeleteOneAsync(It.IsAny<FilterDefinition<Person>>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Call_Find()
    {
        var id = Guid.NewGuid();
        var expectedPerson = new Person { Id = id };

        var cursorMock = new Mock<IAsyncCursor<Person>>();
        cursorMock.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);
        cursorMock.SetupGet(x => x.Current).Returns(new List<Person> { expectedPerson });

        _collectionMock.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<Person>>(),
                It.IsAny<FindOptions<Person, Person>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursorMock.Object);

        var result = await _repository.GetByIdAsync(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }


    [Fact]
    public async Task GetAllAsync_Should_Return_List()
    {
        var expectedList = new List<Person> { new Person() };

        var cursorMock = new Mock<IAsyncCursor<Person>>();
        cursorMock.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);
        cursorMock.SetupGet(x => x.Current).Returns(expectedList);

        _collectionMock.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<Person>>(),
                It.IsAny<FindOptions<Person, Person>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursorMock.Object);

        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.Single(result);
    }

}
