using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class PointsOfInterestsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int NumberOfPointsOfInterests {
            get
            {
                return PointsOfInterests.Count;
            }               
        }

        public ICollection<PointsOfInterestsDto> PointsOfInterests { get; set; }
        = new List<PointsOfInterestsDto>(); // Initialize PointsOfInterestsDto as it cannot be null.
                                            // Typically it should be done in constructor but C# auto-init property can be used instead
    }
}
