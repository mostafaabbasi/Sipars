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
    public class ServiceWaitingList : IServiceWaitingList
    {
        private readonly IUnitOfWork _uow;
        readonly IDbSet<ServiceWaitingList> _Service;

        public ServiceWaitingList(IUnitOfWork uow)
        {
            _uow = uow;
            _Service = _uow.Set<ServiceWaitingList>();
        }

        public Task AddProviderToWaitListService(int ServiceId, List<int> ProviderId)
        {
            throw new NotImplementedException();
        }

        public Task DeactiveProviderFromWaitListService(int ServiceId, List<int> ProviderId)
        {
            throw new NotImplementedException();
        }

        public Task<List<DomainClasses.Entities.PrivateTraining.ServiceWaitingList>> GetWaitListService(int ServiceId)
        {
            throw new NotImplementedException();
        }

        public Task ProviderAcceptRequest(int ServiceId, int ProviderId)
        {
            throw new NotImplementedException();
        }

        public Task ProviderPendRequest(int ServiceId, int ProviderId)
        {
            throw new NotImplementedException();
        }
    }
}
