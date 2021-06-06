using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLocationLayer.Interface.PrivateTraining
{

    public interface IServiceLocation
    {
        Task AddServiceLocation(View_ServiceLocations ServiceLocations);
        Task<IQueryable<View_ServiceLocations>> GetAllServiceLocation();
        IList<View_ServiceLocations> GetAllServiceLocationWithLocationId(int LocationId = 0);
        Task<IQueryable<View_ServiceLocations>> GetAllServiceLocationWirhServiceId(int ServiceId);
        View_ServiceLocations  GetAllServiceLocationWirhServiceIds(int ServiceId = 0, int LocationId = 0);
        Task<View_ServiceLocations> GetServiceLocation(int id);
        Task<int> DeleteServiceLocation(int ServiceLocationId);
    }
}
