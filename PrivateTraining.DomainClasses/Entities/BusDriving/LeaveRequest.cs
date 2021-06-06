using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.BusDriving
{
    [Table("LeaveRequests", Schema = "BusDriving")]
    public class LeaveRequest : BaseEntity
    {
        public int UserId { get; set; }

        public int BusId { get; set; }

        public string DayRequest { get; set; }

        public string TimeRequest { get; set; }

        public string DayName { get; set; }

        public string DayLeave { get; set; }

        public byte StatusLeave { get; set; }

        public string DateChangeStatus { get; set; }

        public string TimeChangeStatus { get; set; }

        public int LineId { get; set; }

        public int ShiftId { get; set; }
    
        public int UserRequest { get; set; }

        #region Navigators

        [ForeignKey("LineId")]
        [InverseProperty("LeaveRequests")]
        public virtual Line Lines { get; set; }

        [ForeignKey("ShiftId")]
        [InverseProperty("LeaveRequests")]
        public virtual Shift Shifts { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("LeaveRequests")]
        public virtual ApplicationUser Users { get; set; }

        [ForeignKey("UserRequest")]
        public virtual ApplicationUser UserRequests { get; set; }

        public virtual ICollection<ReferenceLeave> ReferenceLeaves { get; set; }
        //[ForeignKey("MaxId")]
        //[InverseProperty("LeaveRequests")]
        //public virtual MaximumLeave MaximumLeaves { get; set; }
        #endregion
    }
}
