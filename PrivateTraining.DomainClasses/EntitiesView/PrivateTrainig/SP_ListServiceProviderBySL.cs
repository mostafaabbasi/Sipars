using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig
{
   public  class SP_ListServiceProviderBySL
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public bool Sex { get; set; }
        public string Picture { get; set; }
        public string Path { get; set; }
        public string Resume { get; set; }
        public byte Level { get; set; }
        public int Star { get; set; }
        public int ServiceLevelListId { get; set; }

    }
}
