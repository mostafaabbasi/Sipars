using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig
{
    public class PrivateTraining_View_ServicePropertiesWorkUnit
    {
        public List<ServiceProperties> ServicesProperties { get; set; }
        public List<ServiceWorkUnit> WorkUnits { get; set; }
    }
}
