
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context; 
        }
        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(x => x.Name).ToList();
        }

        public City GetCity(int cityId, bool includePOI)
        {
            if (includePOI)
            {
                return _context.Cities.Include(x => x.PointsOfInterest)
                    .Where(x => x.Id == cityId).FirstOrDefault();
            }
            return _context.Cities.FirstOrDefault(x => x.Id == cityId);
        }

        public PointsOfInterest GetPointsOfInterestForCity(int cityId, int pointOfInterest)
        {
            //return _context.Cities.Where(a => a.Id == cityId).FirstOrDefault()
            //    .PointsOfInterest.Where(x => x.Id == pointOfInterest).FirstOrDefault();

            return _context.PointsOfInterest
                .Where(a => a.CityId == cityId && a.Id == pointOfInterest).FirstOrDefault();
        }

        public IEnumerable<PointsOfInterest> GetPointsOfInterests(int cityId)
        {
            return _context.PointsOfInterest
                .Where(a => a.CityId == cityId).ToList();
        }
    }
}
