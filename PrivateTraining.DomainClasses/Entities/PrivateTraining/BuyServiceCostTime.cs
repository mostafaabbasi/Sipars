using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    // ReSharper disable  InconsistentNaming
    [Table("BuyServiceCostTime", Schema = "PrivateTraining")]
    public class BuyServiceCostTime 
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id { get; set; }
        public string dateRegister { get; set; }
        public string timeRegister { get; set; }
        
        // 0 -> pending, 1-> customer accept it, 2 -> customer reject it, 3 -> admin accept it, 4 -> admin reject it
        public int status { get; set; }
        public string statusChangeJson { get; set; }
        public string description { get; set; }
        
        //model
        
        public int buyServiceId { get; set; }
        public string date { get; set; }
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public long priceReceived { get; set; }

        public bool notFinished { get; set; }
        public bool next { get; set; }

        

        public string nextDate { get; set; }
        public string nextFromTime { get; set; }
        public string nextToTime { get; set; }

        #endregion
    }
}