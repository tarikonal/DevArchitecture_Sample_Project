
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
using Business.Handlers.Games.ValidationRules;

namespace Business.Handlers.Games.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateGameCommand : IRequest<IResult>
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }


        public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, IResult>
        {
            private readonly IGameRepository _gameRepository;
            private readonly IMediator _mediator;
            public CreateGameCommandHandler(IGameRepository gameRepository, IMediator mediator)
            {
                _gameRepository = gameRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateGameValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateGameCommand request, CancellationToken cancellationToken)
            {
                var isThereGameRecord = _gameRepository.Query().Any(u => u.Name == request.Name);

                if (isThereGameRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedGame = new Game
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,

                };

                _gameRepository.Add(addedGame);
                await _gameRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}