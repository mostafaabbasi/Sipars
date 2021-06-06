using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.BaseTable
{
    [Table("States", Schema = "BaseInfo")]
    public class State :BaseEntity
    {
        #region Property
        public string Name { get; set; }
        #endregion

        #region nvaigation
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public virtual ICollection<City> Cities { get; set; }

        #endregion
    }
}
