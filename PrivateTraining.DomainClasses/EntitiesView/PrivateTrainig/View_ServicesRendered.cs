using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig
{
    public class View_ServicesRendered
    {
        public string Service { get; set; }
        public string Location { get; set; }
        public string ServiceProvider { get; set; }
        public string ServiceReceiver { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public byte Status { get; set; }
    }
}
