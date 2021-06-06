using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Interface.PrivateTraining
{
    public interface IServiceReceiverServiceLocation
    {
        Task AddServiceReceiverServiceLocation(ServiceReceiverServiceLocation Locations);
       // Task AddServiceReceiverServiceLocationTime(ServiceReceiverServiceLocationTime param);
        Task AddServiceReceiverRequest(ServiceReceiverRequest param);



        //Task<int> UpdateServiceReceiverServiceLocationRequest(int Id, int Status);
    }
}
