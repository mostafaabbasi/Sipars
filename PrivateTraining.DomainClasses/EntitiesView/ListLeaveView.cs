using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.EntitiesView
{
    public class ListLeaveView
    {
        public int LeverRequestId { get; set; }

        public string UsersName_Family { get; set; }

        public string LinesName { get; set; }

        public string ShiftsName { get; set; }

        public string DayName { get; set; }

        public string DayLeave { get; set; }

        public string DayRequest { get; set; }

        public string TimeRequest { get; set; }

        public byte StatusLeave { get; set; }

        public string DateChangeStatus { get; set; }

        public int BusId { get; set; }

        public string UserRequestsName_Family { get; set; }

        public string UsersRemainingleave { get; set; }


    }
}
