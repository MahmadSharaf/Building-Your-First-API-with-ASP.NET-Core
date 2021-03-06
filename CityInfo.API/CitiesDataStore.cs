﻿using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class CitiesDataStore
    {   
        public static CitiesDataStore Current { get; } = new CitiesDataStore(); //This makes sure we work on the same data as long as we don't restart  
        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id=1,
                    Name = "New York City",
                    Description = "The one with that big park.",
                    PointsOfInterest = new List<PointsOfInterestsDto>()
                    {
                        new PointsOfInterestsDto()
                        {
                            Id = 1,
                            Name = "Central Park",
                            Description = "The most visited urban park in the United States."
                        },
                        new PointsOfInterestsDto()
                        {
                            Id = 2,
                            Name = "Empire State Building",
                            Description = "A 102-story skyscraper located in Midtown Manhattan."
                        }
                    }
                },
                new CityDto()
                {
                    Id=2,
                    Name = "Antwerp",
                    Description = "The one with cathedral that was never really finished."
                },
                new CityDto()
                {
                    Id=3,
                    Name = "Paris",
                    Description = "The one with that big tower."
                }
            };
        }
    }
}
