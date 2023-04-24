
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class GameRepository : EfEntityRepositoryBase<Game, ProjectDbContext>, IGameRepository
    {
        public GameRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
