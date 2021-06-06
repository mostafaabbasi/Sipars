using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Repository.PrivateTraining
{
   public class ServiceLevelRepository : IServiceLevel
    {

        private readonly IUnitOfWork _uow;
        readonly IDbSet<ServiceLevel> _ServiceLevel;
        public ServiceLevelRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _ServiceLevel = _uow.Set<ServiceLevel>();

        }

        public async Task AddServiceLevel(ServiceLevel ServiceLevels)
        {
            _ServiceLevel.Add(ServiceLevels);

        }

        public async Task<IQueryable<ServiceLevel>> GetAllServiceLevel()
        {
            return _ServiceLevel;
        }

        public IQueryable<ServiceLevel> GetAllServiceLevel2()
        {
            return _ServiceLevel;
        }

        public async Task<ServiceLevel> GetServiceLevel(int id)
        {
            return _ServiceLevel.Where(c => c.Id == id).FirstOrDefault();
        }

        public async Task<int> DeleteServiceLevel(int ServiceLevelId)
        {
            ServiceLevel ServiceLevel = _ServiceLevel.Find(ServiceLevelId);
            _ServiceLevel.Remove(ServiceLevel);
            return await _uow.SaveAllChangesAsync();

        }
    }
}
