
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


namespace Business.Handlers.Gamers.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteGamerCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteGamerCommandHandler : IRequestHandler<DeleteGamerCommand, IResult>
        {
            private readonly IGamerRepository _gamerRepository;
            private readonly IMediator _mediator;

            public DeleteGamerCommandHandler(IGamerRepository gamerRepository, IMediator mediator)
            {
                _gamerRepository = gamerRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteGamerCommand request, CancellationToken cancellationToken)
            {
                var gamerToDelete = _gamerRepository.Get(p => p.Id == request.Id);

                _gamerRepository.Delete(gamerToDelete);
                await _gamerRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

