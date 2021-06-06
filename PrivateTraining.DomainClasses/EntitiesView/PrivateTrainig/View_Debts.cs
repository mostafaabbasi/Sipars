using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig
{
    public class View_Debts
    {
        //a
        public int Id { get; set; }

        public double TotalCost { get; set; }

        public int PercentOfShares { get; set; }

        public double CompanyCost { get; set; }

        public double TotalCostReceived { get; set; }

        //- در حال بررسی - تایید شده - رد شده
        public byte Status { get; set; }

        public string ReasonDebt { get; set; }

        public string ServiceLocationName { get; set; }

        public string ProviderFullName { get; set; }
        public string RecevierFullName { get; set; }

        public byte StatusServiceReceiverServiceLocation { get; set; }
        public int ServiceId { get; set; }

    }
}
