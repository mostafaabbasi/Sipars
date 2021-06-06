using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.DomainClasses.Entities.BusDriving;

namespace PrivateTraining.DomainClasses.EntitiesView
{
    public class BusDriving_View_LeaveRequest
    {
        public string DayName { get; set; }
        public string DayDate { get; set; }
        public int MaximumLeave { get; set; }
        public int Count { get; set; }
        public byte StatusLeave { get; set; }
        public string StatusColorLeave { get; set; }
        public string Message { get; set; }
        public int RequestId { get; set; }
        public bool Registered { get; set; }
        public int RemainingLeave { get; set; }



        public List<MaximumLeaveLine> MaximumLeaveLines { get; set; }
        public List<InvalidDayLine> InvalidDayLines { get; set; }

    }
}
