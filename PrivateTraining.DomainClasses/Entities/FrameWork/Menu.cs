using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrivateTraining.DomainClasses.Entities.Security;

namespace PrivateTraining.DomainClasses.Entities.FrameWork
{
    [Table("Menus", Schema = "Framework")]
    public class Menu : BaseEntity
    {

        #region Property

        [StringLength(300)]
        [Required]
        public string Name { get; set; }

        public int ParentId { get; set; }
        
        public int ActionId { get; set; }

        [MaxLength(50)]
        public string Code{ get; set; }

        [MaxLength(100)]
        public string RoleAccess { get; set; }

        public string IconName { get; set; }
        // خدمت دهنده و مشتری =0
        //خدمت دهنده =1 
        //مشتری =2
        public byte TypeUser { get; set; }

        public int Sort { get; set; }

        #endregion

        #region nvaigation

        [JsonIgnore]
        [ForeignKey("ActionId")]
        [InverseProperty("Menus")]
        public virtual  FrameWork.Action Action { get; set; }

        #endregion

    }
}
