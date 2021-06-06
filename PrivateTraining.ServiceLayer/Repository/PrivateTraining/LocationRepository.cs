using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.LocationLayer.Interface.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;

namespace PrivateTraining.LocationLayer.Repository.PrivateTraining
{
     public class LocationRepository : ILocation
    {
        private readonly IUnitOfWork _uow;
        readonly IDbSet<Location> _Location;
        public LocationRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _Location = _uow.Set<Location>();

        }

        public async Task AddLocation(Location Locations)
        {
            _Location.Add(Locations);

        }

        public async Task<IQueryable<Location>> GetAllLocation()
        {
            return _Location.Include("Cities").Where(c=>c.IsEnable==true);
        }

        public  IQueryable<Location> GetAllLocations()
        {
            return _Location.Include("Cities").Where(c => c.IsEnable == true);
        }

        public async Task<Location> GetLocation(int id)
        {
            return _Location.Where(c => c.Id == id).FirstOrDefault();
        }

        public async Task<int> DeleteLocation(int LocationId)
        {
            Location Location = _Location.Find(LocationId);
            _Location.Remove(Location);
            return await _uow.SaveAllChangesAsync();

        }
    }
}
