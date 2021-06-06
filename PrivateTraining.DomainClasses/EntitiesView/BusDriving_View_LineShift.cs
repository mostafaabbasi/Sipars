using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.DomainClasses.Entities.BusDriving;
using Microsoft.AspNet.Identity;
using PrivateTraining.DomainClasses.Security;

namespace PrivateTraining.DomainClasses.EntitiesView
{
    public class BusDriving_View_LineShift
    {

        public List<Line> Lines { get; set; }
        public List<Shift> Shifts { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public bool currentUser { get; set; }
        public List<SupplementaryInfoUser> SupplementaryInfoUsers { get; set; }


    }
}
