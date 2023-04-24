
using Business.Handlers.Gamers.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Gamers.Queries.GetGamerQuery;
using Entities.Concrete;
using static Business.Handlers.Gamers.Queries.GetGamersQuery;
using static Business.Handlers.Gamers.Commands.CreateGamerCommand;
using Business.Handlers.Gamers.Commands;
using Business.Constants;
using static Business.Handlers.Gamers.Commands.UpdateGamerCommand;
using static Business.Handlers.Gamers.Commands.DeleteGamerCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class GamerHandlerTests
    {
        Mock<IGamerRepository> _gamerRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _gamerRepository = new Mock<IGamerRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Gamer_GetQuery_Success()
        {
            //Arrange
            var query = new GetGamerQuery();

            _gamerRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Gamer, bool>>>())).ReturnsAsync(new Gamer()
//propertyler buraya yazılacak
//{																		
//GamerId = 1,
//GamerName = "Test"
//}
);

            var handler = new GetGamerQueryHandler(_gamerRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.GamerId.Should().Be(1);

        }

        [Test]
        public async Task Gamer_GetQueries_Success()
        {
            //Arrange
            var query = new GetGamersQuery();

            _gamerRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Gamer, bool>>>()))
                        .ReturnsAsync(new List<Gamer> { new Gamer() { /*TODO:propertyler buraya yazılacak GamerId = 1, GamerName = "test"*/ } });

            var handler = new GetGamersQueryHandler(_gamerRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Gamer>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Gamer_CreateCommand_Success()
        {
            Gamer rt = null;
            //Arrange
            var command = new CreateGamerCommand();
            //propertyler buraya yazılacak
            //command.GamerName = "deneme";

            _gamerRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Gamer, bool>>>()))
                        .ReturnsAsync(rt);

            _gamerRepository.Setup(x => x.Add(It.IsAny<Gamer>())).Returns(new Gamer());

            var handler = new CreateGamerCommandHandler(_gamerRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _gamerRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Gamer_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateGamerCommand();
            //propertyler buraya yazılacak 
            //command.GamerName = "test";

            _gamerRepository.Setup(x => x.Query())
                                           .Returns(new List<Gamer> { new Gamer() { /*TODO:propertyler buraya yazılacak GamerId = 1, GamerName = "test"*/ } }.AsQueryable());

            _gamerRepository.Setup(x => x.Add(It.IsAny<Gamer>())).Returns(new Gamer());

            var handler = new CreateGamerCommandHandler(_gamerRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Gamer_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateGamerCommand();
            //command.GamerName = "test";

            _gamerRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Gamer, bool>>>()))
                        .ReturnsAsync(new Gamer() { /*TODO:propertyler buraya yazılacak GamerId = 1, GamerName = "deneme"*/ });

            _gamerRepository.Setup(x => x.Update(It.IsAny<Gamer>())).Returns(new Gamer());

            var handler = new UpdateGamerCommandHandler(_gamerRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _gamerRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Gamer_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteGamerCommand();

            _gamerRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Gamer, bool>>>()))
                        .ReturnsAsync(new Gamer() { /*TODO:propertyler buraya yazılacak GamerId = 1, GamerName = "deneme"*/});

            _gamerRepository.Setup(x => x.Delete(It.IsAny<Gamer>()));

            var handler = new DeleteGamerCommandHandler(_gamerRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _gamerRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

