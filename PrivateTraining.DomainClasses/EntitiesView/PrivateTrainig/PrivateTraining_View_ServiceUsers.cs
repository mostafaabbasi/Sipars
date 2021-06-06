using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig
{
    public class PrivateTraining_View_ServiceUsers
    {
        public int Id { get; set; }
        public int Serviceid { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
        public int ParentId { get; set; }
        public int Level { get; set; }
        public bool IsEnable { get; set; }
        // در حال بررسی =0 و فعال =1 وغیرفعال =2
        public byte ActiveServiceForUser { get; set; }
        public int CapacityServiceUser { get; set; }
        [NotMapped]
        public bool selected { get; set; }
        public int MaxCapacity { get; set; }
        public List<PrivateTraining_View_ServiceLevelList> ListServiceLevel { get; set; }
        public int ServiceLevelListId { get; set; }

    }
}
