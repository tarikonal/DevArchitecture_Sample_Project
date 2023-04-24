using Core.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Campaign : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int DiscountRate { get; set; }

        [ForeignKey("Game")]
        public int GameId { get; set; }
    }
}
