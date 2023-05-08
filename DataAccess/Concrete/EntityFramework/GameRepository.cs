
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
using Core.Entities.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class GameRepository : EfEntityRepositoryBase<Game, ProjectDbContext>, IGameRepository
    {
        public GameRepository(ProjectDbContext context) : base(context)
        {
            
        }
        public async Task<List<GameDto>> GetGameDto()
        {
            var list = await (from game in Context.Games
                              //in trs in Context.Translates on lng.Id equals trs.LangId
                              select new GameDto()
                              {
                                  Id = game.Id,
                                  Name = game.Name,
                                  Description = game.Description,
                                  Price = game.Price
                              }).ToListAsync();

            return list;
        }
    }
}
