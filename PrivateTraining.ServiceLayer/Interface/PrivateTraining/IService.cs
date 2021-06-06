using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Interface.PrivateTraining
{
    public interface IService
    {
        Task AddService(Service Services);
        Task<IQueryable<Service>> GetAllService();
        Task<Service> GetService(int id);
        Task<int> DeleteService(int ServiceId);

        IQueryable<Service> GetAllServiceParent();

    }
}
