
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Campaigns.ValidationRules;

namespace Business.Handlers.Campaigns.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateCampaignCommand : IRequest<IResult>
    {

        public string Name { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int DiscountRate { get; set; }
        public int GameId { get; set; }


        public class CreateCampaignCommandHandler : IRequestHandler<CreateCampaignCommand, IResult>
        {
            private readonly ICampaignRepository _campaignRepository;
            private readonly IMediator _mediator;
            public CreateCampaignCommandHandler(ICampaignRepository campaignRepository, IMediator mediator)
            {
                _campaignRepository = campaignRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateCampaignValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateCampaignCommand request, CancellationToken cancellationToken)
            {
                var isThereCampaignRecord = _campaignRepository.Query().Any(u => u.Name == request.Name);

                if (isThereCampaignRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedCampaign = new Campaign
                {
                    Name = request.Name,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    DiscountRate = request.DiscountRate,
                    GameId = request.GameId,

                };

                _campaignRepository.Add(addedCampaign);
                await _campaignRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}