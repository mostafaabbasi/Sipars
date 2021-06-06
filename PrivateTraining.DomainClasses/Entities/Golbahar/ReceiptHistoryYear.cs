using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Golbahar
{
    [Table("ReceiptHistoryYears", Schema = "Golbahar")]
    public class ReceiptHistoryYear: BaseEntity
    {
        [Display(Name = "مبلغ اجاره")]
        public double Price { get; set; }

        [Display(Name = "سال")]
        [MaxLength(4)]
        public string Year { get; set; }

        public int ReceiptHistoryId { get; set; }

        [ForeignKey("ReceiptHistoryId")]
        [InverseProperty("ReceiptHistoryYears")]
        public virtual ReceiptHistory ReceiptHistorys { get; set; }
    }
}
