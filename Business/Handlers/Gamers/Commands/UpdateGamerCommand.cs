
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
using Business.Handlers.Gamers.ValidationRules;


namespace Business.Handlers.Gamers.Commands
{


    public class UpdateGamerCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }
        public long IdentityNumber { get; set; }

        public class UpdateGamerCommandHandler : IRequestHandler<UpdateGamerCommand, IResult>
        {
            private readonly IGamerRepository _gamerRepository;
            private readonly IMediator _mediator;

            public UpdateGamerCommandHandler(IGamerRepository gamerRepository, IMediator mediator)
            {
                _gamerRepository = gamerRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateGamerValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateGamerCommand request, CancellationToken cancellationToken)
            {
                var isThereGamerRecord = await _gamerRepository.GetAsync(u => u.Id == request.Id);


                isThereGamerRecord.FirstName = request.FirstName;
                isThereGamerRecord.LastName = request.LastName;
                isThereGamerRecord.BirthYear = request.BirthYear;
                isThereGamerRecord.IdentityNumber = request.IdentityNumber;


                _gamerRepository.Update(isThereGamerRecord);
                await _gamerRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

