using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Repository.PrivateTraining
{
    public class ServiceReceiverInfoRepository: IServiceReceiverInfo
    {
        private readonly IUnitOfWork _uow;
        readonly IDbSet<ServiceReceiverInfo> _ServiceReceiverInfo;
        public ServiceReceiverInfoRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _ServiceReceiverInfo = _uow.Set<ServiceReceiverInfo>();

        }

        public async Task AddServiceReceiverInfo(ServiceReceiverInfo ServiceReceiverInfos)
        {
            _ServiceReceiverInfo.Add(ServiceReceiverInfos);

        }

        public async Task<IQueryable<ServiceReceiverInfo>> GetAllServiceReceiverInfo()
        {
            return _ServiceReceiverInfo;
        }

        public async Task<ServiceReceiverInfo> GetServiceReceiverInfo(int id)
        {
            return _ServiceReceiverInfo.Where(c => c.Id == id).FirstOrDefault();
        }

        public async Task<int> DeleteServiceReceiverInfo(int ServiceReceiverInfoId)
        {
            ServiceReceiverInfo ServiceReceiverInfo = _ServiceReceiverInfo.Find(ServiceReceiverInfoId);
            _ServiceReceiverInfo.Remove(ServiceReceiverInfo);
            return await _uow.SaveAllChangesAsync();

        }
    }
}
