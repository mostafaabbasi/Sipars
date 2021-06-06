using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig
{
   public  class PrivateTraining_View_UserServiceLocations
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string TitleService { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public byte StatusServiceLocationUser { get; set; }

    }
}
