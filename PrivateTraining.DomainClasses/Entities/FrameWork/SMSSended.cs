using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.FrameWork
{

    [Table("SMSsSended", Schema = "FrameWork")]
    public class SMSSended : BaseEntity
    {

        public Nullable<int> UserId { get; set; }

    //    [Required]
        [Display(Name = "شماره فرستنده")]
        public string SenderNumber { get; set; }

   //     [Required]
        [Display(Name = "شماره گیرنده")]
        public string ReceiverNumber { get; set; }

    //    [Required]
        [Display(Name = "متن پیام")]
        public string Content { get; set; }

    //    [Required]
        [Display(Name = "وضعیت ارسال پیام")]
        public string Status { get; set; }

    //    [Required]
        [Display(Name = "وضعیت ارسال پیام")]
        public byte StatusType { get; set; }

    //    [Required]
        [Display(Name = "تاریخ ارسال پیام")]
        public string Date { get; set; }

     //   [Required]
        [Display(Name = "زمان ارسال پیام")]
        public string Time { get; set; }
    }
}
