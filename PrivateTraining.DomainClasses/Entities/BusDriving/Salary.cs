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
    [Table("Salaries", Schema = "BusDriving")]
    public class Salary : BaseEntity
    {
        public int UserId { get; set; }

        [Display(Name = "تاریخ فیش")]
        [MaxLength(10)]
        public string SalaryDate { get; set; }

        [Display(Name = "تاریخ فیش")]
        [MaxLength(20000)]
        public string SalaryDetail { get; set; }

        [Display(Name = "انتخاب ماه ")]
        public byte Month { get; set; }

        [Display(Name = "انتخاب سال ")]
        public int YearId { get; set; }

        #region Navigators

        [ForeignKey("UserId")]
        [InverseProperty("Salaries")]
        public virtual ApplicationUser Users { get; set; }

        [ForeignKey("YearId")]
        [InverseProperty("Salaries")]
        public virtual Year Years { get; set; }

        #endregion

    }
}
