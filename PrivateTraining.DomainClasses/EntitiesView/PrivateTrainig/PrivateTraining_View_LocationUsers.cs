using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig
{
    public class PrivateTraining_View_LocationUsers
    {
        public int Id { get; set; }
      //  public int LocationId { get; set; }
        public string Name { get; set; }
      //  public int UserId { get; set; }
        [NotMapped]
        public bool selected { get; set; }
        public int CityId { get; set; }


    }
}
