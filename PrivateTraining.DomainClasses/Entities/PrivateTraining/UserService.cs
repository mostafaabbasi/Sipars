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
    [Table("UserServices", Schema = "PrivateTraining")]
    public class UserService : BaseEntity
    {
        public UserService()
        {
            ActiveServiceForUser = 0;
            ScoreByAdmin = 0;
            CalcScoreByServiceReciverAndSystem = 0;
            CountSTarScoreServiceUser = 0;
            CapacityServiceUser = 0;
            CountScoreByServiceRecivers = 0;
            ServiceLevelListId = 0;
        }

        #region Properties
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int CalcScoreByServiceReciverAndSystem { get; set; }

        [Required]
        public int CountScoreByServiceRecivers { get; set; }

        [Required]
        [Range(0, 100)]
        public int ScoreByAdmin { get; set; }

        [Range(0, 5)]
        public double CountSTarScoreServiceUser { get; set; }

        [Required]
        public int CapacityServiceUser { get; set; }

        //public Nullable<int> UpdateScoreByUserId { get; set; }

        //public string DateUpdateScore { get; set; }

        //public string TimeUpdateScore { get; set; }

        // در حال بررسی =0 و فعال =1 وغیرفعال =2
        public byte ActiveServiceForUser { get; set; }

        [Display(Name = "شرایط ویژه انجام خدمت ")]
        public string SpecialConditionsOfWork { get; set; }

        [Display(Name = " سطح خدمت")]
        public int ServiceLevelListId { get; set; }


        #endregion

        #region Navigators
        [ForeignKey("UserId")]
        [InverseProperty("UserServices")]
        public virtual ApplicationUser Users { get; set; }
      
        [ForeignKey("ServiceId")]
        [InverseProperty("UserServices")]
        public virtual ServiceProperties ServiceProperties { get; set; }

        #endregion

    }
}
