using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("Services", Schema = "PrivateTraining")]
    public class Service : BaseEntity
    {

        #region Properties

        [Display(Name = "عنوان خدمت")]
        [Required]
        public string Title { get; set; }

        [Required]
        public int ParentId { get; set; }

        [Required]
        public int Level { get; set; }

        [NotMapped]
        public bool selected { get; set; }

        [Required]
        public bool automation { get; set; }
        #endregion

        #region Navigators

        //public virtual ICollection<ServiceNotInLocation> ServiceNotInLocations { get; set; }
        //   public virtual ICollection<View_ServiceLocations> ServiceLocations { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserServiceLocation> UserServiceLocations { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<ServiceLevel> ServiceLevels { get; set; }

        #endregion

    }
}
