using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class PointOfInterestForUpdateDto
    {
        [Required(ErrorMessage = "You should provide a name value")] // This makes sure that this field can not be null
        [MaxLength(50)] // Max allowed number of characters are 50
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}
