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
    public class ServiceRepository:IService
    {
        private readonly IUnitOfWork _uow;
        readonly IDbSet<Service> _Service;
        public ServiceRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _Service = _uow.Set<Service>();

        }

        public async Task AddService(Service Services)
        {
            _Service.Add(Services);

        }

        public async Task<IQueryable<Service>> GetAllService()
        {
            return _Service;
        }

        public async Task<Service> GetService(int id)
        {
            return _Service.Where(c => c.Id == id).FirstOrDefault();
        }

        public async Task<int> DeleteService(int ServiceId)
        {
            Service Service = _Service.Find(ServiceId);
            _Service.Remove(Service);
            return await _uow.SaveAllChangesAsync();

        }

        public IQueryable<Service> GetAllServiceParent()
        {
            return _Service.Where(c=>c.ParentId==0);
        }



    }


  
}
