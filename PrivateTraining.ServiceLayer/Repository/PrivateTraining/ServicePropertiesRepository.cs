using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServicePropertiesLayer.Repository.PrivateTraining
{
    public class ServicePropertiesRepository: IServiceProperties
    {
        private readonly IUnitOfWork _uow;
        readonly IDbSet<ServiceProperties> _ServiceProperties;
        public ServicePropertiesRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _ServiceProperties = _uow.Set<ServiceProperties>();

        }

        public async Task AddServiceProperties(ServiceProperties ServiceProperties)
        {
            _ServiceProperties.Add(ServiceProperties);

        }

        public async Task<IQueryable<ServiceProperties>> GetAllServiceProperties()
        {
            return _ServiceProperties;
        }


        public IEnumerable<ServiceProperties> GetAllServiceProperties2()
        {
            return _ServiceProperties;
        }

        public async Task<ServiceProperties> GetServiceProperties(int id)
        {
            return _ServiceProperties.Where(c => c.Id == id).FirstOrDefault();
        }

        public async Task<int> DeleteServiceProperties(int ServicePropertiesId)
        {
            ServiceProperties ServiceProperties = _ServiceProperties.Find(ServicePropertiesId);
            _ServiceProperties.Remove(ServiceProperties);
            return await _uow.SaveAllChangesAsync();

        }
    }


}
