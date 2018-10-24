using CityInfo.API.Entities;
using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public static class CityInfoContextExtensions
    {
        //This extension method is created because EF7 was having an issue regarding seeding data. 
        // This class will be used instead to be called when configuring the HTTP request pipeline in the starup class
        // NB: This issue is resolved by now
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            // Checks if the DB contains our sample data 
            if (context.Cities.Any())
            {
                return;
            }

            // Init Seed data
            var cities = new List<City>()
            {
                new City()
                {
                    
                    Name = "New York City",
                    Description = "The one with that big park.",
                    PointsOfInterest = new List<PointsOfInterest>()
                    {
                        new PointsOfInterest()
                        {
                            
                            Name = "Central Park",
                            Description = "The most visited urban park in the United States."
                        },
                        new PointsOfInterest()
                        {
                           
                            Name = "Empire State Building",
                            Description = "A 102-story skyscraper located in Midtown Manhattan."
                        }
                    }
                },
                new City()
                {
                    
                    Name = "Antwerp",
                    Description = "The one with cathedral that was never really finished."
                },
                new City()
                {
                    
                    Name = "Paris",
                    Description = "The one with that big tower.",
                    PointsOfInterest = new List<PointsOfInterest>
                    {
                        new PointsOfInterest()
                        {
                            Name = "Eiffel Tower",
                            Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gust"
                        },
                        new PointsOfInterest()
                        {
                            Name = "The Louver",
                            Description = "The world's largest museum."
                        },
                    }
                }
            };

            //Add these to the context
            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
