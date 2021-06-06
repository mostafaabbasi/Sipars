using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Golbahar
{
    [Table("CompanyTypes", Schema = "Golbahar")]

    public class CompanyType : BaseEntity
    {
        #region Properties

        [Display(Name = "عنوان")]
        [MaxLength(50)]
        public string TypeName { get; set; }

        #endregion

        #region Navigators

        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<DelayFinePercent> DelayFinePercents { get; set; }

        #endregion
    }
}
