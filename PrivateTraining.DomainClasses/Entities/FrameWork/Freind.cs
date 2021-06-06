using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.FrameWork
{
    [Table("Freinds", Schema = "Framework")]

    public class Freind : BaseEntity
    {
        #region Property

        [Display(Name = "درخواست کننده")]
        public int UserIdApplicant { get; set; }

        [Display(Name = "پذیرنده")]
        public int UserIdAcceptor { get; set; }

        [MaxLength(20)]
        public string SaveDate { get; set; }

        #endregion

        #region navigation

        [ForeignKey("UserIdApplicant")]
        [InverseProperty("Freinds")]
        public virtual ApplicationUser ApplicantUsers { get; set; }

        [ForeignKey("UserIdAcceptor")]
        public virtual ApplicationUser AcceptorUsers { get; set; }

        #endregion
    }
}
