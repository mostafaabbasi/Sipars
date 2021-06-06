using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Interface.PrivateTraining
{
    public interface IServiceProviderInfo
    {
        Task AddServiceProviderInfo(ServiceProviderInfo ServiceProviderInfos);
        Task<IQueryable<ServiceProviderInfo>> GetAllServiceProviderInfo();
        IQueryable<ServiceProviderInfo> GetAllServiceProviderInfoById(int UserId = 0);
        Task<ServiceProviderInfo> GetServiceProviderInfo(int id);
        List<SP_ListServiceProviderBySL> ListServiceLocation(int ServiceId, int LocationId);
        Task<int> DeleteServiceProviderInfo(int ServiceProviderInfoId);
        ServiceProviderInfo ServiceProviderInfo(int id);

    }
}
