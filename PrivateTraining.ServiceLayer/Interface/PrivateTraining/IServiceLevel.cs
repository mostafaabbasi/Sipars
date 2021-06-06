using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Interface.PrivateTraining
{
    public interface IServiceLevel
    {
        Task AddServiceLevel(ServiceLevel ServiceLevels);
        Task<IQueryable<ServiceLevel>> GetAllServiceLevel();
        IQueryable<ServiceLevel> GetAllServiceLevel2();
        Task<ServiceLevel> GetServiceLevel(int id);
        Task<int> DeleteServiceLevel(int ServiceLevelId);
    }
}
