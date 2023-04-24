
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


namespace Business.Handlers.Games.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteGameCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand, IResult>
        {
            private readonly IGameRepository _gameRepository;
            private readonly IMediator _mediator;

            public DeleteGameCommandHandler(IGameRepository gameRepository, IMediator mediator)
            {
                _gameRepository = gameRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
            {
                var gameToDelete = _gameRepository.Get(p => p.Id == request.Id);

                _gameRepository.Delete(gameToDelete);
                await _gameRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

