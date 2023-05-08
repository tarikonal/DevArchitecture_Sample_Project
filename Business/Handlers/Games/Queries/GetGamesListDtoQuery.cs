using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Dtos;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Games.Queries
{
    public class GetGamesListDtoQuery : IRequest<IDataResult<IEnumerable<GameDto>>>
    {
        public class
            GetGamesListDtoQueryHandler : IRequestHandler<GetGamesListDtoQuery,
                IDataResult<IEnumerable<GameDto>>>
        {
            private readonly IGameRepository _gameRepository;
            private readonly IMediator _mediator;

            public GetGamesListDtoQueryHandler(IGameRepository gameRepository, IMediator mediator)
            {
                _gameRepository = gameRepository;
                _mediator = mediator;
            }

            [SecuredOperation(Priority = 1)]
            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<IEnumerable<GameDto>>> Handle(GetGamesListDtoQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<GameDto>>(await _gameRepository.GetGameDto());
            }

          
        }
    }
}