using PrivateTraining.DomainClasses.Entities.Security;
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
    [Table("UserServiceLocations", Schema = "PrivateTraining")]
    public class UserServiceLocation : BaseEntity
    {
        public UserServiceLocation()
        {
            StatusServiceLocationUser = 0;
        }

        #region Properties
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ServiceLocationId { get; set; }


        [Required]
        public int LocationId { get; set; }

        //[Required]
        public int? ServiceId { get; set; }

        /// <summary>
        /// در انتظار تایید= 0     
        /// فعال=1
        /// غیر فعال=2      
        /// رزرو شده=3
        /// ثبت اطلاعات شدگان=4
        [Required]
        public byte StatusServiceLocationUser { get; set; }

        #endregion

        #region Navigators
        [ForeignKey("UserId")]
        [InverseProperty("UserServiceLocations")]
        public virtual ApplicationUser Users { get; set; }


        [ForeignKey("ServiceLocationId")]
        [InverseProperty("UserServiceLocations")]
        public virtual View_ServiceLocations ServiceLocations { get; set; }

        [ForeignKey("LocationId")]
        [InverseProperty("UserServiceLocations")]
        public virtual Location Locations { get; set; }

        [ForeignKey("ServiceId")]
        [InverseProperty("UserServiceLocations")]
        public virtual Service Services { get; set; }


        #endregion
    }
}
