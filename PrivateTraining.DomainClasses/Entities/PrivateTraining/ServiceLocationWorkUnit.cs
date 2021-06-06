using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("ServiceLocationWorkUnits", Schema = "PrivateTraining")]
   public class ServiceLocationWorkUnit:BaseEntity
    {

        [Required]
        public int ServiceLocationId { get; set; }

        [Required]
        public int WorkUnitId { get; set; }

        [Required]
        public int PriceWorkUnit { get; set; }

        [Display(Name = "قیمت واحد کار محاسبه شده پس از اعمال درصد")]
        [Required]
        public double CalculationPriceServiceLocationWorkUnit { get; set; }


        #region Navigatos

        [ForeignKey("ServiceLocationId")]
        [InverseProperty("ServiceLocationWorkUnits")]
        public virtual View_ServiceLocations ServiceLocations { get; set; }

        [ForeignKey("WorkUnitId")]
        [InverseProperty("ServiceLocationWorkUnits")]
        public virtual WorkUnit WorkUnits { get; set; }

        //[ForeignKey("PriceWorkUnit")]
        //public virtual WorkUnit PriceWorkUnits { get; set; }

        #endregion

    }
}
