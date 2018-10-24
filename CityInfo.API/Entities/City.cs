using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Entities
{
    public class City
    {
        [Key] // Makes Id the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Makes Id property values, generated automatically on add.
        public int Id { get; set; }                           // It has three possible values:
                                                              // 1-None: for no generation
                                                              // 2-Identity: for generation on add
                                                              // 3-Computed: for generation on add or update
        

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public ICollection<PointsOfInterest> PointsOfInterest { get; set; }
        = new List<PointsOfInterest>();
    }
}
