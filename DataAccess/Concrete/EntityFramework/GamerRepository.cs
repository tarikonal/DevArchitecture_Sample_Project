
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class GamerRepository : EfEntityRepositoryBase<Gamer, ProjectDbContext>, IGamerRepository
    {
        public GamerRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
