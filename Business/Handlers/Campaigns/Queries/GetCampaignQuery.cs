
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Campaigns.Queries
{
    public class GetCampaignQuery : IRequest<IDataResult<Campaign>>
    {
        public int Id { get; set; }

        public class GetCampaignQueryHandler : IRequestHandler<GetCampaignQuery, IDataResult<Campaign>>
        {
            private readonly ICampaignRepository _campaignRepository;
            private readonly IMediator _mediator;

            public GetCampaignQueryHandler(ICampaignRepository campaignRepository, IMediator mediator)
            {
                _campaignRepository = campaignRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Campaign>> Handle(GetCampaignQuery request, CancellationToken cancellationToken)
            {
                var campaign = await _campaignRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Campaign>(campaign);
            }
        }
    }
}
