
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class CampaignRepository : EfEntityRepositoryBase<Campaign, ProjectDbContext>, ICampaignRepository
    {
        public CampaignRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
