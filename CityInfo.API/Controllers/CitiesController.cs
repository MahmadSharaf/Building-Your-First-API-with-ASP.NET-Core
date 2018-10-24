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

        [HttpGet("{id}")] //The routing template. This can connects a request with that uri with this block
        public IActionResult GetCity(int id)
        {   //This method fetches the related data to the Id number that is requested 

            var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            // This creates a variable of City Id requested by using lambda expression
            if (cityToReturn == null)
            {// If no data related to the requested city Id number
                return NotFound(); // 404 status code returns
            }
            return Ok(cityToReturn);
        }

        public IActionResult GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities);


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
