using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("ServiceLevels", Schema = "PrivateTraining")]

    public class ServiceLevel: BaseEntity
    {
        [Required]
        [Display(Name = "عنوان")]
        public string Title { get; set; }

   //     public byte Percent { get; set; }

        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        [InverseProperty("ServiceLevels")]
        public virtual Service Services { get; set; }

        public virtual ICollection<ServiceLevelList> ServiceLevelLists { get; set; }

    }
}
