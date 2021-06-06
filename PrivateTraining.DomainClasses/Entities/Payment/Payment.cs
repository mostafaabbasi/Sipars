using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Payment
{
    // ReSharper disable  InconsistentNaming
    [Table("Payment", Schema = "Payment")]
    public class Payment 
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int userId { get; set; }
        
        
        //admin id , bank payment id, buy service
        public int refId { get; set; }
        
        public int price { get; set; }
//        public bool charge { get; set; }
        
        
        public string date{ get; set; }
        public string time{ get; set; }
        
        //bank, admin, buy, service, customerCash
        public int refType { get; set; }
        
        //not used yet can be offline or online 
        public string type { get; set; }
        
        public int status { get; set; }

        
        
        #endregion
    }

    public static class PaymentTypeEnum
    {
        public static int bank = 1;
        public static int admin = 2;
        public static int minPricePay = 3;
        public static int customerFinishPay = 4;
        public static int customerCash = 5;
    }

}