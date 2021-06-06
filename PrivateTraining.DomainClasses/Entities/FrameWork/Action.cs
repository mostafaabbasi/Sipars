using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.DomainClasses.Entities.Security;

namespace PrivateTraining.DomainClasses.Entities.FrameWork
{
    [Table("Actions", Schema = "Framework")]
    public class Action : BaseEntity
    {

        #region Property

        [StringLength(300)]
        [Required]
        public string Name { get; set; }

        [StringLength(100)]
        [Required]
        public string Actionname { get; set; }

        [StringLength(100)]
        [Required]
        public string Controller { get; set; }

        [StringLength(100)]
        public string Area { get; set; }
        public int ParentId { get; set; }

        [StringLength(300)]
        public string ShowName { get; set; }

        [StringLength(30)]
        [Required]
        public string AccessCode { get; set; }

        #endregion

        #region nvaigation
        public ICollection<AccessLevel> AccessLevels { get; set; }
        public ICollection<Menu> Menus { get; set; }
        #endregion

    }
}
