using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{   //! What is returned from or accepted by an API is not the same as the entities used by the underlying datastore.
    public class CityDto
    {
        //public List<PointsOfInterestsDto> PointsOfInterest;

        public int    Id          { get ; set ; } 
        public string Name        { get ; set ; } 
        public string Description { get ; set ; } 

        public int NumberOfPointsOfInterests
        {
            get
            {
                return PointsOfInterest.Count;
            }
        }

        public ICollection<PointsOfInterestsDto> PointsOfInterest { get; set; }
        = new List<PointsOfInterestsDto>(); // Initialize PointsOfInterestsDto as to avoid null reference issues.
                                            // Typically it should be done in constructor but C# 6's auto-property initializer can be used instead
    }
}
