
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Gamers.Queries
{
    public class GetGamerQuery : IRequest<IDataResult<Gamer>>
    {
        public int Id { get; set; }

        public class GetGamerQueryHandler : IRequestHandler<GetGamerQuery, IDataResult<Gamer>>
        {
            private readonly IGamerRepository _gamerRepository;
            private readonly IMediator _mediator;

            public GetGamerQueryHandler(IGamerRepository gamerRepository, IMediator mediator)
            {
                _gamerRepository = gamerRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Gamer>> Handle(GetGamerQuery request, CancellationToken cancellationToken)
            {
                var gamer = await _gamerRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Gamer>(gamer);
            }
        }
    }
}
