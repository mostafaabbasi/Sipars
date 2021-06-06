using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.EntitiesView
{
    public class DataTable
    {
        public int sEcho { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }
        public int iColumn { get; set; }

        public string filter { get; set; }
    }
}
