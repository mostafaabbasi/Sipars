using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Extention
{
    public enum StatusLeave
    {
        NotChecked = 0,
        Confirm = 1,
        Reject = 2,
        Reference = 3
    }

    public enum StatusColorLeave
    {
        Gray = 0,
        Green = 1,
        Orange = 2,
        Red = 3
    }

    public enum Roles
    {
        Administrator = 1,
        Admin = 2,
        Modrator = 3,
        User = 4,
            ServiceProvider=5,
    }

    public enum DeleteUserRecord
    {
        Delete = 1,
        Show = 0
    }
    public enum UserType
    {
        NotUser = 0,
        ServiceProvider = 1,
        ServiceReceiver = 2,
    }

    public enum StatusRequest
    {
        Send = 0,
        Read = 1,
        Progress = 2,
    }

    public enum StatusServiceLocationUser
    {
        /// <summary>
        /// در انتظار تایید
        /// </summary>
        PendingApproval = 0,
        /// <summary>
        /// فعال
        /// </summary>
        Active = 1,
        /// <summary>
        /// غیر فعال
        /// </summary>
        DeActive = 2,
        /// <summary>
        /// رزرو شده
        /// </summary>
        Reservation = 3,
        /// <summary>
        /// ثبت اطلاعات شدگان
        /// </summary>
        SubmitInformation = 4
    }
    public enum StatusServiceLocationRequest
    {
        [Description("موافق ")]
        Accept = 1,

        [Description("قطعی ")]
        certain = 2,
 
        [Description("ناتمام ")]
        Unfinished = 3,

        [Description("اتمام ")]
        final = 4,

        [Description("در حال بررسی ")]
        checking = 0,

        [Description("غیر قطعی ")]
        UnCertain = 6,
        
        [Description("مخالفت شده ")]
        Rejected = 7,
        
        [Description("لغو شده ")]
        Canceled = 8,
        
        [Description("پاک شده ")]
        Deleted = 8,
    }

    public enum StatusConfirmServiceReciverRequest
    {
        NotChecked = 0,
        Confirm = 1,
        Reject = 2,
    }

    public enum HowPerformService
    {
        /// <summary>
        /// شرکتی
        /// </summary>
        Corporative = 0,
        /// <summary>
        /// شخصی
        /// </summary>
        Personal = 1,
        /// <summary>
        /// شرکتی و شخصی
        /// </summary>
        CorporativePersonal = 2,
    }
    public enum Problem
    {
        /// <summary>
        /// اعلام بروز مشکل پس از قطعی شدن خدمت توسط خدمت دهنده
        /// </summary>
        [Description("اعلام بروز مشکل پس از قطعی شدن خدمت توسط خدمت دهنده")]
        AfterCertainByServiceProvider = 1,

        /// <summary>
        /// اعلام بروز مشکل پس از قطعی شدن خدمت توسط مشتری
        /// </summary>
        [Description("اعلام بروز مشکل پس از قطعی شدن خدمت توسط مشتری")]
        AfterCertainByServiceReceiver = 2,

        /// <summary>
        /// غیر قطعی کردن خدمت
        /// </summary>
        [Description("غیر قطعی کردن خدمت")]
        UnCertain = 3,

        /// <summary>
        ///وجود نداشتن خدمت دهنده دیگر برای انتقال خدمت به آن
        /// </summary>
        [Description("وجود نداشتن خدمت دهنده دیگر برای انتقال خدمت به آن")]
        NotFindAnotherServiceProvider = 4
    }


    public enum TypeUserFile
    {
        /// <summary>
        /// کارت ملی
        /// </summary>
        NationalCard = 0,

        /// <summary>
        /// مدرک تحصیلی
        /// </summary>
        DegreeEducation = 1,

        /// <summary>
        /// مدرک فنی و حرفه ای
        /// </summary>
        Vocational = 2,

        /// <summary>
        ///سایر مدارک
        /// </summary>
        Other = 3,
    }

    //-------------------------- Climbing

    public enum UploadFileEnum
    {
        [Description("/Userfiles/Club")]
        Club,
        [Description("/Userfiles/Program")]
        Program,
        [Description("/Userfiles/ProfilePicture")]
        ProfilePicture
    }

    public enum FileType
    {
        Logo = 1,
        File = 2,
        Pic = 3,
        UserPic=4,

    }

    public enum ProgramStatus
    {
        //Now = 1,
        //End = 2,
        //Cancel = 3,
        [Description("عدم نمایش ")]
        Hide = 0,
        [Description("نمایش")]
        Show = 1,
        [Description("Trash")]
        Trash = 2,
        [Description("کنسل شده")]
        Cancel = 3,
        [Description("حذف شده")]
        Del = 4,
    }

    public enum ProgramStatusName
    {
        //Now = 1,
        //End = 2,
        //Cancel = 3,
        [Description("در انتظار اجرا")]
        NotStart = 1,
        [Description("در حال اجرا")]
        Now = 2,
        [Description("کنسل شده")]
        Cancel = 3,
        [Description("اتمام برنامه")]
        End = 4,
    }

    public enum ProgramRegisterStatusName
    {
        [Description("ثبت نام هنوز شروع نشده است.")]
        NotStart = 1,
        [Description("ثبت نام")]
        Ok = 2,
        [Description("برنامه کنسل شده")]
        Cancel = 3,
        [Description("اتمام مهلت ثبت نام")]
        End = 4,
    }


    public enum SexType
    {
        [Description("مذکر")]
        Male = 1,
        [Description("مونث")]
        Female = 2,
        [Description("هر دو")]
        Both = 3,
    }

    public enum CountDayType
    {
        [Description("یک روز")]
        Day = 1,
        [Description("بیش از یک روز")]
        Days = 2,
    }

    public enum TransactionType
    {
        //[Description("ثبت نام در برنامه")]
        //Buy = 1,
        //[Description("کنسلی")]
        //Cancel = 2,
        //[Description("شارژ پنل")]
        //Panel = 3,
        [Description("دریافت")]
        Received = 1,
        [Description("برداشت از پنل")]
        Payable = 3,
        [Description("برگشت به پنل")]
        Back = 2,
        [Description("برگشت به کارت")]
        Account = 4,
    }

    public enum PaymentType
    {
        //[Description("پرداخت الکترونیک")]
        //Online = 1,
        //[Description("idpay")]
        //idPay = 2,
        //[Description("کنسلی")]
        //Cancel = 3,
        [Description("پرداخت الکترونیک")]
        Online = 1,
        [Description("idpay")]
        idPay = 2,
        [Description("پنل")]
        Panel = 3,

    }

    public enum TransactionName
    {
        [Description("ثبت نام در برنامه")]
        Buy = 1,
        [Description("شارژ پنل")]
        Panel =2,
    }

    public enum ActivePayment
    {
        [Description("تایید")]
        Ok = 1,
        [Description("عدم تایید")]
        No = 2,
        [Description("در انتظار تایید ")]
        Pending = 0,
    }

    public enum ActivePaymentForUser
    {
        [Description("کنسل شده")]
        No = 1,
        [Description("کنسل شود")]
        Ok = 0,
    }

    public enum Paymentverified
    {
        //0 تراکنش ثبت شده ولی سمت بانک نرفته است
        //1 تراکنش توسط بانک تایید شده است
        //2 تراکنش تایید نشده است
        //3 فیش بانکی ثبت شده است
        NotSend = 0,
        TransactionOk = 1,
        TransactionCancel = 2,
        RecieptOk = 3,
        NotOnline = 4,
    }

    public enum TypeStatusDeliverSms
    {
        [Description(" ارسال پیام انجام نشد")]
        Error = 0,
        [Description(" ارسال پیام با موفقیت انجام شد")]
        Success = 1,
    }

    public enum PaymentTypeGolbahar
    {
        [Description("پرداخت الکترونیک")]
        Online = 1,
        [Description("فیش")]
        Receipt = 0,
    }



}
