using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Entities
{
    public class CityInfoContext : DbContext
    { 
        public CityInfoContext(DbContextOptions<CityInfoContext> options)
            :base(options)
        {
            Database.Migrate(); // This make sure of none existing DB to initial version to upcoming versions after that. So this is the best approach.
            //Database.EnsureCreated(); // This call insures the DB created effectively
        }
    
        public DbSet<City> Cities { get; set; }
        public DbSet<PointsOfInterest> PointsOfInterest { get; set; }
    }
}
