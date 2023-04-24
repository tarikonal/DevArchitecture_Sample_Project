
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

namespace Business.Handlers.Gamers.Queries
{

    public class GetGamersQuery : IRequest<IDataResult<IEnumerable<Gamer>>>
    {
        public class GetGamersQueryHandler : IRequestHandler<GetGamersQuery, IDataResult<IEnumerable<Gamer>>>
        {
            private readonly IGamerRepository _gamerRepository;
            private readonly IMediator _mediator;

            public GetGamersQueryHandler(IGamerRepository gamerRepository, IMediator mediator)
            {
                _gamerRepository = gamerRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Gamer>>> Handle(GetGamersQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Gamer>>(await _gamerRepository.GetListAsync());
            }
        }
    }
}