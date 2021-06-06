using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Interface.PrivateTraining
{
    public interface IServiceProperties
    {
        Task AddServiceProperties(ServiceProperties ServiceProperties);
        Task<IQueryable<ServiceProperties>> GetAllServiceProperties();
        Task<ServiceProperties> GetServiceProperties(int id);
        IEnumerable<ServiceProperties> GetAllServiceProperties2();
        Task<int> DeleteServiceProperties(int ServicePropertiesId);
    }

}
