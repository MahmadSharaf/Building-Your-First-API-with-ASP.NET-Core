using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{   //This class will act as the contract in which th repository implementation will adhere to
    public interface ICityInfoRepository
    {
        bool CityExists(int cityId);
        IEnumerable<City> GetCities();
        City GetCity(int cityId, bool includePOI);
        IEnumerable<PointsOfInterest> GetPointsOfInterests(int cityId);
        PointsOfInterest GetPointsOfInterestForCity(int cityId, int pointOfInterest);
    }
}
