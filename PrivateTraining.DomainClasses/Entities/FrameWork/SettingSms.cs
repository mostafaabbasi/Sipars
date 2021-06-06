using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.FrameWork
{
    [Table("SettingSms", Schema = "Framework")]

    public class SettingSms : BaseEntity
    {
        public SettingSms()
        {
            SmsJoinTimeForUser = false;
            SmsJoinTimeFoAdmin = false;
            SmsRegistrationTimeForUser = false;
            SmsRegistrationTimeForAdmin = false;
            SmsRegistrationTimeForModrator = false;
            SmsCapacityFillingTimeForAdmin = false;
            SmsCapacityFillingTimeForModrator = false;
        }

        [Display(Name = "ارسال پیامک زمان عضویت  ، برای کاربر")]
        public bool SmsJoinTimeForUser { get; set; }

        [Display(Name = "ارسال پیامک زمان عضویت کاربر  ، برای مدیر سایت")]
        public bool SmsJoinTimeFoAdmin { get; set; }

        [Display(Name = "ارسال پیامک زمان ثبت نام در برنامه  ، برای کاربر")]
        public bool SmsRegistrationTimeForUser { get; set; }

        [Display(Name = "ارسال پیامک زمان ثبت نام در برنامه  ، برای مدیر سایت")]
        public bool SmsRegistrationTimeForAdmin { get; set; }

        [Display(Name = "ارسال پیامک زمان ثبت نام در برنامه  ، برای سرپرست ها")]
        public bool SmsRegistrationTimeForModrator { get; set; }

        [Display(Name = "ارسال پیامک زمان پرشدن ظرفیت برنامه  ، برای مدیر سایت")]
        public bool SmsCapacityFillingTimeForAdmin { get; set; }

        [Display(Name = "ارسال پیامک زمان پرشدن ظرفیت برنامه  ، برای سرپرست ها")]
        public bool SmsCapacityFillingTimeForModrator { get; set; }
    }
}
