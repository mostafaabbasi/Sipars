using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.BusDriving
{
    [Table("PeriodLeaves", Schema = "BusDriving")]
    public class PeriodLeave:BaseEntity
    {
        public int StartDay { get; set; }
        public int CountLeaveDay { get; set; }
    }
}
