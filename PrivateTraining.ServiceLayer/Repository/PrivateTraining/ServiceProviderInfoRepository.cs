using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig;

namespace PrivateTraining.ServiceLayer.Repository.PrivateTraining
{
    public class ServiceProviderInfoRepository : IServiceProviderInfo
    {

        private readonly IUnitOfWork _uow;
        readonly IDbSet<ServiceProviderInfo> _ServiceProviderInfo;
        public ServiceProviderInfoRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _ServiceProviderInfo = _uow.Set<ServiceProviderInfo>();

        }

        public async Task AddServiceProviderInfo(ServiceProviderInfo ServiceProviderInfos)
        {
            _ServiceProviderInfo.Add(ServiceProviderInfos);

        }

        public async Task<IQueryable<ServiceProviderInfo>> GetAllServiceProviderInfo()
        {
            return _ServiceProviderInfo;
        }

        public IQueryable<ServiceProviderInfo> GetAllServiceProviderInfoById(int UserId=0)
        {
            var d= _ServiceProviderInfo.Where(c => c.Id == UserId);
            return d;
        }


        public async Task<ServiceProviderInfo> GetServiceProviderInfo(int id)
        {
            return _ServiceProviderInfo.Where(c => c.Id == id).FirstOrDefault();
        }

        public async Task<int> DeleteServiceProviderInfo(int ServiceProviderInfoId)
        {
            ServiceProviderInfo ServiceProviderInfo = _ServiceProviderInfo.Find(ServiceProviderInfoId);
            _ServiceProviderInfo.Remove(ServiceProviderInfo);
            return await _uow.SaveAllChangesAsync();
        }

        public List<SP_ListServiceProviderBySL> ListServiceLocation(int ServiceId, int LocationId)
        {
            return _uow.GetRowsWithoutParam<SP_ListServiceProviderBySL>("exec [PrivateTraining].[SP_ListServiceProviderBySL] " + ServiceId + "," + LocationId + "").ToList();
        }

        public ServiceProviderInfo ServiceProviderInfo(int id)
        {
            return _ServiceProviderInfo.Where(c => c.Id == id).FirstOrDefault();
        }
    }
}
