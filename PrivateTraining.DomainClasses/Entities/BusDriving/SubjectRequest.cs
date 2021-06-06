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
    [Table("SubjectRequests", Schema = "BusDriving")]
    public class SubjectRequest:BaseEntity
    {
        [Display(Name = "موضوع درخواست ")]
        [Required]
        public string Subject { get; set; }


        //[Display(Name = "مدیر ")]
        ////[Required]
        //public int UserId { get; set; }

        #region Navigators

        //[ForeignKey("UserId")]
        //[InverseProperty("SubjectRequests")]
        //public virtual ApplicationUser Users{ get; set; }


        public virtual ICollection<UserRequest> UserRequests { get; set; }
        #endregion

    }
}
