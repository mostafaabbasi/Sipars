using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Climbings
{
    [Table("Equipments", Schema = "Climbing")]
    public class Equipment :BaseEntity
    {
        #region Properties

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        #endregion
    }
}
