using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Golbahar
{
    [Table("SettingGolbahars", Schema = "Golbahar")]

   public class SettingGolbahar :BaseEntity
    {
        [Display(Name = "بانک (فیش)")]
        [MaxLength(50)]
        public string ReceiptBankName { get; set; }

        [Display(Name = "شماره حساب (فیش)")]
        [MaxLength(50)]
        public string ReceiptAccountNumber { get; set; }

        [Display(Name = "توضیحات (فیش)")]
        [MaxLength(500)]
        public string ReceiptDesc { get; set; }
    }
}
