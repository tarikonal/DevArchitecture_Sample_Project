
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Campaigns.ValidationRules;


namespace Business.Handlers.Campaigns.Commands
{


    public class UpdateCampaignCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int DiscountRate { get; set; }
        public int GameId { get; set; }

        public class UpdateCampaignCommandHandler : IRequestHandler<UpdateCampaignCommand, IResult>
        {
            private readonly ICampaignRepository _campaignRepository;
            private readonly IMediator _mediator;

            public UpdateCampaignCommandHandler(ICampaignRepository campaignRepository, IMediator mediator)
            {
                _campaignRepository = campaignRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateCampaignValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateCampaignCommand request, CancellationToken cancellationToken)
            {
                var isThereCampaignRecord = await _campaignRepository.GetAsync(u => u.Id == request.Id);


                isThereCampaignRecord.Name = request.Name;
                isThereCampaignRecord.StartDate = request.StartDate;
                isThereCampaignRecord.EndDate = request.EndDate;
                isThereCampaignRecord.DiscountRate = request.DiscountRate;
                isThereCampaignRecord.GameId = request.GameId;


                _campaignRepository.Update(isThereCampaignRecord);
                await _campaignRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

