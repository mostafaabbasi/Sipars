using PrivateTraining.DomainClasses.EntitiesView;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.BusDriving
{
    [Table("MaximumLeaves", Schema = "BusDriving")]
    public class MaximumLeave : BaseEntity
    {
        /// <summary>
        /// سقف تعداد مرخصی
        /// </summary>
        [Required]
        public int Count { get; set; }

        [MaxLength(10)]
        public string FromDate { get; set; }
        [MaxLength(10)]
        public string ToDate { get; set; }

        //public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
      
    }
}
