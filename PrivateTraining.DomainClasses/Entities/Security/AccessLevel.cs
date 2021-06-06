using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Security
{
    [Table("AccessLevels", Schema = "Security")]
    public class AccessLevel : BaseEntity
    {

        #region Property

        public int ActionId { get; set; }


        #endregion

        #region navigation

        [ForeignKey("ActionId")]
        [InverseProperty("AccessLevels")]
        public virtual FrameWork.Action Actions { get; set; }

        #endregion

    }
}
