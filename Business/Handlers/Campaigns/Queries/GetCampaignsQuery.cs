
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.Campaigns.Queries
{

    public class GetCampaignsQuery : IRequest<IDataResult<IEnumerable<Campaign>>>
    {
        public class GetCampaignsQueryHandler : IRequestHandler<GetCampaignsQuery, IDataResult<IEnumerable<Campaign>>>
        {
            private readonly ICampaignRepository _campaignRepository;
            private readonly IMediator _mediator;

            public GetCampaignsQueryHandler(ICampaignRepository campaignRepository, IMediator mediator)
            {
                _campaignRepository = campaignRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Campaign>>> Handle(GetCampaignsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Campaign>>(await _campaignRepository.GetListAsync());
            }
        }
    }
}