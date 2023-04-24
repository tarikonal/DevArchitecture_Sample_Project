
using Business.Handlers.Games.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Games.Queries.GetGameQuery;
using Entities.Concrete;
using static Business.Handlers.Games.Queries.GetGamesQuery;
using static Business.Handlers.Games.Commands.CreateGameCommand;
using Business.Handlers.Games.Commands;
using Business.Constants;
using static Business.Handlers.Games.Commands.UpdateGameCommand;
using static Business.Handlers.Games.Commands.DeleteGameCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class GameHandlerTests
    {
        Mock<IGameRepository> _gameRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _gameRepository = new Mock<IGameRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Game_GetQuery_Success()
        {
            //Arrange
            var query = new GetGameQuery();

            _gameRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Game, bool>>>())).ReturnsAsync(new Game()
//propertyler buraya yazılacak
//{																		
//GameId = 1,
//GameName = "Test"
//}
);

            var handler = new GetGameQueryHandler(_gameRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.GameId.Should().Be(1);

        }

        [Test]
        public async Task Game_GetQueries_Success()
        {
            //Arrange
            var query = new GetGamesQuery();

            _gameRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                        .ReturnsAsync(new List<Game> { new Game() { /*TODO:propertyler buraya yazılacak GameId = 1, GameName = "test"*/ } });

            var handler = new GetGamesQueryHandler(_gameRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Game>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Game_CreateCommand_Success()
        {
            Game rt = null;
            //Arrange
            var command = new CreateGameCommand();
            //propertyler buraya yazılacak
            //command.GameName = "deneme";

            _gameRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                        .ReturnsAsync(rt);

            _gameRepository.Setup(x => x.Add(It.IsAny<Game>())).Returns(new Game());

            var handler = new CreateGameCommandHandler(_gameRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _gameRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Game_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateGameCommand();
            //propertyler buraya yazılacak 
            //command.GameName = "test";

            _gameRepository.Setup(x => x.Query())
                                           .Returns(new List<Game> { new Game() { /*TODO:propertyler buraya yazılacak GameId = 1, GameName = "test"*/ } }.AsQueryable());

            _gameRepository.Setup(x => x.Add(It.IsAny<Game>())).Returns(new Game());

            var handler = new CreateGameCommandHandler(_gameRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Game_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateGameCommand();
            //command.GameName = "test";

            _gameRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                        .ReturnsAsync(new Game() { /*TODO:propertyler buraya yazılacak GameId = 1, GameName = "deneme"*/ });

            _gameRepository.Setup(x => x.Update(It.IsAny<Game>())).Returns(new Game());

            var handler = new UpdateGameCommandHandler(_gameRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _gameRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Game_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteGameCommand();

            _gameRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Game, bool>>>()))
                        .ReturnsAsync(new Game() { /*TODO:propertyler buraya yazılacak GameId = 1, GameName = "deneme"*/});

            _gameRepository.Setup(x => x.Delete(It.IsAny<Game>()));

            var handler = new DeleteGameCommandHandler(_gameRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _gameRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

