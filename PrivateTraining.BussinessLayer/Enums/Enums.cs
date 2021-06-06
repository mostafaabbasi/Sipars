using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.BussinessLayer.Enums
{
    public enum StatusServiceLocationUser
    {
        PendingApproval = 0,
        Active = 1,
        DeActive = 2,
        Reservation = 3,
        SubmitInformation = 4
    }
    public enum StatusServicesRequested
    {
        New=0,
        Doing=1,
        Done=2,
    }
}
