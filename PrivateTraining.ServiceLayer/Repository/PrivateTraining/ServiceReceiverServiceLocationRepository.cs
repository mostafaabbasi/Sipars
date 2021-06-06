using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DataLayer.Context;
using System.Data.Entity;

namespace PrivateTraining.ServiceLayer.Repository.PrivateTraining
{
    public class ServiceReceiverServiceLocationRepository : IServiceReceiverServiceLocation
    {

        private readonly IUnitOfWork _uow;
        readonly IDbSet<ServiceReceiverServiceLocation> _service;
        readonly IDbSet<ServiceReceiverRequest> _serviceRequest;

        public ServiceReceiverServiceLocationRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _service = _uow.Set<ServiceReceiverServiceLocation>();
            _serviceRequest = _uow.Set<ServiceReceiverRequest>();

        }
        public async Task AddServiceReceiverServiceLocation(ServiceReceiverServiceLocation Locations)
        {
            _service.Add(Locations);
        }
     
        public async Task AddServiceReceiverRequest(ServiceReceiverRequest param)
        {
            _serviceRequest.Add(param);
        }


        //public async Task<int> UpdateServiceReceiverServiceLocationRequest(int Id,int Status)
        //{
        //    int savestatus = 0;          
        //    var RequestTemp = _servicerequest.Where(c => c.Id == Id).FirstOrDefault();
        //    if (RequestTemp != null)
        //    {
        //        RequestTemp.StatusRequest = Status;
        //        RequestTemp.Status = Status;
        //        savestatus = await _uow.SaveAllChangesAsync();
        //    }
        //    return savestatus;
        //}
    }
}
