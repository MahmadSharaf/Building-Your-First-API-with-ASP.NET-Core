using Microsoft.AspNetCore.Mvc; //Controller
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")] // This makes all resources consumed by starting with API. and allresources in this controller start with /cities 
    public class CitiesController : Controller
    {
        [HttpGet("{id}")] //The routing template. This can connects a request with that url with this block
        public JsonResult GetCity(int id)
        {
            return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id));
        }
        public JsonResult GetCities()//This returns a JSONified version of whaterever is passed into the constructor
        {
            return new JsonResult(CitiesDataStore.Current.Cities);
                                                  

            /*
            List<object> l = new List<object>
            // This creates a list of objects object is a reference data type. It is an alias for System.Object class
              // The object types can be assigned values of any other types, value types, reference types, 
              //  predefined or user-defined types. However, before assigning values, it needs type conversion.
            {
                new { id=1, Name="New York City" }, // List can be initialized while defining in this way instead of     
                new { id=2, Name="Antwerp"}         // using list.Add(...)    
            };

            JsonResult result = new JsonResult(l);
            return result;

             // The above block can be summarized as follows
             *
            return new JsonResult(new List<object>
                {
                    new { id=1, Name="New York City" },
                    new { id=2, Name="Antwerp"}
                });*/   
        }
    }
}
