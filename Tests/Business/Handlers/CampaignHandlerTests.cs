
using Business.Handlers.Campaigns.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Campaigns.Queries.GetCampaignQuery;
using Entities.Concrete;
using static Business.Handlers.Campaigns.Queries.GetCampaignsQuery;
using static Business.Handlers.Campaigns.Commands.CreateCampaignCommand;
using Business.Handlers.Campaigns.Commands;
using Business.Constants;
using static Business.Handlers.Campaigns.Commands.UpdateCampaignCommand;
using static Business.Handlers.Campaigns.Commands.DeleteCampaignCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class CampaignHandlerTests
    {
        Mock<ICampaignRepository> _campaignRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _campaignRepository = new Mock<ICampaignRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Campaign_GetQuery_Success()
        {
            //Arrange
            var query = new GetCampaignQuery();

            _campaignRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Campaign, bool>>>())).ReturnsAsync(new Campaign()
//propertyler buraya yazılacak
//{																		
//CampaignId = 1,
//CampaignName = "Test"
//}
);

            var handler = new GetCampaignQueryHandler(_campaignRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.CampaignId.Should().Be(1);

        }

        [Test]
        public async Task Campaign_GetQueries_Success()
        {
            //Arrange
            var query = new GetCampaignsQuery();

            _campaignRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Campaign, bool>>>()))
                        .ReturnsAsync(new List<Campaign> { new Campaign() { /*TODO:propertyler buraya yazılacak CampaignId = 1, CampaignName = "test"*/ } });

            var handler = new GetCampaignsQueryHandler(_campaignRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Campaign>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Campaign_CreateCommand_Success()
        {
            Campaign rt = null;
            //Arrange
            var command = new CreateCampaignCommand();
            //propertyler buraya yazılacak
            //command.CampaignName = "deneme";

            _campaignRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Campaign, bool>>>()))
                        .ReturnsAsync(rt);

            _campaignRepository.Setup(x => x.Add(It.IsAny<Campaign>())).Returns(new Campaign());

            var handler = new CreateCampaignCommandHandler(_campaignRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _campaignRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Campaign_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateCampaignCommand();
            //propertyler buraya yazılacak 
            //command.CampaignName = "test";

            _campaignRepository.Setup(x => x.Query())
                                           .Returns(new List<Campaign> { new Campaign() { /*TODO:propertyler buraya yazılacak CampaignId = 1, CampaignName = "test"*/ } }.AsQueryable());

            _campaignRepository.Setup(x => x.Add(It.IsAny<Campaign>())).Returns(new Campaign());

            var handler = new CreateCampaignCommandHandler(_campaignRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Campaign_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateCampaignCommand();
            //command.CampaignName = "test";

            _campaignRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Campaign, bool>>>()))
                        .ReturnsAsync(new Campaign() { /*TODO:propertyler buraya yazılacak CampaignId = 1, CampaignName = "deneme"*/ });

            _campaignRepository.Setup(x => x.Update(It.IsAny<Campaign>())).Returns(new Campaign());

            var handler = new UpdateCampaignCommandHandler(_campaignRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _campaignRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Campaign_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteCampaignCommand();

            _campaignRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Campaign, bool>>>()))
                        .ReturnsAsync(new Campaign() { /*TODO:propertyler buraya yazılacak CampaignId = 1, CampaignName = "deneme"*/});

            _campaignRepository.Setup(x => x.Delete(It.IsAny<Campaign>()));

            var handler = new DeleteCampaignCommandHandler(_campaignRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _campaignRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

