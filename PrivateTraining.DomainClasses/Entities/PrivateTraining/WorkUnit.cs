using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("WorkUnits", Schema = "PrivateTraining")]
    public class WorkUnit:BaseEntity
    {
        [Required]
        [Display(Name = "عنوان واحد کار")]
        public string Title { get; set; }

        [NotMapped]
        public bool selected { get; set; }


        //public virtual ICollection<ServiceProperties> ServicesProperties { get; set; }
        public virtual ICollection<ServiceWorkUnit> ServiceWorkUnits { get; set; }
        public virtual ICollection<ServiceLocationWorkUnit> ServiceLocationWorkUnits { get; set; }
        public virtual ICollection<ServiceReceiverServiceLocation> ServiceReceiverServiceLocations { get; set; }

    }
}
