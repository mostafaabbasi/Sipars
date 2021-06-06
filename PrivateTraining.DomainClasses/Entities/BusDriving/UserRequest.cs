using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.BusDriving
{
    [Table("UserRequests", Schema = "BusDriving")]
    public class UserRequest:BaseEntity
    {
        [Display(Name = "موضوع درخواست ")]
        [Required]
        public int SubjectId { get; set; }

        [Display(Name = "متن  درخواست ")]
        [Required]
        public string Content { get; set; }

        [Display(Name = "کاربر")]
        [Required]
        public int UserId { get; set; }


        [Display(Name = "شماره پیگیری")]
        [MaxLength(20)]
        public string NumTracking { get; set; }

        [Display(Name = "وضعیت درخواست")]
        [Required]
        public byte StatusRequest { get; set; }

        #region Navigators
        [ForeignKey("SubjectId")]
        [InverseProperty("UserRequests")]
        public virtual SubjectRequest SubjectRequests { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("UserRequests")]
        public virtual ApplicationUser Users { get; set; }


        #endregion Navigators
    }
}
