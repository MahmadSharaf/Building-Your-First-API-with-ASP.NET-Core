using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc; //Controller
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")] // This makes all resources consumed by starting with API, and all resources in this controller start with /cities 
    public class CitiesController : Controller
    {//This class is a Cities controller class responsible for routing URLs with the corresponding codes

        private ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }




        [HttpGet("{cityId}")] //The routing template. This can connects a request with that uri with this block
        public IActionResult GetCity(int cityId, bool includePOI=false)
        {   //This method fetches the related data to the Id number that is requested 

            var cityToReturn = _cityInfoRepository.GetCity(cityId, includePOI);

            if (cityToReturn == null)
            {// If no data related to the requested city Id number

                return NotFound(); // 404 status code returns
            }

            if (includePOI)
            {
                var cityResult = Mapper.Map<CityDto>(cityToReturn);

                return Ok(cityResult);
            }

            else
            {
                var cityResult = Mapper.Map<CityWithoutPOIDto>(cityToReturn);

                return Ok(cityResult);
            }



            //todo manual mapping            

            //x if (includePOI)
            //x {
            //x     var cityResult = new CityDto()
            //x     {
            //x         Id = cityToReturn.Id,
            //x         Name = cityToReturn.Name,
            //x         Description = cityToReturn.Description
            //x     };
            //x 
            //x     foreach (var item in cityToReturn.PointsOfInterest)
            //x     {
            //x         cityResult.PointsOfInterest.Add(new PointsOfInterestsDto()
            //x         {
            //x             Id = item.Id,
            //x             Name = item.Name,
            //x             Description = item.Description
            //x         });
            //x     }
            //x 
            //x     return Ok(cityResult);
            //x }
            //x else
            //x {
            //x     var cityResult = new CityWithoutPOIDto()
            //x     {
            //x         Id = cityToReturn.Id,
            //x         Name = cityToReturn.Name,
            //x         Description = cityToReturn.Description
            //x     };
            //x 
            //x     return Ok(cityResult);
            //x }




            //x var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            //x // This creates a variable of City Id requested by using lambda expression
            //x if (cityToReturn == null)
            //x {
            //x     // If no data related to the requested city Id number
            //x     return NotFound(); // 404 status code returns
            //x }
            //x return Ok(cityToReturn);
        }



        public IActionResult GetCities()
        {
            var cityEntities = _cityInfoRepository.GetCities();
            //! cityEntities are not the action in the API should return. So mapping should occur.

            var results = Mapper.Map<IEnumerable<CityWithoutPOIDto>>(cityEntities);

            return Ok(results);








            //todo Manual mapping
            //x var results = new List<CityWithoutPOIDto>();
                
            //x foreach (var cityEntity in cityEntities)
            //x {
            //x     results.Add(new CityWithoutPOIDto
            //x     {
            //x         Id = cityEntity.Id,
            //x         Description = cityEntity.Description,
            //x         Name = cityEntity.Name
            //x     });
            //x }
            //x return Ok(results);

            //x return Ok(CitiesDataStore.Current.Cities);


            //todo ************* unneeded code ***************//
            //x List<object> l = new List<object>
            // This creates a list of objects object is a reference data type. It is an alias for System.Object class
            // The object types can be assigned values of any other types, value types, reference types, 
            //  predefined or user-defined types. However, before assigning values, it needs type conversion.
            //x {
            //x     new { id=1, Name="New York City" }, // List can be initialized while defining in this way instead of     
            //x     new { id=2, Name="Antwerp"}         // using list.Add(...)    
            //x };
            //x 
            //x JsonResult result = new JsonResult(l);
            //x return result;
            //x 
            //x  // The above block can be summarized as follows
            //x  *
            //x return new JsonResult(new List<object> //This returns a JSONified version of whatever is passed into the constructor
            //x     {
            //x         new { id=1, Name="New York City" },
            //x         new { id=2, Name="Antwerp"}
            //x     });
            //todo ************* unneeded code ***************//
        }
    }
}
