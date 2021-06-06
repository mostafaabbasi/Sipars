using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.BusDriving
{
   public class BusDriving_View_UserRole
    {
        public List<CustomUserRole> UserRoles { get; set; }
        public List<ApplicationUser> Users { get; set; }

    }
}
