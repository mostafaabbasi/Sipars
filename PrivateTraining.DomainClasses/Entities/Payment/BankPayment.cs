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
    [Table("BankPayment", Schema = "Payment")]
    public class BankPayment 
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        
        
        //0 تراکنش ثبت شده ولی سمت بانک نرفته است
        //1 تراکنش توسط بانک تایید شده است
        //2 تراکنش تایید نشده است
        //3 فیش بانکی ثبت شده است
        public int status { get; set; }
        public int userId { get; set; }
        public int price { get; set; }
        
        
        //1 zarin pal
        public int bankCode { get; set; }

        public string date{ get; set; }
        public string time{ get; set; }
        
        public string okDate{ get; set; }
        public string okTime{ get; set; }
        
        public string detailJson{ get; set; }
        
        [Display(Name = "کد رهگیری")]
        //  [Required]
        //  [MaxLength(100)]
        public string transactionCode { get; set; }
        
        
        #endregion
    }
}