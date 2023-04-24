
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
using Business.Handlers.Games.ValidationRules;


namespace Business.Handlers.Games.Commands
{


    public class UpdateGameCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommand, IResult>
        {
            private readonly IGameRepository _gameRepository;
            private readonly IMediator _mediator;

            public UpdateGameCommandHandler(IGameRepository gameRepository, IMediator mediator)
            {
                _gameRepository = gameRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateGameValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
            {
                var isThereGameRecord = await _gameRepository.GetAsync(u => u.Id == request.Id);


                isThereGameRecord.Name = request.Name;
                isThereGameRecord.Description = request.Description;
                isThereGameRecord.Price = request.Price;


                _gameRepository.Update(isThereGameRecord);
                await _gameRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

