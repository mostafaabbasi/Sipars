using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.ServiceLocationLayer.Interface.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
namespace PrivateTraining.ServiceLayer.Repository.PrivateTraining
{
    public class ServiceLocationRepository: IServiceLocation
    {

        private readonly IUnitOfWork _uow;
        readonly IDbSet<View_ServiceLocations> _ServiceLocation;
        public ServiceLocationRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _ServiceLocation = _uow.Set<View_ServiceLocations>();
        }

        public async Task AddServiceLocation(View_ServiceLocations ServiceLocations)
        {
            _ServiceLocation.Add(ServiceLocations);

        }

        public async Task<IQueryable<View_ServiceLocations>> GetAllServiceLocation()
        {
            //  return _ServiceLocation.Include("Services").Include("Locations");
            return _ServiceLocation.Include("Services").Include("Locations").Where(c=>c.Locations.IsEnable==true);

        }
        
         public async Task<IQueryable<View_ServiceLocations>> GetAllServiceLocationWirhServiceId(int ServiceId=0)
        {
            //  return _ServiceLocation.Include("Services").Include("Locations");
            return _ServiceLocation.Include("Services").Include("Locations").Where(c => c.Locations.IsEnable == true  && c.ServiceId== ServiceId);

        }

        public View_ServiceLocations GetAllServiceLocationWirhServiceIds(int ServiceId = 0,int LocationId=0)
        {
            return _ServiceLocation.Include("Services").Include("Locations").Where(c => c.Locations.IsEnable == true && c.ServiceId == ServiceId && c.LocationId== LocationId).FirstOrDefault();
        }

        public IList<View_ServiceLocations> GetAllServiceLocationWithLocationId(int LocationId=0)
        {
            return _ServiceLocation.Include("Services").Include("Locations").Where(c => c.Locations.IsEnable == true && c.LocationId==LocationId).ToList();
        }

        public async Task<View_ServiceLocations> GetServiceLocation(int id)
        {
            return _ServiceLocation.Where(c => c.Id == id).FirstOrDefault();
        }

        public async Task<int> DeleteServiceLocation(int ServiceLocationId)
        {
            View_ServiceLocations ServiceLocation = _ServiceLocation.Find(ServiceLocationId);
            _ServiceLocation.Remove(ServiceLocation);
            return await _uow.SaveAllChangesAsync();

        }
    }
}
