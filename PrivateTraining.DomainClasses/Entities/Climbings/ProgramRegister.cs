using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Climbings
{
    [Table("ProgramRegisters", Schema = "Climbing")]
    public class ProgramRegister : BaseEntity
    {

        #region Properties

        [Display(Name = " کد برنامه")]
        public int ProgramId { get; set; }

        [Display(Name = "کاربر ثبت کننده")]
        public int UserId { get; set; }

        [Display(Name = "کد تراکنش ")]
        public int PaymentOrderId { get; set; }

        [Display(Name = "کاربر شرکت کننده در برنامه")]
        public int RegisterUserId { get; set; }

        [Display(Name = "تاریخ ثبت")]
        [Required]
        [MaxLength(20)]
        public string SaveDate { get; set; }

        [Display(Name = "مبلغ ثبت نام")]
        public int price { get; set; }
        #endregion

        #region Navigators

        //[ForeignKey("UserId")]
        //[InverseProperty("ProgramRegisters")]
        //public virtual DomainClasses.Security.ApplicationUser Users { get; set; }

        //[ForeignKey("RegisterUserId")]
        //public virtual DomainClasses.Security.ApplicationUser RegisterUsers { get; set; }

        //[ForeignKey("ProgramId")]
        //[InverseProperty("ProgramRegisters")]
        //public virtual Program Programs { get; set; }

        #endregion

    }
}
