using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.LocationLayer.Interface.PrivateTraining
{
    public interface ILocation
    {
        Task AddLocation(Location Locations);
        Task<IQueryable<Location>> GetAllLocation();
        IQueryable<Location> GetAllLocations();

        Task<Location> GetLocation(int id);
        Task<int> DeleteLocation(int LocationId);
    }
}
