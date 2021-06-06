using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Golbahar
{
    [Table("Projects", Schema = "Golbahar")]
    public class Project :BaseEntity
    {
        #region Properties

        [Display(Name = "کد شرکت")]
        public int CompanyId { get; set; }

        [Display(Name = "کد پروژه")]
        [MaxLength(20)]
        public string ProjectCode { get; set; }

        [Display(Name = "نام پروژه")]
        [MaxLength(50)]
        public string ProjectName { get; set; }

        [Display(Name = "تاریخ تحویل به شرکت")]
        [MaxLength(10)]
        public string ProjectDeliveryDate { get; set; }

        [Display(Name = "عرصه کل")]
        public double TotalField { get; set; }

        [Display(Name = "اعیان کل")]
        public double TotalArea { get; set; }

        [Display(Name = "قیمت منطقه")]
        public double PriceOfArea { get; set; }

        [NotMapped]
        public bool selected { get; set; }


        #endregion

        #region Navigators

        [ForeignKey("CompanyId")]
        [InverseProperty("Projects")]
        public virtual Company Companies { get; set; }

        public virtual ICollection<Bloc> Blocs { get; set; }

        #endregion

    }
}
