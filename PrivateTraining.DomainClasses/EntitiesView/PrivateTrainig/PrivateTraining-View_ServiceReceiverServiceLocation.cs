using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig
{
    public class PrivateTraining_View_ServiceReceiverServiceLocation
    {
        public decimal Id { get; set; }

        public int ServiceId { get; set; }
        public int LocationId { get; set; }
        public string ServiceName { get; set; }
        public string LocationName { get; set; }
        public string ServiceProviderFullName { get; set; }
        public string ServiceReceiverFullName { get; set; }
        public string WorkUnitName { get; set; }
        

        public int ServiceLocationId { get; set; }

        public int ServiceReceiverId { get; set; }

        public int ServiceProviderId { get; set; }

        public string DateRegister { get; set; }

        public string DateAcceptStatus { get; set; }

        public string TimeAcceptStatus { get; set; }

        public string DateCertainStatus { get; set; }

        public string TimeCertainStatus { get; set; }

        public Nullable<int> WhoChangeStatus { get; set; }

        public int WorkUnitId { get; set; }

        public int Status { get; set; }

        public int CalcPrice { get; set; }

        public int CalcPriceReceived { get; set; }

        public byte TypeProblem { get; set; }

        public Nullable<int> ReasonProblemByUserId { get; set; }

        public string ReasonProblem { get; set; }

        public string DateProblem { get; set; }

        public string TimeProblem { get; set; }

    }
}
