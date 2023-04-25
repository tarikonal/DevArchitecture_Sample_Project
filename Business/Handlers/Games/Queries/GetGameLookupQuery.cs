using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Dtos;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Games.Queries
{
    public class GetGameLookupQuery : IRequest<IDataResult<IEnumerable<SelectionItem>>>
    {
        public class
            GetUserLookupQueryHandler : IRequestHandler<GetGameLookupQuery, 
                IDataResult<IEnumerable<SelectionItem>>>
        {
            private readonly IGameRepository _gameRepository;

            //public GetGameLookupQueryHandler(IGameRepository gameRepository)
            //{
            //    _gameRepository = gameRepository;
            //}

            [SecuredOperation(Priority = 1)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<IEnumerable<SelectionItem>>> Handle(GetGameLookupQuery request, CancellationToken cancellationToken)
            {
                var list = await _gameRepository.GetListAsync(/*x => x.Status*/);
                var userLookup = list.Select(x => new SelectionItem() { Id = x.Id.ToString(), Label = x.Name });
                return new SuccessDataResult<IEnumerable<SelectionItem>>(userLookup);
            }
        }
    }
}