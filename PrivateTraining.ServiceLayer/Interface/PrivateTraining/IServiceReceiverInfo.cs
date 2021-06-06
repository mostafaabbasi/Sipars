using PrivateTraining.DomainClasses.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Interface.PrivateTraining
{ 
    public interface IServiceReceiverInfo
    {
        Task AddServiceReceiverInfo(ServiceReceiverInfo ServiceReceiverInfos);
        Task<IQueryable<ServiceReceiverInfo>> GetAllServiceReceiverInfo();
        Task<ServiceReceiverInfo> GetServiceReceiverInfo(int id);
        Task<int> DeleteServiceReceiverInfo(int ServiceReceiverInfoId);
    }
}
