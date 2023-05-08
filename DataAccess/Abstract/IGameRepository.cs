
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataAccess;
using Core.Entities.Dtos;
using Entities.Concrete;
namespace DataAccess.Abstract
{
    public interface IGameRepository : IEntityRepository<Game>
    {
        Task<List<GameDto>> GetGameDto();
    }
}