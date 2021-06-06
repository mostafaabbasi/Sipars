using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Golbahar
{
    [Table("Units", Schema = "Golbahar")]
    public class Unit : ApplicationUser
    {
        #region Properties

        [Display(Name = "کد بلوک")]
        public int BlocId { get; set; }

        [Display(Name = "سال شروع قرارداد")]
        [MaxLength(10)]
        public string YearStartContract { get; set; }

        [Display(Name = "سال افتتاح")]
        [MaxLength(10)]
        public string YearInauguration { get; set; }

        [Display(Name = "طبقه")]
        [MaxLength(10)]
        public string Floor { get; set; }

        [Display(Name = "شماره واحد")]
        [MaxLength(10)]
        public string UnitNumber { get; set; }

        [Display(Name = "پلاک ثبتی")]
        [MaxLength(10)]
        public string RegistrationPlate { get; set; }

        [Display(Name = "اعیان")]
        public double Area { get; set; }

        [Display(Name = "سهم العرصه")]
        public double PortionField { get; set; }

        [Display(Name = "قیمت منطقه")]
        public double PriceOfArea { get; set; }

        [Display(Name = "نام پدر")]
        [MaxLength(50)]
        public string FatherName { get; set; }

        [Display(Name = "شماره قرارداد")]
        [MaxLength(20)]
        public string ContractNumber { get; set; }

        [Display(Name = "تاریخ قرارداد")]
        [MaxLength(20)]
        public string ContractDate { get; set; }

        [Display(Name = "شناسه قبض")]
        [MaxLength(50)]
        public string BillingID { get; set; }

        [Display(Name = "کدنوسازی جدید")]
        [MaxLength(50)]
        public string RenovationCodeNew { get; set; }

        #endregion

        #region Navigators

        [ForeignKey("BlocId")]
        [InverseProperty("Units")]
        public virtual Bloc Blocs { get; set; }

        public virtual ICollection<CalRent> CalRents { get; set; }

        #endregion
    }
}

