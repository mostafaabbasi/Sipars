using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig
{
    public class PrivateTraining_View_ServiceLocationWorkUnit
    {
        [Required]
        public int ServiceLocationId { get; set; }

        [Required]
        public int WorkUnitId { get; set; }

        //[Required]
        // public int? PriceWorkUnit { get; set; }
        public float? PriceWorkUnit { get; set; }


        [NotMapped]
        public bool selected { get; set; }

        public string WorkUnitTitle { get; set; }

    }

    public class PrivateTraining_View_ServiceLevelList
    {
        public int ServicePropertiesId { get; set; }
        public int ServiceLevelId { get; set; }
        public int PercentServiceLevel { get; set; }
        [NotMapped]
        public bool selected { get; set; }
        public string ServiceLevelTitle { get; set; }
        public int ServiceLevelListId { get; set; }

    }

}
