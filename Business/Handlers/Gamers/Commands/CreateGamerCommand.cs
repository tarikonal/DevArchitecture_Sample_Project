
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
using Business.Handlers.Gamers.ValidationRules;

namespace Business.Handlers.Gamers.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateGamerCommand : IRequest<IResult>
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }
        public long IdentityNumber { get; set; }


        public class CreateGamerCommandHandler : IRequestHandler<CreateGamerCommand, IResult>
        {
            private readonly IGamerRepository _gamerRepository;
            private readonly IMediator _mediator;
            public CreateGamerCommandHandler(IGamerRepository gamerRepository, IMediator mediator)
            {
                _gamerRepository = gamerRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateGamerValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateGamerCommand request, CancellationToken cancellationToken)
            {
                var isThereGamerRecord = _gamerRepository.Query().Any(u => u.FirstName == request.FirstName);

                if (isThereGamerRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedGamer = new Gamer
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthYear = request.BirthYear,
                    IdentityNumber = request.IdentityNumber,

                };

                _gamerRepository.Add(addedGamer);
                await _gamerRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}