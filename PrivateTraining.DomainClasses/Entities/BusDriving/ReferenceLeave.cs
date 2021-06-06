using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.BusDriving
{
    [Table("ReferenceLeaves", Schema = "BusDriving")]
    public class ReferenceLeave : BaseEntity
    {
        /// <summary>
        /// آی دی مدیر ارجاع داده شده
        /// </summary>
        [Required]
        public int ReceiverId { get; set; }

        /// <summary>
        ///آی دی مرخصی ارجاع داده  شده 
        /// </summary>
        [Required]
        public int LeaveRequestId { get; set; }

        /// <summary>
        /// آی دی مدیر ارجاع دهنده
        /// </summary>
        [Required]
        public int SenderId { get; set; }

        /// <summary>
        /// تاریخ ارجاع
        /// </summary>
        [Required]
        public string DateReference { get; set; }

        /// <summary>
        /// تاریخ ارجاع
        /// </summary>
        [Required]
        public string TimeReference { get; set; }

        /// <summary>
        /// وضعیت مرخصی
        /// </summary>
        [Required]
        public byte Status { get; set; }



        #region Navigators

        
            [ForeignKey("LeaveRequestId")]
        [InverseProperty("ReferenceLeaves")]
        public virtual LeaveRequest LeaveRequests { get; set; }

        [ForeignKey("ReceiverId")]
        [InverseProperty("ReferenceLeaves")]
        public virtual ApplicationUser ReceiverUser { get; set; }

        [ForeignKey("SenderId")]
       // [InverseProperty("ReferenceLeaves")]
        public virtual ApplicationUser SenderUser { get; set; }

        #endregion
    }
}
