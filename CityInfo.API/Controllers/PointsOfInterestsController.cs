using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{[Route("api/cities")]
    public class PointsOfInterestsController : Controller
    {//This class is a Points Of Interests controller class which is responsible for routing URLs with the corresponding codes

        //! ILoggerFactory can be requested and create a logger, but there is another approach. 
        //! The container can also directly provide an ILogger<T> instance, when this technique is used, 
        //! the logger will automatically use the type's name as its category name.
        private ILogger<PointsOfInterestsController> _logger;
        private ICityInfoRepository _cityInfoRepository;
        private IMailService _mailService; // Setting the instance

        public PointsOfInterestsController(ILogger<PointsOfInterestsController> logger,
            IMailService mailService, ICityInfoRepository cityInfoRepository)
        {
            _mailService = mailService; // inject instance in the constructor
            _logger = logger;           // Create an instance of the ILogger
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                //throw new Exception("Exception sample");

                //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

                if (!_cityInfoRepository.CityExists(cityId))
                {
                    // This logs with an information message
                    _logger.LogInformation($"City with id {cityId} couldn't be found when accessing points of interest.");
                    return NotFound();
                }

                var pointsOfInterestForCity = _cityInfoRepository.GetPointsOfInterests(cityId);

                var poiForCityResults = Mapper.Map<IEnumerable<PointsOfInterestsDto>>(pointsOfInterestForCity);

                return Ok(poiForCityResults);

                //todo Entity and DTO with manual mapping
                //x var poiForCityResults = new List<PointsOfInterestsDto>();
                //x foreach (var poi in pointsOfInterestForCity)
                //x {
                //x     poiForCityResults.Add(new PointsOfInterestsDto()
                //x     {
                //x         Id          = poi . Id          , 
                //x         Name        = poi . Name        , 
                //x         Description = poi . Description   
                //x     });
                //x }

                //x return Ok(poiForCityResults);

                //todo No Entity ,DTO directly
                //x if (city == null)
                //x {
                //x     // This logs with an information message
                //x     _logger.LogInformation($"City with id {cityId} couldn't be found when accessing points of interest.");
                //x     return NotFound();
                //x }
                //x return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "City Id is not correct.");
            }
           
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int Id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                // This logs with an information message
                _logger.LogInformation($"City with id {cityId} couldn't be found when accessing points of interest.");
                return NotFound();
            }

            var pointOfInterest = _cityInfoRepository.GetPointsOfInterestForCity(cityId, Id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestResult = Mapper.Map <PointsOfInterestsDto>(pointOfInterest);

            return Ok(pointOfInterestResult);

            //todo Entity and DTO manual mapping
            //x var pointOfInterestResult = new PointsOfInterestsDto()
            //x {
            //x     Id = pointOfInterest.Id,
            //x     Name = pointOfInterest.Name,
            //x     Description = pointOfInterest.Description
            //x };

            //x return Ok(pointOfInterestResult);

            //todo No Entity, DTO directly
            //x var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //x if (city == null)
            //x {                
            //x     return NotFound();
            //x }
            //x 
            //x var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == Id);
            //x if (pointOfInterest == null)
            //x {
            //x     return NotFound();
            //x }
            //x return Ok(pointOfInterest);
        }

        [HttpPost("{cityId}/pointsofinterest")] //updates the entity with requested 
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointOfInterestsForCreationDto pointOfInterest)//This fetches the point of interest data and then deserialize it to PointOfInterestsForCreationDto
        {
            //Check if the sent data is null
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            //x var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            
            //check if the City Id in the data sent is not null
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            //A custom model state validation check. This checks if the Name and the Description are equal.
            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "Description and name should not be the same.");
            }

            //Model State is a dictionary, it contains both the state of the model, and model-binding validation. 
            //It represents a collection of name and value pairs that were submitted to API, one for each property
            //It also contains a collection of error messages for each value submitted.
            //Whenever a request come in the rules that is applied in the DTO, are checked
            if (!ModelState.IsValid)
            {//Check if the validations are not satisfied
                return BadRequest(ModelState);
            }

            //todo unneeded anymore as the ID is now auto generated
            // mapping poi ID 
            //x var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
            //x     c => c.PointsOfInterest).Max(p => p.Id);

            var finalPointOfInterest = Mapper.Map<Entities.PointsOfInterest>(pointOfInterest);

            _cityInfoRepository.AddPointOfInterestForCity(cityId, finalPointOfInterest);
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request");
            }

            var createdPointOfInterestToReturn = Mapper.Map<PointsOfInterestsDto>(finalPointOfInterest);

            // This allows to return a response with a location header that contains the URI where the newly-created point of interest can be found.  
            return CreatedAtRoute("GetPointOfInterest", new
            { cityId = cityId, id = createdPointOfInterestToReturn.Id }, createdPointOfInterestToReturn);
        }


        // All property fields should be send with request body, as HttpPut updates all the fields data.If a property field is not send, 
        //it will be set as its default value which is most probably null
        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "Description and name should not be the same.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointsOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            //This a mapper override which overrides the data in the destination with the data in the source.
            Mapper.Map(pointOfInterest, pointOfInterestEntity);

            //var pointOfInterestFromStore = Mapper.Map<Entities.PointsOfInterest>(pointOfInterest);

            //_cityInfoRepository.UpdatePointOfInterestForCity(cityId, id, pointOfInterestFromStore);
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request");
            }

            
            return NoContent(); // This means the request proceeded successfully but there is nothing to return
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartialUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
       //JsonPatchDocument can take either PointsOfInterestDto or PointOfInterestForUpdateDto:
        //*If PointsOfInterestDto is chosen then an extra validation check that guarantees the id is not changed, is needed.
        //*If PointOfInterestForUpdateDto is chosen, it already has the validation annotations and does not have the ID
        // but the point of interest in this class has to mapped to the point of interest DTO before applying the patch.
        {
            if (patchDoc == null)
            {//The request content is not empty
                return BadRequest();
            }
                       
            
            if (!_cityInfoRepository.CityExists(cityId))
            {//The city Id is not available
                return NotFound();
            }

            var PointsOfInterestEntity = _cityInfoRepository.GetPointsOfInterestForCity(cityId, id);
            if (PointsOfInterestEntity == null)
            {//targeted Point of interest is not available 
                return NotFound();
            }

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(PointsOfInterestEntity);

            // Creating an instance of PointOfInterestForUpdateDto with the values available in the datastore which is fetched by PointsOfInterestEntity
            //x var pointOfInterestToPatch =
            //x     new PointOfInterestForUpdateDto()
            //x     {
            //x         Name = PointsOfInterestEntity.Name,
            //x         Description = PointsOfInterestEntity.Description
            //x     };

            //applying the patch document
            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {//Check for the validation for the patch document 
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Name == pointOfInterestToPatch.Description)
            {
                ModelState.AddModelError("Description", "Description and name should not be the same.");
            }

            //This triggers validations for this models. If any errors it will end in the model state
            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {//Check for the validation for the poi in PointOfInterestForUpdateDto is still valid
                return BadRequest(ModelState);
            }

            //Applying the data to the datastore property fields
            Mapper.Map(pointOfInterestToPatch, PointsOfInterestEntity);
            //x PointsOfInterestEntity.Name = pointOfInterestToPatch.Name;
            //x PointsOfInterestEntity.Description = pointOfInterestToPatch.Description;

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request");
            }

            return NoContent();
        }


        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeleteUpdatePointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {//The city Id is not available
                return NotFound();
            }

            var pointOfInterestFromStore = _cityInfoRepository.GetPointsOfInterestForCity(cityId, id);
            if (pointOfInterestFromStore == null)
            {//targeted Point of interest is not available 
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterestForCity(pointOfInterestFromStore);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request");
            }

            _mailService.Send("Point of interest deleted.",
                $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted.");

            return NoContent();
        }
    }
}
