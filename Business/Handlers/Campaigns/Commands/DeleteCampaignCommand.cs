
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.Campaigns.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteCampaignCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteCampaignCommandHandler : IRequestHandler<DeleteCampaignCommand, IResult>
        {
            private readonly ICampaignRepository _campaignRepository;
            private readonly IMediator _mediator;

            public DeleteCampaignCommandHandler(ICampaignRepository campaignRepository, IMediator mediator)
            {
                _campaignRepository = campaignRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteCampaignCommand request, CancellationToken cancellationToken)
            {
                var campaignToDelete = _campaignRepository.Get(p => p.Id == request.Id);

                _campaignRepository.Delete(campaignToDelete);
                await _campaignRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

