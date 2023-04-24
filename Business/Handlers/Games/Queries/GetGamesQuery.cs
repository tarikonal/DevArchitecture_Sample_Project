
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

namespace Business.Handlers.Games.Queries
{

    public class GetGamesQuery : IRequest<IDataResult<IEnumerable<Game>>>
    {
        public class GetGamesQueryHandler : IRequestHandler<GetGamesQuery, IDataResult<IEnumerable<Game>>>
        {
            private readonly IGameRepository _gameRepository;
            private readonly IMediator _mediator;

            public GetGamesQueryHandler(IGameRepository gameRepository, IMediator mediator)
            {
                _gameRepository = gameRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Game>>> Handle(GetGamesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Game>>(await _gameRepository.GetListAsync());
            }
        }
    }
}