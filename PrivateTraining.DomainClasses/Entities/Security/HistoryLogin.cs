using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Security
{
    [Table("HistoryLogins", Schema = "Security")]

    public class HistoryLogin : BaseEntity
    {
        [Display(Name = "تاریخ لاگین")]
        [MaxLength(10)]
        public string LoginDate { get; set; }

        [Display(Name = "ساعت لاگین")]
        [MaxLength(10)]
        public string LoginTime { get; set; }

        [Display(Name = "کد کاربر")]
        public int UserId { get; set; }

    }
}
