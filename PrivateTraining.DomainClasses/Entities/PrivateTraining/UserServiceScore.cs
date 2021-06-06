using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("UserServiceScores", Schema = "PrivateTraining")]

    public class UserServiceScore : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int Score { get; set; }

        // یا توسط سیستم یا کاربر موجود در سامانه
        public int ScoreByUserId { get; set; }

        [Required]
        public string DateUpdateScore { get; set; }

        [Required]
        public string TimeUpdateScore { get; set; }


        [ForeignKey("UserId")]
        [InverseProperty("UserServiceScores")]
        public virtual ApplicationUser Users { get; set; }


        [ForeignKey("ServiceId")]
        [InverseProperty("UserServiceScores")]
        public virtual ServiceProperties ServiceProperties { get; set; }
    }
}
