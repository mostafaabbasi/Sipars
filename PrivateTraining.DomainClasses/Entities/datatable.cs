using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;

namespace PrivateTraining.DomainClasses.Entities
{
    public class datatable
    {
        public string draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<string[]> data { get; set; }
        public string start { get; set; }
        public string length { get; set; }
        public string sortColumn { get; set; }
        public string sortColumnDir { get; set; }
        public string searchValue { get; set; }
        public int pageSize { get; set; }
        public int skip { get; set; }
        public JArray list { get; set; }
        

    }
}
