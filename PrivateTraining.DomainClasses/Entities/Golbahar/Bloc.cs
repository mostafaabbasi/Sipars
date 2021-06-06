using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Golbahar
{
    [Table("Blocs", Schema = "Golbahar")]

    public class Bloc: BaseEntity
    {
        #region Properties

        [Display(Name = "کد پروژه")]
        public int ProjectId { get; set; }

        [Display(Name = "نام بلوک")]
        [MaxLength(50)]
        public string BlocName { get; set; }

        [Display(Name = "تعداد واحد")]
        public byte NumberOfUnit { get; set; }

        // تاریخ تحویل به اعضا
        [Display(Name = "تاریخ افتتاح")]
        [MaxLength(10)]
        public string InaugurationDate { get; set; }

        [NotMapped]
        public bool selected { get; set; }


        #endregion

        #region Navigators

        [ForeignKey("ProjectId")]
        [InverseProperty("Blocs")]
        public virtual Project Projects { get; set; }

        public virtual ICollection<Unit> Units { get; set; }

        #endregion

    }
}
