using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.DomainClasses.Entities.FrameWork;

namespace PrivateTraining.DataLayer.Context
{
    public class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            #region Actions

            #region MainPage 1001 , 1002

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Id = 1,
            //    Name = "صفحه اصلی سایت",
            //    Actionname = "Index",
            //    Controller = "Home",
            //    Area = "",
            //    ParentId = 0,
            //    IsEnable = true,
            //    ShowName = "صفحه اصلی سایت",
            //    AccessCode = "1001"
            //});
            //context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Id = 1,
                Name = "صفحه اصلی پنل",
                Actionname = "IndexPanel",
                Controller = "Home",
                Area = "",
                ParentId = 0,
                IsEnable = true,
                ShowName = "صفحه اصلی پنل",
                AccessCode = "1002",

            });
            context.SaveChanges();

            #endregion

            #region User Info 1003

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "اطلاعات کاربر",
                Actionname = "Index",
                Controller = "Account",
                Area = "",
                ParentId = 0,
                IsEnable = true,
                ShowName = "اطلاعات کاربر",
                AccessCode = "1003"
            });
            context.SaveChanges();
            int Infos = context.Actions.FirstOrDefault(c => c.AccessCode == "1003").Id;

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "ویرایش اطلاعات",
            //    Actionname = "NewRegister?Type=Edit",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = Infos,
            //    IsEnable = true,
            //    ShowName = "ویرایش اطلاعات",
            //    AccessCode = "1003001"
            //});

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "تغییر رمز عبور",
                Actionname = "ChangePassword",
                Controller = "Account",
                Area = "",
                ParentId = Infos,
                IsEnable = true,
                ShowName = "تغییر رمز عبور",
                AccessCode = "1003002"
            });

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "انتخاب دوست",
                Actionname = "SelectFreinds",
                Controller = "Freind",
                Area = "Framework",
                ParentId = Infos,
                IsEnable = true,
                ShowName = "انتخاب دوست",
                AccessCode = "1003003"
            });

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "ثبت عکس",
            //    Actionname = "InsertUserPic",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = Infos,
            //    IsEnable = true,
            //    ShowName = "ثبت عکس",
            //    AccessCode = "1003004"
            //});

            #endregion

            #region Access List  Access Code 2001

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Id = 2,
                Name = "سطوح دسترسی",
                Actionname = "UserInGroup",
                Controller = "GroupPolicy",
                Area = "Security",
                ParentId = 0,
                IsEnable = true,
                ShowName = "تعريف گروههاي دسترسي",
                AccessCode = "2001"
            });
            context.SaveChanges();
            int AccessId = context.Actions.FirstOrDefault(c => c.AccessCode == "2001").Id;

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Id = 2,
                Name = "تعريف گروههاي دسترسي",
                Actionname = "UserInGroup",
                Controller = "GroupPolicy",
                Area = "Security",
                ParentId = AccessId,
                IsEnable = true,
                ShowName = "تعريف گروههاي دسترسي",
                AccessCode = "2001001"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Id = 3,
                Name = "تعیین دسترسی گروه",
                Actionname = "ActionInGroup",
                Controller = "AccessLevel",
                Area = "Security",
                ParentId = AccessId,
                IsEnable = true,
                ShowName = "تعیین دسترسی گروه",
                AccessCode = "2001002"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Id = 4,
                Name = "تعیین دسترسی کاربر",
                Actionname = "UserAccess",
                Controller = "AccessLevel",
                Area = "Security",
                ParentId = AccessId,
                IsEnable = true,
                ShowName = "تعیین دسترسی کاربر",
                AccessCode = "2001003"
            });
            context.SaveChanges();

            #endregion Access List

            #region Security User 10002
            //----------------------- Access Code 10002  ------------------------------------------------
            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "مدیریت کاربران",
                Actionname = "Index",
                Controller = "Account",
                Area = "",
                ParentId = 0,
                IsEnable = true,
                ShowName = "مدیریت کاربران",
                AccessCode = "10002"
            });
            context.SaveChanges();
            int UserId = context.Actions.FirstOrDefault(c => c.AccessCode == "10002").Id;

            //---------- USER-------------

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " لیست کاربران",
                Actionname = "ListUsers",
                Controller = "Account",
                Area = "",
                ParentId = UserId,
                IsEnable = true,
                ShowName = "لیست کاربران",
                AccessCode = "10002001"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "صفحه ثبت نام کاربر",
                Actionname = "Register",
                Controller = "Account",
                Area = "",
                ParentId = UserId,
                IsEnable = true,
                ShowName = "صفحه ثبت نام کاربر",
                AccessCode = "10002002"
            });
            context.SaveChanges();

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = " ثبت نام اولیه خدمت دهنده",
            //    Actionname = "GetAddEditServiceProvider",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = " ثبت نام  اولیه خدمت دهنده",
            //    AccessCode = "10002003"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = " اطلاعات تکمیلی خدمت دهنده",
            //    Actionname = "GetAddEditServiceProviderInfo",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = " اطلاعات تکمیلی خدمت دهنده",
            //    AccessCode = "10002004"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = " مدیریت خدمت دهنده ها",
            //    Actionname = "ListServiceProvider",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = " مدیریت خدمت دهنده ها",
            //    AccessCode = "10002005"
            //});
            //context.SaveChanges();


            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "ثبت نام کاربر جدید",
            //    Actionname = "NewRegister",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = "ثبت نام کاربر جدید",
            //    AccessCode = "10002008"
            //});
            //context.SaveChanges();

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = " مدیریت خدمت گیرنده ها",
            //    Actionname = "ListServiceReceiver",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = " مدیریت خدمت گیرنده ها",
            //    AccessCode = "10002006"
            //});
            //context.SaveChanges();
            #endregion User

            #region Framework 10008 , 20003

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " مدیریت پیام ها ",
                Actionname = "MessagForAdmin",
                Controller = "Message",
                Area = "Framework",
                ParentId = 0,
                IsEnable = true,
                ShowName = " مدیریت پیام ها ",
                AccessCode = "10008"
            });
            context.SaveChanges();

            int MessageId = context.Actions.FirstOrDefault(c => c.AccessCode == "10008").Id;

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " ارسال پیام ",
                Actionname = "MessagForAdmin",
                Controller = "Message",
                Area = "Framework",
                ParentId = MessageId,
                IsEnable = true,
                ShowName = " ارسال پیام ",
                AccessCode = "10008001"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " لیست پیام ها ",
                Actionname = "ListMessages",
                Controller = "Message",
                Area = "Framework",
                ParentId = MessageId,
                IsEnable = true,
                ShowName = " لیست پیام ها ",
                AccessCode = "10008002"
            });
            context.SaveChanges();

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = " تنظیمات ",
            //    Actionname = "Setting",
            //    Controller = "Setting",
            //    Area = "Framework",
            //    ParentId = 0,
            //    IsEnable = true,
            //    ShowName = " تنظیمات ",
            //    AccessCode = "20003"
            //});
            //context.SaveChanges();

            #endregion

            // PrivateTraining 10001 , 10003,  10004 , 10005 , 10007

            // Climing 10009  ,20001  ,20002

            #region Golbahar 20004,20005

            #region Save Info

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " ثبت اطلاعات  ",
                Actionname = "Index",
                Controller = "Golbaharr",
                Area = "GolbaharArea",
                ParentId = 0,
                IsEnable = true,
                ShowName = " ثبت اطلاعات ",
                AccessCode = "20004"
            });
            context.SaveChanges();
            int Gol = context.Actions.FirstOrDefault(c => c.AccessCode == "20004").Id;

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " ثبت اطلاعات از طریق اکسل ",
                Actionname = "ImportInfo",
                Controller = "Golbaharr",
                Area = "GolbaharArea",
                ParentId = Gol,
                IsEnable = true,
                ShowName = " ثبت اطلاعات از طریق اکسل ",
                AccessCode = "20004001"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " تنظیمات ",
                Actionname = "VSettingGolbahar",
                Controller = "Golbaharr",
                Area = "GolbaharArea",
                ParentId = Gol,
                IsEnable = true,
                ShowName = " تنظیمات ",
                AccessCode = "20004002"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " درصد یا مبلغ افزایش سالیانه ",
                Actionname = "PercentagIncrease",
                Controller = "Golbaharr",
                Area = "GolbaharArea",
                ParentId = Gol,
                IsEnable = true,
                ShowName = " درصد یا مبلغ افزایش سالیانه ",
                AccessCode = "20004003"
            });
            context.SaveChanges();


            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " درصد یا مبلغ جریمه ",
                Actionname = "DelayFinePercents",
                Controller = "Golbaharr",
                Area = "GolbaharArea",
                ParentId = Gol,
                IsEnable = true,
                ShowName = " درصد یا مبلغ جریمه ",
                AccessCode = "20004004"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " ثبت پرداختی ",
                Actionname = "InsertPay",
                Controller = "Golbaharr",
                Area = "GolbaharArea",
                ParentId = Gol,
                IsEnable = true,
                ShowName = " ثبت پرداختی ",
                AccessCode = "20004005"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " درصد خوش حسابی ",
                Actionname = "AccountPercents",
                Controller = "Golbaharr",
                Area = "GolbaharArea",
                ParentId = Gol,
                IsEnable = true,
                ShowName = " درصد خوش حسابی ",
                AccessCode = "20004006"
            });
            context.SaveChanges();

            #endregion

            #region ShowList

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " گزارشات  ",
                Actionname = "Index",
                Controller = "Golbaharr",
                Area = "GolbaharArea",
                ParentId = 0,
                IsEnable = true,
                ShowName = " گزارشات ",
                AccessCode = "20005"
            });
            context.SaveChanges();
            int Project = context.Actions.FirstOrDefault(c => c.AccessCode == "20005").Id;

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " لیست شرکت ها ",
                Actionname = "ListCompany",
                Controller = "Company",
                Area = "GolbaharArea",
                ParentId = Project,
                IsEnable = true,
                ShowName = " لیست شرکت ها ",
                AccessCode = "20005001"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " لیست مجتمع ها ",
                Actionname = "ListProject",
                Controller = "Project",
                Area = "GolbaharArea",
                ParentId = Project,
                IsEnable = true,
                ShowName = " لیست مجتمع ها ",
                AccessCode = "20005002"
            });
            context.SaveChanges();

            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " لیست بلوک ها ",
                Actionname = "ListBloc",
                Controller = "Bloc",
                Area = "GolbaharArea",
                ParentId = Project,
                IsEnable = true,
                ShowName = " لیست بلوک ها ",
                AccessCode = "20005003"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " لیست واحد ها ",
                Actionname = "ListUnit",
                Controller = "Unit",
                Area = "GolbaharArea",
                ParentId = Project,
                IsEnable = true,
                ShowName = " لیست واحد ها ",
                AccessCode = "20005004"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " نمایش فیش اجاره ها ",
                Actionname = "PrintReceipt",
                Controller = "Unit",
                Area = "GolbaharArea",
                ParentId = Project,
                IsEnable = true,
                ShowName = " نمایش فیش اجاره ها ",
                AccessCode = "20005005"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " لیست پرداختی ها ",
                Actionname = "ListPayment",
                Controller = "Golbaharr",
                Area = "GolbaharArea",
                ParentId = Project,
                IsEnable = true,
                ShowName = " لیست پرداختی ها ",
                AccessCode = "20005006"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " گزارش بدهی ها ",
                Actionname = "ReportDebt",
                Controller = "Golbaharr",
                Area = "GolbaharArea",
                ParentId = Project,
                IsEnable = true,
                ShowName = " گزارش بدهی ها ",
                AccessCode = "20005007"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "نمودار نوع پرداختی ",
                Actionname = "ReportPaymentType",
                Controller = "Golbaharr",
                Area = "GolbaharArea",
                ParentId = Project,
                IsEnable = true,
                ShowName = " نمودار نوع پرداختی ",
                AccessCode = "20005008"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "نمودار شرکت های فعال ",
                Actionname = "ReportActiveCompany",
                Controller = "Golbaharr",
                Area = "GolbaharArea",
                ParentId = Project,
                IsEnable = true,
                ShowName = " نمودار شرکت های فعال ",
                AccessCode = "20005009"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " لیست گزارش بدهی ",
                Actionname = "ListReportDebt",
                Controller = "Report",
                Area = "GolbaharArea",
                ParentId = Project,
                IsEnable = true,
                ShowName = " لیست گزارش بدهی ",
                AccessCode = "20005010"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " نمایش تاریخچه فیش ها ",
                // Actionname = "PrintReceipt?History=2",
                Actionname = "ShowUserHistory?History=2",
                Controller = "Unit",
                Area = "GolbaharArea",
                ParentId = Project,
                IsEnable = true,
                ShowName = " نمایش تاریخچه فیش ها ",
                AccessCode = "20005011"
            });
            context.SaveChanges();

            #endregion

            #region sms 

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "ارسال پیامک به اعضا",
                Actionname = "SendSmsForUnit",
                Controller = "Golbaharr",
                Area = "GolbaharArea",
                ParentId = 0,
                IsEnable = true,
                ShowName = "ارسال پیامک به اعضا",
                AccessCode = "20007"
            });
            context.SaveChanges();

            #endregion

            #endregion

            #endregion Actions

            //***********************************************************************************************************//

            #region Menus

            #region MainPage

            //context.Menus.AddOrUpdate(p => p.Name,
            //    new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //    {
            //        Name = "صفحه اصلی سایت",
            //        ActionId = context.Actions.Where(c => c.AccessCode == "1001").FirstOrDefault().Id,
            //        IsEnable = true,
            //        ParentId = 0,
            //        Code = "1001",
            //        RoleAccess = "Admin,Administrator,User,Modrator,User",
            //        IconName = "fa-external-link-square",
            //    });
            //context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name,
                new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
                {
                    Name = "صفحه اصلی پنل",
                    ActionId = context.Actions.Where(c => c.AccessCode == "1002").FirstOrDefault().Id,
                    IsEnable = true,
                    ParentId = 0,
                    Code = "1002",
                    RoleAccess = "Admin,Administrator,User,Modrator,User",
                    IconName = "fa-home",
                    Sort = 1,
                });
            context.SaveChanges();

            #endregion

            #region User Info 

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "اطلاعات کاربر",
                ActionId = 1,
                IsEnable = true,
                ParentId = 0,
                Code = "1003",
                RoleAccess = "Admin,Administrator,Modrator,User",
                IconName = "fa-edit",
                Sort = 2,
            });
            context.SaveChanges();
            int UInfos = context.Menus.FirstOrDefault(c => c.Code == "1003").Id;

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "ویرایش اطلاعات",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "1003001").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = UInfos,
            //    Code = "1003001",
            //    RoleAccess = "Admin,Administrator,Modrator,User",
            //});
            //context.SaveChanges();


            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "تغییر رمز عبور",
                ActionId = context.Actions.Where(c => c.AccessCode == "1003002").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = UInfos,
                Code = "1003002",
                RoleAccess = "Admin,Administrator,Modrator,User",
            });
            context.SaveChanges();

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "انتخاب همنورد",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "1003003").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = UInfos,
            //    Code = "1003003",
            //    RoleAccess = "Admin,Administrator,Modrator,User",
            //});
            //context.SaveChanges();

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "ثبت عکس",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "1003004").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = UInfos,
            //    Code = "1003004",
            //    RoleAccess = "Admin,Administrator,Modrator,User",
            //});
            //context.SaveChanges();

            #endregion User Info

            #region User
            //------------------------ Usere Code : 5002 -------------------------------------
            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "کاربران",
                ActionId = 1,
                IsEnable = true,
                ParentId = 0,
                Code = "5002",
                RoleAccess = "Admin,Administrator",
                IconName = "fa-user",
                Sort = 3,
            });
            context.SaveChanges();
            int UserData = context.Menus.FirstOrDefault(c => c.Code == "5002").Id;

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "لیست کاربران",
                ActionId = context.Actions.Where(c => c.AccessCode == "10002001").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = UserData,
                Code = "10002001",
                RoleAccess = "Admin,Administrator"
            });
            context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "مدیریت خدمت دهنده ها",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "10002005").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = UserData,
            //    Code = "10002005",
            //    RoleAccess = "Admin,Administrator,Modrator"
            //});
            //context.SaveChanges();

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "ثبت نام کاربر",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "10002002").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = UserData,
            //    Code = "10002002",
            //    RoleAccess = "Admin,Administrator,Modrator,User"
            //});
            //context.SaveChanges();

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "مدیریت خدمت دهنده ها",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "10002005").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = UserData,
            //    Code = "10002005",
            //    RoleAccess = "Admin,Administrator,Modrator"
            //});
            //context.SaveChanges();


            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "ثبت نام",
                ActionId = context.Actions.Where(c => c.AccessCode == "10002008").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = UserData,
                Code = "10002008",
                RoleAccess = "Admin,Administrator,Modrator,User"
            });
            context.SaveChanges();

            //------------------------ User Code : 5004 -------------------------------------

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "مدیریت خدمات کاربران ",
            //    ActionId = 1,
            //    IsEnable = true,
            //    ParentId = 0,
            //    Code = "5004",
            //    RoleAccess = "User",
            //    IconName = ""
            //});
            //context.SaveChanges();

            //int ParentServiceProvider = context.Menus.FirstOrDefault(c => c.Code == "5004").Id;

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "خدمات جدید ",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "10004001").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = ParentServiceProvider,
            //    Code = "10004001",
            //    RoleAccess = "User",
            //});
            //context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "خدمات قطعی شده ",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "10004002").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = ParentServiceProvider,
            //    Code = "10004002",
            //    RoleAccess = "User",
            //});
            //context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "خدمات تمام شده ",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "10004003").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = ParentServiceProvider,
            //    Code = "10004003",
            //    RoleAccess = "User",
            //});
            //context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "مدیریت خدمت گیرنده ",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "10002007").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = ParentServiceProvider,
            //    Code = "10002007",
            //    RoleAccess = "User"
            //});
            //context.SaveChanges();

            ////-----------------------admin ServiceCode :5005-------------------------------------

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "مدیریت خدمات مدیر ",
            //    ActionId = 1,
            //    IsEnable = true,
            //    ParentId = 0,
            //    Code = "5005",
            //    RoleAccess = "Admin,Administrator,Modrator",
            //    IconName = ""
            //});
            //context.SaveChanges();

            //int ParentadminServices = context.Menus.FirstOrDefault(c => c.Code == "5005").Id;

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "خدمات غیر اتوماسیون ",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "10005001").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = ParentadminServices,
            //    Code = "10005001",
            //    RoleAccess = "Admin,Administrator,Modrator",
            //});
            //context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "خدمات غیر اتوماسیون و قطعی ",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "10005002").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = ParentadminServices,
            //    Code = "10005002",
            //    RoleAccess = "Admin,Administrator,Modrator",
            //});
            //context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "خدمات اتوماسیون ",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "10005003").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = ParentadminServices,
            //    Code = "10005003",
            //    RoleAccess = "Admin,Administrator,Modrator",
            //});
            //context.SaveChanges();

            ////----------------------
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "اطلاعات پایه کاربران ",
            //    ActionId = 1,
            //    IsEnable = true,
            //    ParentId = 0,
            //    Code = "5006",
            //    RoleAccess = "User",
            //    IconName = ""
            //});
            //context.SaveChanges();

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = " اطلاعات تکمیلی خدمت دهنده",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "10002004").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = context.Menus.FirstOrDefault(c => c.Code == "5006").Id,
            //    Code = "10002004",
            //    RoleAccess = "User"
            //});
            //context.SaveChanges();


            #endregion User

            #region Acces List
            //------------------------ سطوح دسترسی Access Range Code : 2001 -------------------------------------
            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "سطوح دسترسي",
                ActionId = context.Actions.Where(c => c.AccessCode == "2001").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = 0,
                Code = "2001",
                RoleAccess = "Admin,Administrator",
                IconName = "fa-key",
                Sort = 4,
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "تعريف گروه هاي دسترسي",
                ActionId = context.Actions.Where(c => c.AccessCode == "2001001").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = context.Menus.Where(c => c.Code == "2001").FirstOrDefault().Id,
                Code = "2001001",
                RoleAccess = "Admin,Administrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "تعیین دسترسی گروه  ",
                ActionId = context.Actions.Where(c => c.AccessCode == "2001002").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = context.Menus.Where(c => c.Code == "2001").FirstOrDefault().Id,
                Code = "2001002",
                RoleAccess = "Admin,Administrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "تعیین دسترسی کاربر  ",
                ActionId = context.Actions.Where(c => c.AccessCode == "2001003").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = context.Menus.Where(c => c.Code == "2001").FirstOrDefault().Id,
                Code = "2001001",
                RoleAccess = "Admin,Administrator"
            });
            context.SaveChanges();

            #endregion Acces List

            #region Framework : Message , Setting

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "مدیریت پیام ها",
                ActionId = 1,
                IsEnable = true,
                ParentId = 0,
                Code = "10008",
                RoleAccess = "Admin,Administrator,Modrator,User",
                IconName = "fa-envelope",
                Sort = 8,
            });
            context.SaveChanges();
            int MessageData = context.Menus.FirstOrDefault(c => c.Code == "10008").Id;

            context.Menus.AddOrUpdate(p => p.Name,
                new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
                {
                    Name = "ارسال پیام",
                    ActionId = context.Actions.Where(c => c.AccessCode == "10008001").FirstOrDefault().Id,
                    IsEnable = true,
                    ParentId = MessageData,
                    Code = "10008001",
                    RoleAccess = "Admin,Administrator,User,Modrator",
                    IconName = "fa-envelope",
                });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name,
                new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
                {
                    Name = "لیست پیام ها",
                    ActionId = context.Actions.Where(c => c.AccessCode == "10008002").FirstOrDefault().Id,
                    IsEnable = true,
                    ParentId = MessageData,
                    Code = "10008002",
                    RoleAccess = "Admin,Administrator,User,Modrator",
                    IconName = "fa-envelope",
                });
            context.SaveChanges();

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "تنظیمات",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "20003").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = 0,
            //    Code = "20003",
            //    RoleAccess = "Admin,Administrator",
            //    IconName = "fa-gear"
            //});
            //context.SaveChanges();

            #endregion

            #region Golbahar

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "ثبت اطلاعات ",
                ActionId = 1,
                IsEnable = true,
                ParentId = 0,
                Code = "20004",
                RoleAccess = "Admin,Administrator,Modrator",
                IconName = " fa-save",
                Sort = 5,
            });
            context.SaveChanges();
            int gols = context.Menus.FirstOrDefault(c => c.Code == "20004").Id;

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "ثبت اطلاعات از طریق اکسل",
                ActionId = context.Actions.Where(c => c.AccessCode == "20004001").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = gols,
                Code = "20004001",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "تنظیمات",
                ActionId = context.Actions.Where(c => c.AccessCode == "20004002").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = gols,
                Code = "20004002",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "درصد یا مبلغ افزایش سالیانه",
                ActionId = context.Actions.Where(c => c.AccessCode == "20004003").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = gols,
                Code = "20004003",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "درصد یا مبلغ جریمه",
                ActionId = context.Actions.Where(c => c.AccessCode == "20004004").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = gols,
                Code = "20004004",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "ثبت پرداختی توسط مدیر",
                ActionId = context.Actions.Where(c => c.AccessCode == "20004005").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = gols,
                Code = "20004005",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "درصد خوش حسابی",
                ActionId = context.Actions.Where(c => c.AccessCode == "20004006").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = gols,
                Code = "20004006",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "ثبت پرداختی",
                ActionId = context.Actions.Where(c => c.AccessCode == "20004005").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = 0,
                Code = "20004005",
                RoleAccess = "User",
                IconName = " fa-save",
                Sort = 7,

            });
            context.SaveChanges();


            //------------------

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "گزارشات ",
                ActionId = 1,
                IsEnable = true,
                ParentId = 0,
                Code = "20005",
                RoleAccess = "Admin,Administrator,User,Modrator",
                IconName = " fa-list-alt",
                Sort = 6,
            });
            context.SaveChanges();
            int gols2 = context.Menus.FirstOrDefault(c => c.Code == "20005").Id;

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "لیست شرکت ها",
                ActionId = context.Actions.Where(c => c.AccessCode == "20005001").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = gols2,
                Code = "20005001",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "لیست مجتمع ها",
                ActionId = context.Actions.Where(c => c.AccessCode == "20005002").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = gols2,
                Code = "20005002",
                RoleAccess = "Admin,Administrato,Modrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "لیست بلوک ها",
                ActionId = context.Actions.Where(c => c.AccessCode == "20005003").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = gols2,
                Code = "20005003",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "لیست واحد ها",
                ActionId = context.Actions.Where(c => c.AccessCode == "20005004").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = gols2,
                Code = "20005004",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "نمایش فیش اجاره ها",
                ActionId = context.Actions.Where(c => c.AccessCode == "20005005").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = gols2,
                Code = "20005005",
                RoleAccess = "User"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "نمایش تاریخچه فیش ها",
                ActionId = context.Actions.Where(c => c.AccessCode == "20005011").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = gols2,
                Code = "20005011",
                RoleAccess = "User"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "لیست پرداختی ها",
                ActionId = context.Actions.Where(c => c.AccessCode == "20005006").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = gols2,
                Code = "20005006",
                RoleAccess = "Admin,Administrator,User,Modrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "لیست گزارش بدهی ",
                ActionId = context.Actions.Where(c => c.AccessCode == "20005010").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = gols2,
                Code = "20005010",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            //-----------------------

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "گزارش نموداری ",
                ActionId = 1,
                IsEnable = true,
                ParentId = 0,
                Code = "20006",
                RoleAccess = "Admin,Administrator,Modrator",
                IconName = " fa-bar-chart-o",
                Sort = 6,
            });
            context.SaveChanges();
            int nemoodar = context.Menus.FirstOrDefault(c => c.Code == "20006").Id;

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "گزارش بدهی ها",
                ActionId = context.Actions.Where(c => c.AccessCode == "20005007").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = nemoodar,
                Code = "20005007",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "نوع پرداختی",
                ActionId = context.Actions.Where(c => c.AccessCode == "20005008").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = nemoodar,
                Code = "20005008",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "شرکت های فعال",
                ActionId = context.Actions.Where(c => c.AccessCode == "20005009").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = nemoodar,
                Code = "20005009",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            //--------------------

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "ارسال پیامک به اعضا",
                ActionId = context.Actions.Where(c => c.AccessCode == "20007").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = 0,
                Code = "20007",
                RoleAccess = "Admin,Administrator,Modrator",
                IconName = " fa-mobile",
                Sort = 7,
            });
            context.SaveChanges();


            #endregion

            #endregion menu

            //***********************************************************************************************************//

            //#region state and city 

            //context.States.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.BaseTable.State
            //{
            //    Name = "خراسان",
            //    IsEnable = true,
            //});
            //context.SaveChanges();
            //context.Cities.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.BaseTable.City
            //{
            //    Name = "مشهد",
            //    StateId = 1,
            //    IsEnable = true,
            //});
            //context.SaveChanges();
            //#endregion

            #region state and city && Service

            //context.WorkUnits.AddOrUpdate(p => p.Title, new PrivateTraining.DomainClasses.Entities.PrivateTraining.WorkUnit
            //{
            //    Title = "ساعتی",
            //    IsEnable = true,
            //});
            //context.SaveChanges();
            //context.WorkUnits.AddOrUpdate(p => p.Title, new PrivateTraining.DomainClasses.Entities.PrivateTraining.WorkUnit
            //{
            //    Title = "روزانه",
            //    IsEnable = true,
            //});
            //context.SaveChanges();
            ////context.ServicesProperties.AddOrUpdate(p => p.Title, new PrivateTraining.DomainClasses.Entities.PrivateTraining.ServiceProperties
            ////{
            ////    Title = "خدمت1",
            ////    ParentId = 0,
            ////    Level = 1,
            ////    ServiceCode = "111",
            ////    MinCapacity = 10,
            ////    MaxCapacity = 60,
            ////    PercentCountReservation = 30,
            ////    PriceRegisterServiceProvider = 200000,
            ////    CapacityServiceProvider = 20,
            ////    PercentOfShares = 10,
            ////    IsEnable = true,
            ////});
            ////context.SaveChanges();
            ////context.ServicesProperties.AddOrUpdate(p => p.Title, new PrivateTraining.DomainClasses.Entities.PrivateTraining.ServiceProperties
            ////{
            ////    Title = "خدمت2",
            ////    ParentId = 0,
            ////    Level = 1,
            ////    ServiceCode = "222",
            ////    MinCapacity = 11,
            ////    MaxCapacity = 55,
            ////    PercentCountReservation = 10,
            ////    PriceRegisterServiceProvider = 3500000,
            ////    CapacityServiceProvider = 12,
            ////    PercentOfShares = 14,
            ////    IsEnable = true,
            ////});
            ////context.SaveChanges();
            ////context.ServicesProperties.AddOrUpdate(p => p.Title, new PrivateTraining.DomainClasses.Entities.PrivateTraining.ServiceProperties
            ////{
            ////    Title = "خدمت3",
            ////    ParentId = 1,
            ////    Level = 2,
            ////    ServiceCode = "333",
            ////    MinCapacity = 9,
            ////    MaxCapacity = 45,
            ////    PercentCountReservation = 12,
            ////    PriceRegisterServiceProvider = 250000,
            ////    CapacityServiceProvider = 14,
            ////    PercentOfShares = 23,
            ////    IsEnable = true,
            ////});
            ////context.SaveChanges();
            ////context.Locations.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.PrivateTraining.Location
            ////{
            ////    Name = "احمدآباد",
            ////    CityId = 1,
            ////    LocationCode = "123",
            ////    IsEnable = true,
            ////});
            ////context.SaveChanges();
            #endregion

            #region Year 
            for (int i = 1394; i < 1401; i++)
            {
                context.Years.AddOrUpdate(p => p.Title, new PrivateTraining.DomainClasses.Entities.BusDriving.Year
                {
                    Title = i.ToString(),
                    IsEnable = true,
                });
            }
            #endregion

            #region setting

            //context.Settings.AddOrUpdate(p => p.Subject, new PrivateTraining.DomainClasses.Entities.FrameWork.Setting
            //{
            //    Id = 1,
            //    Subject = "Email",
            //    Value1 = "main@mega-tech.ir",
            //    Value2 = "mega123",
            //    Value3 = "smtp.mega-tech.ir",
            //    Value4 = "mail.mega-tech.ir",

            //});
            //context.SaveChanges();

            #endregion

            #region Define Store Procedure with Sql Query


            context.Database.ExecuteSqlCommand(
   "IF EXISTS ( SELECT * FROM   sysobjects  WHERE  id = object_id(N'[dbo].[ListAccessUsers]') " +
   "and OBJECTPROPERTY(id, N'IsProcedure') = 1 ) BEGIN DROP PROCEDURE [dbo].[ListAccessUsers] END   "

   );
            context.Database.ExecuteSqlCommand("create procedure [dbo].[ListAccessUsers] " +
   "( @UserId int)  as " +
   "( SELECT Framework.Actions.*, Security.AccessLevels.ActionId as Id,Framework.Actions.Name as Name " +
   "FROM    Security.GroupPolicyUsers INNER JOIN  Security.AccessLevelGroups " +
   "ON Security.GroupPolicyUsers.GroupPolicyId = Security.AccessLevelGroups.GroupId INNER JOIN " +
   "Security.AccessLevels ON Security.AccessLevelGroups.Id = Security.AccessLevels.Id INNER JOIN " +
   "Framework.Actions ON Security.AccessLevels.ActionId=Framework.Actions.Id WHERE  " +
   " (Security.GroupPolicyUsers.UserId = @UserId) and Security.AccessLevels.IsEnable=1   and Security.AccessLevels.ActionId not in (SELECT  Security.AccessLevels.ActionId   FROM  Security.AccessLevelUsers INNER JOIN " +
   " Security.AccessLevels ON Security.AccessLevelUsers.Id = Security.AccessLevels.Id " +
   " INNER JOIN Framework.Actions ON Security.AccessLevels.ActionId = Framework.Actions.Id WHERE " +
   " Security.AccessLevels.IsEnable = 0 and Security.AccessLevelUsers.UserId = @UserId) ) union " +
   " (SELECT  Framework.Actions.*,Security.AccessLevels.ActionId as Id ,Framework.Actions.Name as Name  FROM " +
   " Security.AccessLevelUsers INNER JOIN Security.AccessLevels ON Security.AccessLevelUsers.Id = " +
   " Security.AccessLevels.Id INNER JOIN Framework.Actions ON Security.AccessLevels.ActionId=Framework.Actions.Id WHERE" +
   " (Security.AccessLevelUsers.UserId = @UserId) and Security.AccessLevels.IsEnable=1 ) ");

            //Defined Stored procedure Payment and peymentDetails


            context.Database.ExecuteSqlCommand(
              "IF EXISTS ( SELECT * FROM   sysobjects  WHERE  id = object_id(N'[PrivateTraining].[SP_CalculationPayment]') " +
              " and OBJECTPROPERTY(id, N'IsProcedure') = 1 ) BEGIN DROP PROCEDURE [PrivateTraining].[SP_CalculationPayment] END   "

              );
            context.Database.ExecuteSqlCommand("create procedure [PrivateTraining].[SP_CalculationPayment] " +
             " @MemberId as int, @ModratorId as int, @TransactionNumber as nvarchar(100), @StatusPayment as tinyint, @ListDebtId AS varchar(MAX)" +
             " AS BEGIN" +
             " declare @Id as numeric(18, 0)" +
             " DECLARE @IsEnable bit " +
             " declare @Date as varchar(20)" +
             " declare @Time as varchar(8)" +
             " declare @CompanyCost as int" +
             " select @CompanyCost=sum(CompanyCost) from PrivateTraining.Debts where Id in  (select item from[dbo].[FN_ConvertStringToTable](@ListDebtId, ','))" +

             " SET @IsEnable = 'true'; " +
             " set @Date = dbo.FarsiDate(getdate())" +
             " set @Time = CONVERT(VARCHAR(8), GETDATE(), 108) " +
             " insert into PrivateTraining.payments " +
             " (Price,[Date],[Time], TransactionNumber,[Status], MemberId, IsEnable, ModratorId) " +
             " values(@CompanyCost, @Date, @Time, @TransactionNumber, @StatusPayment, @MemberId, @IsEnable, @ModratorId) " +

             " SELECT @Id =@@IDENTITY " +
             " exec[PrivateTraining].[Sp_CalculationPaymentDetail] @Id, @ListDebtId;" +
             " END"
             );


            //**************************************************************

            context.Database.ExecuteSqlCommand(
              "IF EXISTS ( SELECT * FROM   sysobjects  WHERE  id = object_id(N'[PrivateTraining].[Sp_CalculationPaymentDetail]') " +
              "and OBJECTPROPERTY(id, N'IsProcedure') = 1 ) BEGIN DROP PROCEDURE [PrivateTraining].[Sp_CalculationPaymentDetail] END   "

              );


            context.Database.ExecuteSqlCommand("create procedure [PrivateTraining].[Sp_CalculationPaymentDetail] " +
             " @paymentId as Int, @ListDebtId AS varchar(MAX)" +
             " AS BEGIN" +
             " delete from paymentDetails where paymentId = @paymentId" +
             " declare  @CalculatePricePayment as int" +
             " declare  @MemberId as int " +
             " declare  @ModratorId as int" +
             " select @CalculatePricePayment = Price, @MemberId = MemberId, @ModratorId = ModratorId" +
             " from [PrivateTraining].[payments]" +
             " where Id = @paymentId" +
             " DECLARE @Id int; DECLARE @CompanyCost int;  DECLARE @TotalCost int; DECLARE @PercentOfShares int; DECLARE @ServiceReceiverServiceLocationId int;  DECLARE @ServiceProviderId  int; DECLARE @IsEnable bit; SET @IsEnable = 'true'; " +
             " DECLARE vendor_cursor CURSOR FOR " +
             " select D.Id as Id_x , " +
                      " D.CompanyCost as CompanyCost_x, " +
                      " D.TotalCost as TotalCost_x," +
                      " D.PercentOfShares as PercentOfShares_x, " +
                      " DS.ServiceReceiverServiceLocationId, " +
                      " DSP.ServiceProviderId" +
             " from PrivateTraining.Debts as D" +
                " left JOIN PrivateTraining.DebtServiceReceiverServiceLocations as DS  ON D.Id = DS.Id" +
                " left JOIN PrivateTraining.DebtServiceProviders as DSP  ON D.Id = DSP.Id" +
             " where D.Id in (select item from[dbo].[FN_ConvertStringToTable](@ListDebtId, ','))" +

             " OPEN vendor_cursor" +
             " FETCH NEXT FROM vendor_cursor" +
             " INTO @Id, @CompanyCost, @TotalCost, @PercentOfShares, @ServiceReceiverServiceLocationId, @ServiceProviderId" +
             " WHILE @@FETCH_STATUS = 0  BEGIN" +
             " insert into[PrivateTraining].[paymentDetails] (CompanyCostDebt,[Status],IsEnable,DebtId,paymentId,ServiceProviderId,ServiceReceiverServiceLocationId,CalculatePricePayment,MemberId,ModratorId,TotalCostDebt,PercentOfSharesDebt)" +
             " values(@CompanyCost,0, @IsEnable, @Id, @paymentId, @ServiceProviderId, @ServiceReceiverServiceLocationId, @CalculatePricePayment, @MemberId, @ModratorId, @TotalCost, @PercentOfShares)" +
             " FETCH NEXT FROM vendor_cursor" +
             " INTO @Id, @CompanyCost, @TotalCost, @PercentOfShares, @ServiceReceiverServiceLocationId, @ServiceProviderId" +
             " END" +
             " CLOSE vendor_cursor;" +
             " DEALLOCATE vendor_cursor;" +
             " END"
             );

            #endregion

            #region function sql (FN_ConvertStringToTable &  FarsiDate)

            //************************** FN_ConvertStringToTable ***************************************
            context.Database.ExecuteSqlCommand(
              "IF OBJECT_ID('dbo.FN_ConvertStringToTable') IS NOT NULL  DROP FUNCTION dbo.FN_ConvertStringToTable "
              );

            context.Database.ExecuteSqlCommand("CREATE FUNCTION[dbo].[FN_ConvertStringToTable] " +
             " ( @array varchar(max),    @del char(1) = ',' ) " +
             " RETURNS @listTable TABLE (   item varchar(max) ) " +
             " AS BEGIN " +
             " IF (CHARINDEX(@Del,@Array)=0)  BEGIN  Insert Into @ListTable(Item) VALUES(@Array)   RETURN  END " +
             " IF Isnull(@Del,'')='' Begin Set @Del = ',' End " +
             " DECLARE @Loop1 NUMERIC SET @Loop1 = 1   DECLARE @Temp VARCHAR(MAX)   SET @Temp = '' " +
             " WHILE (@Loop1<=Len(@Array))  BEGIN IF(SubString(@Array, @Loop1, 1) <> @Del)  BEGIN  SET @Temp = @Temp + SubString(@Array, @Loop1, 1)  END  ELSE  BEGIN  INSERT INTO @listTable  SELECT @Temp SET @Temp = ''  END  SET @Loop1 = @Loop1 + 1    END" +
             " If (ISNULL(@Temp,'')<>'')   BEGIN  INSERT INTO @listTable  SELECT @Temp END RETURN " +
             " END");

            //***************************** FarsiDate ***************************************************

            context.Database.ExecuteSqlCommand(
              "IF OBJECT_ID('dbo.FarsiDate') IS NOT NULL  DROP FUNCTION dbo.FarsiDate "
              );

            context.Database.ExecuteSqlCommand("CREATE FUNCTION[dbo].[FarsiDate] (@EnglishDate DateTime) " +
             " RETURNS nvarchar(10) " +
              " AS BEGIN " +
              " Declare @EYear Numeric,@EMonth Numeric, @EDay Numeric " +
              " Declare @FYear Numeric,@FMonth Numeric, @FDay Numeric " +
              " Declare @M1 Int,@M2 Int,@D1 Int,@D2 Int,@Counter Int " +
              " Declare @Kabflag bit " +
              " Declare @FarsiDate nvarchar(10) " +
              " SET @EYear=DatePart(yyyy,@EnglishDate) " +
              " SET @EMonth=DatePart(mm,@EnglishDate) " +
              " SET @EDay=DatePart(dd,@EnglishDate) " +
              " SET @D1=0  SET @M1 = 0  SET @M2 = 0  SET @Counter = 0 " +
              " while ((1900 + 4*@Counter)< @EYear) " +
              " SET @Counter=@Counter+1 " +
              " if ((((1900+4*@Counter)=@EYear) and ((@EMonth>2) or ((@EMonth=2) and (@EDay=29)))) or (((1901+4*(@Counter-1))=@EYear) and ((@EMonth<3) or ((@EMonth=3) and(@EDay <= 20))))) " +
              " begin " +
              "  SET @EYear=DatePart(yyyy,@EnglishDate+1) " +
              "  SET @EMonth=DatePart(mm,@EnglishDate+1) " +
              " SET @EDay=DatePart(dd,@EnglishDate+1) " +
              "  if (((1901+4*(@Counter-1))=@EYear) and ((@EMonth<3) or ((@EMonth=3) and (@EDay<=21))))  " +
              "  begin  Set @KabFlag = 1  end end" +
              "  if (((@EMonth>=3) and ((@EDay>21) or ((@EDay=21) and (@KabFlag=0)))) or (@EMonth >3)) SET @FYear=@EYear - 621" +
              "  else  SET @FYear = @EYear - 622" +
              "  if (@EMonth = 1)  begin  SET @M1 = 9  SET @D1 = 10  SET @M2 = 10 end" +
              "  if (@EMonth = 3)  begin  SET @M1 = 9  SET @D1 = 9  SET @M2 = -2 end" +
              "  if (@EMonth = 4)  begin  SET @M1 = -3  SET @D1 = 11  SET @M2 = -2 end" +
              "  if (@EMonth=1) or (@EMonth=3) or (@EMonth=4) begin  if ((@EDay < 21) or(@KabFlag = 1))  begin SET @FMonth = @EMonth + @M1  SET @FDay = @EDay + @D1  end  else  begin SET @FMonth = @EMonth + @M2  SET @FDay = @EDay - 20 end end" +
              "  if (@EMonth=2) begin if (@EDay < 20) begin SET @FMonth = @EMonth + 9 SET @FDay = @EDay + 11 end else begin SET @FMonth = @EMonth + 10 SET @FDay = @EDay - 19 end end" +
              "  if (@EMonth>=7) and (@EMonth<=10) begin if (@EMonth = 10)   SET @D1 = 8 else SET @D1 = 9 if (@EDay < 23)  begin   SET @FMonth = @EMonth - 3  SET @FDay = @EDay + @D1  end else begin SET @FMonth = @EMonth - 2  SET @FDay = @EDay - 22 end end" +
              "  if (@EMonth=5) or (@EMonth=6) or (@EMonth=11) or (@EMonth=12) begin if ((@EMonth = 5) or(@EMonth = 6))  SET @D2 = 10 else SET @D2 = 9 if (@EDay < 22)   begin    SET @FMonth = @EMonth - 3 SET @FDay = @EDay + @D2 end else begin SET @FMonth = @EMonth - 2 SET @FDay = @EDay - 21 end end" +
              "  if (@FMonth>=1) and (@FMonth<=6) begin if (@FDay > 31)   begin  SET @FDay = @FDay - 31 SET @FMonth = @FMonth + 1;  end   end" +
              "  else if (@FMonth >= 7) and(@FMonth <= 11) begin if (@FDay > 30)  begin  SET @FDay = @FDay - 30 SET @FMonth = @FMonth + 1;  end  end" +
              "  else if (@FMonth = 12)  begin if (((@FDay > 29) and((1901 + 4 * (@Counter - 1)) <> @EYear)) or((@FDay > 30) and((1901 + 4 * (@Counter - 1)) = @EYear)))  begin SET @FDay = @FDay - 29 SET @FMonth = 1 SET @FYear = @FYear + 1 end;  end" +
              "  if (@FMonth<10) and (@FDay<10)  SET @FarsiDate = Cast(@FYear As nvarchar(4)) + '/0' + Cast(@FMonth As nvarchar(1)) + '/0' + Cast(@FDay As nvarchar(1))" +
              "  if (@FMonth>=10) and (@FDay<10)  SET @FarsiDate = Cast(@FYear As nvarchar(4)) + '/' + Cast(@FMonth As nvarchar(2)) + '/0' + Cast(@FDay As nvarchar(1))" +
              "  if (@FMonth<10) and (@FDay>=10)  SET @FarsiDate = Cast(@FYear As nvarchar(4)) + '/0' + Cast(@FMonth As nvarchar(1)) + '/' + Cast(@FDay As nvarchar(2))" +
              "  if (@FMonth>=10) and (@FDay>=10)  SET @FarsiDate = Cast(@FYear As nvarchar(4)) + '/' + Cast(@FMonth As nvarchar(2)) + '/' + Cast(@FDay As nvarchar(2))" +
              "  Return @FarsiDate" +
             " END");


            #endregion

            #region Actions old (Toos)

            //#region Lines

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    //Id = 2,
            //    Name = "مدیریت خطوط",
            //    Actionname = "Index",
            //    Controller = "Line",
            //    Area = "BusDriving",
            //    ParentId = 0,
            //    IsEnable = true,
            //    ShowName = "مدیریت خطوط",
            //    AccessCode = "3001"
            //});
            //context.SaveChanges();
            //int LineId = context.Actions.FirstOrDefault(c => c.AccessCode == "3001").Id;
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    //Id = 2,
            //    Name = "صفحه تعریف خط ها",
            //    Actionname = "Index",
            //    Controller = "Line",
            //    Area = "BusDriving",
            //    ParentId = LineId,
            //    IsEnable = true,
            //    ShowName = "صفحه تعریف خط ها",
            //    AccessCode = "3001001"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "لیست خط ها",
            //    Actionname = "ListLines",
            //    Controller = "Line",
            //    Area = "BusDriving",
            //    ParentId = LineId,
            //    IsEnable = true,
            //    ShowName = "لیست خط ها",
            //    AccessCode = "3001002"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    //Id = 2,
            //    Name = "افزودن خطوط",
            //    Actionname = "AddLine",
            //    Controller = "Line",
            //    Area = "BusDriving",
            //    ParentId = LineId,
            //    IsEnable = true,
            //    ShowName = "افزودن خطوط",
            //    AccessCode = "3001003"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    // Id = 2,
            //    Name = "حذف خطوط",
            //    Actionname = "DeleteLine",
            //    Controller = "Line",
            //    Area = "BusDriving",
            //    ParentId = LineId,
            //    IsEnable = true,
            //    ShowName = "حذف خطوط",
            //    AccessCode = "3001004"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    // Id = 2,
            //    Name = "ویرایش خطوط",
            //    Actionname = "EditLine",
            //    Controller = "Line",
            //    Area = "BusDriving",
            //    ParentId = LineId,
            //    IsEnable = true,
            //    ShowName = "ویرایش خطوط",
            //    AccessCode = "3001005"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    // Id = 2,
            //    Name = "لود اطلاعات برای ویرایش خطوط",
            //    Actionname = "LoadEdit",
            //    Controller = "Line",
            //    Area = "BusDriving",
            //    ParentId = LineId,
            //    IsEnable = true,
            //    ShowName = "لود اطلاعات برای ویرایش خطوط",
            //    AccessCode = "3001006"
            //});
            //context.SaveChanges();
            //#endregion Lines

            //#region Shift

            ////----------------------- Access Code 3002  ------------------------------------------------
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "مدیریت شیفت ها",
            //    Actionname = "Index",
            //    Controller = "Shift",
            //    Area = "BusDriving",
            //    ParentId = 0,
            //    IsEnable = true,
            //    ShowName = "مدیریت شیفت ها",
            //    AccessCode = "3002"
            //});
            //context.SaveChanges();
            //int ShiftId = context.Actions.FirstOrDefault(c => c.AccessCode == "3002").Id;
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "صفحه مدیریت شیفت ها",
            //    Actionname = "Index",
            //    Controller = "Shift",
            //    Area = "BusDriving",
            //    ParentId = ShiftId,
            //    IsEnable = true,
            //    ShowName = "صفحه مدیریت شیفت ها",
            //    AccessCode = "3002001"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "لیست شیفت ها",
            //    Actionname = "ListShifts",
            //    Controller = "Shift",
            //    Area = "BusDriving",
            //    ParentId = ShiftId,
            //    IsEnable = true,
            //    ShowName = "لیست شیفت ها",
            //    AccessCode = "3002002"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "افزودن شیفت ها",
            //    Actionname = "AddShift",
            //    Controller = "Shift",
            //    Area = "BusDriving",
            //    ParentId = ShiftId,
            //    IsEnable = true,
            //    ShowName = "افزودن شیفت ها",
            //    AccessCode = "3002003"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "حذف شیفت ها",
            //    Actionname = "DeleteShift",
            //    Controller = "Shift",
            //    Area = "BusDriving",
            //    ParentId = ShiftId,
            //    IsEnable = true,
            //    ShowName = "حذف شیفت ها",
            //    AccessCode = "3002004"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "ویرایش شیفت ها",
            //    Actionname = "EditShift",
            //    Controller = "Shift",
            //    Area = "BusDriving",
            //    ParentId = ShiftId,
            //    IsEnable = true,
            //    ShowName = "ویرایش شیفت ها",
            //    AccessCode = "3002005"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "لود اطلاعات برای ویرایش شیفت ها",
            //    Actionname = "LoadEdit",
            //    Controller = "Shift",
            //    Area = "BusDriving",
            //    ParentId = ShiftId,
            //    IsEnable = true,
            //    ShowName = "لود اطلاعات برای ویرایش شیفت ها",
            //    AccessCode = "3002006"
            //});
            //context.SaveChanges();
            ////----------------------- Access Code 3002  ------------------------------------------------
            //#endregion Shift

            //#region Max Leave
            ////----------------------- Access Code 3003  ------------------------------------------------
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "مدیریت سقف تعداد مرخصی ها",
            //    Actionname = "Index",
            //    Controller = "MaximumLeave",
            //    Area = "BusDriving",
            //    ParentId = 0,
            //    IsEnable = true,
            //    ShowName = "مدیریت سقف تعداد مرخصی ها",
            //    AccessCode = "3003"
            //});
            //context.SaveChanges();
            //int MaxLeaveId = context.Actions.FirstOrDefault(c => c.AccessCode == "3003").Id;

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "صفحه سقف مرخصی ها",
            //    Actionname = "Index",
            //    Controller = "MaximumLeave",
            //    Area = "BusDriving",
            //    ParentId = MaxLeaveId,
            //    IsEnable = true,
            //    ShowName = "صفحه سقف مرخصی ها",
            //    AccessCode = "3003001"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "لیست سقف مرخصی ها",
            //    Actionname = "ListMaximumLeaves",
            //    Controller = "MaximumLeave",
            //    Area = "BusDriving",
            //    ParentId = MaxLeaveId,
            //    IsEnable = true,
            //    ShowName = "لیست سقف مرخصی ها",
            //    AccessCode = "3003002"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "افزودن سقف مرخصی ها برای تمامی خط و شیفت ها",
            //    Actionname = "AddMaximumLeave",
            //    Controller = "MaximumLeave",
            //    Area = "BusDriving",
            //    ParentId = MaxLeaveId,
            //    IsEnable = true,
            //    ShowName = "افزودن سقف مرخصی ها برای تمامی خط و شیفت ها",
            //    AccessCode = "3003003"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = " افزودن سقف مرخصی ها برای خط و شیفت خاص",
            //    Actionname = "AddMaximumLeaveLine",
            //    Controller = "MaximumLeave",
            //    Area = "BusDriving",
            //    ParentId = MaxLeaveId,
            //    IsEnable = true,
            //    ShowName = " افزودن سقف مرخصی ها برای خط و شیفت خاص",
            //    AccessCode = "3003004"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "حذف سقف مرخصی",
            //    Actionname = "DeleteMaximumLeave",
            //    Controller = "MaximumLeave",
            //    Area = "BusDriving",
            //    ParentId = MaxLeaveId,
            //    IsEnable = true,
            //    ShowName = "حذف سقف مرخصی",
            //    AccessCode = "3003005"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "ویرایش سقف مرخصی",
            //    Actionname = "EditMaximumLeave",
            //    Controller = "MaximumLeave",
            //    Area = "BusDriving",
            //    ParentId = MaxLeaveId,
            //    IsEnable = true,
            //    ShowName = "ویرایش سقف مرخصی",
            //    AccessCode = "3003006"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "لود اطلاعات برای ویرایش سقف مرخصی",
            //    Actionname = "LoadEdit",
            //    Controller = "MaximumLeave",
            //    Area = "BusDriving",
            //    ParentId = MaxLeaveId,
            //    IsEnable = true,
            //    ShowName = " لود اطلاعات برای ویرایش سقف مرخصی",
            //    AccessCode = "3003007"
            //});
            //context.SaveChanges();

            ////----------------------- Access Code 3003  ------------------------------------------------
            //#endregion Max Leave

            //#region Invalid Day
            ////----------------------- Access Code 3004  ------------------------------------------------
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "مدیریت روز های نامعتبر",
            //    Actionname = "Index",
            //    Controller = "InvalidDay",
            //    Area = "BusDriving",
            //    ParentId = 0,
            //    IsEnable = true,
            //    ShowName = "مدیریت روز های نامعتبر",
            //    AccessCode = "3004"
            //});
            //context.SaveChanges();
            //int InvalidDayId = context.Actions.FirstOrDefault(c => c.AccessCode == "3004").Id;

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "صفحه روزهای نامعتبر",
            //    Actionname = "Index",
            //    Controller = "InvalidDay",
            //    Area = "BusDriving",
            //    ParentId = InvalidDayId,
            //    IsEnable = true,
            //    ShowName = "صفحه روزهای نامعتبر",
            //    AccessCode = "3004001"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "لیست روزهای نامعتبر",
            //    Actionname = "ListInvalidDays",
            //    Controller = "InvalidDay",
            //    Area = "BusDriving",
            //    ParentId = InvalidDayId,
            //    IsEnable = true,
            //    ShowName = "لیست روزهای نامعتبر",
            //    AccessCode = "3004002"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "افزودن روزهای نامعتبر",
            //    Actionname = "AddInvalidDay",
            //    Controller = "InvalidDay",
            //    Area = "BusDriving",
            //    ParentId = InvalidDayId,
            //    IsEnable = true,
            //    ShowName = "افزودن روزهای نامعتبر",
            //    AccessCode = "3004003"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "افزودن روزهای نامعتبر",
            //    Actionname = "AddInvalidDay",
            //    Controller = "InvalidDay",
            //    Area = "BusDriving",
            //    ParentId = InvalidDayId,
            //    IsEnable = true,
            //    ShowName = "افزودن روزهای نامعتبر",
            //    AccessCode = "3004004"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "افزودن روزهای نامعتبر به خط و شیفت خاص",
            //    Actionname = "AddInvalidDayLine",
            //    Controller = "InvalidDay",
            //    Area = "BusDriving",
            //    ParentId = InvalidDayId,
            //    IsEnable = true,
            //    ShowName = "افزودن روزهای نامعتبر به خط و شیفت خاص",
            //    AccessCode = "3004005"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "حذف روزهای نامعتبر",
            //    Actionname = "DeleteInvalidDay",
            //    Controller = "InvalidDay",
            //    Area = "BusDriving",
            //    ParentId = InvalidDayId,
            //    IsEnable = true,
            //    ShowName = "حذف روزهای نامعتبر",
            //    AccessCode = "3004006"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "ویرایش روزهای نامعتبر",
            //    Actionname = "EditInvalidDay",
            //    Controller = "InvalidDay",
            //    Area = "BusDriving",
            //    ParentId = InvalidDayId,
            //    IsEnable = true,
            //    ShowName = "ویرایش روزهای نامعتبر",
            //    AccessCode = "3004007"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "لود اطلاعات برای ویرایش روزهای نامعتبر",
            //    Actionname = "LoadEdit",
            //    Controller = "InvalidDay",
            //    Area = "BusDriving",
            //    ParentId = InvalidDayId,
            //    IsEnable = true,
            //    ShowName = "لود اطلاعات برای ویرایش روزهای نامعتبر",
            //    AccessCode = "3004008"
            //});
            //context.SaveChanges();

            ////----------------------- Access Code 3004  ------------------------------------------------
            //#endregion Max Leave

            //#region Leave Request 
            ////----------------------- Access Code 3005  ------------------------------------------------
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "مدیریت درخواست مرخصی",
            //    Actionname = "Index",
            //    Controller = "LeaveRequest",
            //    Area = "BusDriving",
            //    ParentId = 0,
            //    IsEnable = true,
            //    ShowName = "مدیریت درخواست مرخصی",
            //    AccessCode = "3005"
            //});
            //context.SaveChanges();
            //int LeaveRequestId = context.Actions.FirstOrDefault(c => c.AccessCode == "3005").Id;

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "صفحه درخواست مرخصی",
            //    Actionname = "Index",
            //    Controller = "LeaveRequest",
            //    Area = "BusDriving",
            //    ParentId = LeaveRequestId,
            //    IsEnable = true,
            //    ShowName = "صفحه درخواست مرخصی",
            //    AccessCode = "3005001"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "لیست روزهای مجاز برای درخواست مرخصی",
            //    Actionname = "ListRequestDays",
            //    Controller = "LeaveRequest",
            //    Area = "BusDriving",
            //    ParentId = LeaveRequestId,
            //    IsEnable = true,
            //    ShowName = "لیست روزهای مجاز برای درخواست مرخصی",
            //    AccessCode = "3005002"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "افزون درخواست مرخصی",
            //    Actionname = "AddLeaveRequest",
            //    Controller = "LeaveRequest",
            //    Area = "BusDriving",
            //    ParentId = LeaveRequestId,
            //    IsEnable = true,
            //    ShowName = "افزودن درخواست مرخصی",
            //    AccessCode = "3005003"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "حذف درخواست مرخصی",
            //    Actionname = "DeleteLeaveRequest",
            //    Controller = "LeaveRequest",
            //    Area = "BusDriving",
            //    ParentId = LeaveRequestId,
            //    IsEnable = true,
            //    ShowName = "حذف درخواست مرخصی",
            //    AccessCode = "3005004"
            //});
            //context.SaveChanges();

            ////----------------------- Access Code 3005  ------------------------------------------------
            //#endregion Leave Request 

            //#region Leave
            ////----------------------- Access Code 3006  ------------------------------------------------
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "مدیریت مرخصی های ثبت شده",
            //    Actionname = "Index",
            //    Controller = "Leave",
            //    Area = "BusDriving",
            //    ParentId = 0,
            //    IsEnable = true,
            //    ShowName = "مدیریت مرخصی های ثبت شده",
            //    AccessCode = "3006"
            //});
            //context.SaveChanges();
            //int LeaveId = context.Actions.FirstOrDefault(c => c.AccessCode == "3006").Id;
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "مدیریت مرخصی های ثبت شده",
            //    Actionname = "Index",
            //    Controller = "Leave",
            //    Area = "BusDriving",
            //    ParentId = LeaveId,
            //    IsEnable = true,
            //    ShowName = "مدیریت مرخصی های ثبت شده",
            //    AccessCode = "3006001"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "لیست مرخصی های ثبت شده",
            //    Actionname = "ListLeaves",
            //    Controller = "Leave",
            //    Area = "BusDriving",
            //    ParentId = LeaveId,
            //    IsEnable = true,
            //    ShowName = "لیست مرخصی های ثبت شده",
            //    AccessCode = "3006002"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "حذف مرخصی های ثبت شده",
            //    Actionname = "DeleteLeave",
            //    Controller = "Leave",
            //    Area = "BusDriving",
            //    ParentId = LeaveId,
            //    IsEnable = true,
            //    ShowName = "حذف مرخصی های ثبت شده",
            //    AccessCode = "3006003"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "تغییر وضعیت مرخصی ها",
            //    Actionname = "ChangeStatusLeave",
            //    Controller = "Leave",
            //    Area = "BusDriving",
            //    ParentId = LeaveId,
            //    IsEnable = true,
            //    ShowName = "تغییر وضعیت مرخصی ها ",
            //    AccessCode = "3006004"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "ارجاع مرخصی به مدیر دیگر",
            //    Actionname = "AddReferenceLeave",
            //    Controller = "Leave",
            //    Area = "BusDriving",
            //    ParentId = LeaveId,
            //    IsEnable = true,
            //    ShowName = "ارجاع مرخصی به مدیر دیگر",
            //    AccessCode = "3006005"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "مدیریت مرخصی های ارجاع داده شده",
            //    Actionname = "ReferenceLeaves",
            //    Controller = "Leave",
            //    Area = "BusDriving",
            //    ParentId = LeaveId,
            //    IsEnable = true,
            //    ShowName = "مدیریت مرخصی های ارجاع داده شده",
            //    AccessCode = "3006006"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "لیست مرخصی های ارجاع داده شده",
            //    Actionname = "ListReference",
            //    Controller = "Leave",
            //    Area = "BusDriving",
            //    ParentId = LeaveId,
            //    IsEnable = true,
            //    ShowName = "لیست مرخصی های ارجاع داده شده",
            //    AccessCode = "3006007"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "تغییر وضعیت مرخصی ارجاع داده شده",
            //    Actionname = "ChangeStatusLeaveRefrences",
            //    Controller = "Leave",
            //    Area = "BusDriving",
            //    ParentId = LeaveId,
            //    IsEnable = true,
            //    ShowName = "تغییر وضعیت مرخصی ارجاع داده شده",
            //    AccessCode = "3006008"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = " حذف مرخصی ارجاع داده شده",
            //    Actionname = "DeleteReference",
            //    Controller = "Leave",
            //    Area = "BusDriving",
            //    ParentId = LeaveId,
            //    IsEnable = true,
            //    ShowName = "حذف وضعیت مرخصی ارجاع داده شده",
            //    AccessCode = "3006009"
            //});
            //context.SaveChanges();
            ////----------------------- Access Code 3006  ------------------------------------------------
            //#endregion Leave

            //#region Period Leave
            ////----------------------- Access Code 3007  ------------------------------------------------
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "مدیریت بازه زمانی درخواست مرخصی",
            //    Actionname = "Index",
            //    Controller = "PeriodLeave",
            //    Area = "BusDriving",
            //    ParentId = 0,
            //    IsEnable = true,
            //    ShowName = "مدیریت بازه زمانی درخواست مرخصی",
            //    AccessCode = "3007"
            //});
            //context.SaveChanges();
            //int PeriodLeaveId = context.Actions.FirstOrDefault(c => c.AccessCode == "3007").Id;

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "مدیریت بازه زمانی درخواست مرخصی",
            //    Actionname = "Index",
            //    Controller = "PeriodLeave",
            //    Area = "BusDriving",
            //    ParentId = PeriodLeaveId,
            //    IsEnable = true,
            //    ShowName = "مدیریت بازه زمانی درخواست مرخصی",
            //    AccessCode = "3007001"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "ویرایش بازه زمانی درخواست مرخصی",
            //    Actionname = "EditPeriodLeave",
            //    Controller = "PeriodLeave",
            //    Area = "BusDriving",
            //    ParentId = PeriodLeaveId,
            //    IsEnable = true,
            //    ShowName = "ویرایش بازه زمانی درخواست مرخصی",
            //    AccessCode = "3007002"
            //});
            //context.SaveChanges();
            ////----------------------- Access Code 3007  ------------------------------------------------
            //#endregion Period Leave

            //#region User
            ////----------------------- Access Code 3008  ------------------------------------------------
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "مدیریت کاربران",
            //    Actionname = "Index",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = 0,
            //    IsEnable = true,
            //    ShowName = "مدیریت کاربران",
            //    AccessCode = "3008"
            //});
            //context.SaveChanges();
            //int UserId = context.Actions.FirstOrDefault(c => c.AccessCode == "3008").Id;

            ////---------- USER-------------

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = " لیست کاربران",
            //    Actionname = "ListUsers",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = "لیست کاربران",
            //    AccessCode = "3008001"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = " لیست کاربران به صورت ایجکس",
            //    Actionname = "ListJsonUsers",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = " لیست کاربران به صورت ایجکس",
            //    AccessCode = "3008002"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "حذف کاربر",
            //    Actionname = "DeleteUsers",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = " حذف کاربر",
            //    AccessCode = "3008003"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "غیر فعال کردن کاربر",
            //    Actionname = "InactiveUsers",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = " غیر فعال کردن کاربر",
            //    AccessCode = "3008004"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "تعلیق کاربر",
            //    Actionname = "SuspensionUsers",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = "تعلیق کاربر",
            //    AccessCode = "3008005"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "نمایش لیست تعلیقی های  کاربر",
            //    Actionname = "SuspensionList",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = "نمایش لیست تعلیقی های  کاربر",
            //    AccessCode = "3008006"
            //});
            //context.SaveChanges();

            ////---------- REGISTER-------------

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "صفحه ثبت نام کاربر",
            //    Actionname = "Register",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = "صفحه ثبت نام کاربر",
            //    AccessCode = "3008007"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = " ثبت نام توسط مدیر",
            //    Actionname = "Register",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = " ثبت نام توسط مدیر",
            //    AccessCode = "3008008"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "لود اطلاعات کاربر",
            //    Actionname = "SetUserInfo",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = "لود اطلاعات کاربر",
            //    AccessCode = "3008009"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "چک کردن شماره پرسنلی تکراری",
            //    Actionname = "CheckPersonnelId",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = "چک کردن شماره پرسنلی تکراری",
            //    AccessCode = "30080010"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "چک کردن  کد ملی تکراری",
            //    Actionname = "CheckNationalCode",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = "چک کردن  کد ملی تکراری",
            //    AccessCode = "30080011"
            //});
            //context.SaveChanges();

            ////---------- IMPORT EXCEL-------------

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = " صفحه آپلود فایل اکسل لیست کاربران ",
            //    Actionname = "ImportExcelUsers",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = "صفحه آپلود فایل اکسل لیست کاربران",
            //    AccessCode = "30080012"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "آپلود فایل اکسل لیست کاربران",
            //    Actionname = "UploadEcxel",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = "آپلود فایل اکسل لیست کاربران",
            //    AccessCode = "30080013"
            //});
            //context.SaveChanges();

            ////---------- Change Password-------------

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "تغییر رمز عبور",
            //    Actionname = "ChangePassword",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = "تغییر رمز عبور",
            //    AccessCode = "30080014"
            //});
            ////---------- OTHER -------------

            ////----------------------- Access Code 3008  ------------------------------------------------
            //#endregion User

            //#region salary
            ////----------------------- Access Code 3009  ------------------------------------------------
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "مدیریت فیش حقوقی",
            //    Actionname = "Index",
            //    Controller = "Salary",
            //    Area = "BusDriving",
            //    ParentId = 0,
            //    IsEnable = true,
            //    ShowName = "مدیریت فیش حقوقی",
            //    AccessCode = "3009"
            //});
            //context.SaveChanges();
            //int SalaryId = context.Actions.FirstOrDefault(c => c.AccessCode == "3009").Id;

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "صفحه فیش حقوقی",
            //    Actionname = "Index",
            //    Controller = "Salary",
            //    Area = "BusDriving",
            //    ParentId = SalaryId,
            //    IsEnable = true,
            //    ShowName = "صفحه فیش حقوقی",
            //    AccessCode = "3009001"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = " صفحه آپلود فایل اکسل فیش حقوقی",
            //    Actionname = "ImportExcelSalary",
            //    Controller = "Salary",
            //    Area = "BusDriving",
            //    ParentId = SalaryId,
            //    IsEnable = true,
            //    ShowName = "صفحه آپلود فایل اکسل فیش حقوقی",
            //    AccessCode = "3009002"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "آپلود فایل اکسل فیش حقوقی",
            //    Actionname = "ImportExcelSalary",
            //    Controller = "Salary",
            //    Area = "BusDriving",
            //    ParentId = SalaryId,
            //    IsEnable = true,
            //    ShowName = "آپلود فایل اکسل فیش حقوقی",
            //    AccessCode = "3009003"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "صفحه نمایش لیست فیش های کاربر لاگین شده",
            //    Actionname = "ShowSalaryOfUser",
            //    Controller = "Salary",
            //    Area = "BusDriving",
            //    ParentId = SalaryId,
            //    IsEnable = true,
            //    ShowName = "صفحه نمایش لیست فیش های کاربر لاگین شده",
            //    AccessCode = "3009004"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "تبدیل صفحه html به pdf",
            //    Actionname = "ConvertHtmlToPdf",
            //    Controller = "Salary",
            //    Area = "BusDriving",
            //    ParentId = SalaryId,
            //    IsEnable = true,
            //    ShowName = "تبدیل صفحه html به pdf",
            //    AccessCode = "3009005"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = " صفحه نمایش لیست فیش ها برای مدیر   ",
            //    Actionname = "ShowSalaryOfAdmin",
            //    Controller = "Salary",
            //    Area = "BusDriving",
            //    ParentId = SalaryId,
            //    IsEnable = true,
            //    ShowName = "صفحه نمایش لیست فیش ها برای مدیر",
            //    AccessCode = "3009006"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "نمایش لیست فیش ها برای مدیر",
            //    Actionname = "ListSalaryOfAdmin",
            //    Controller = "Salary",
            //    Area = "BusDriving",
            //    ParentId = SalaryId,
            //    IsEnable = true,
            //    ShowName = "نمایش لیست فیش ها برای مدیر",
            //    AccessCode = "3009007"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "نمایش  فیش ",
            //    Actionname = "ShowSalaryInfo",
            //    Controller = "Salary",
            //    Area = "BusDriving",
            //    ParentId = SalaryId,
            //    IsEnable = true,
            //    ShowName = "نمایش  فیش ",
            //    AccessCode = "3009008"
            //});
            //context.SaveChanges();
            ////----------------------- Access Code 3009  ------------------------------------------------
            //#endregion salary

            //#region Request User
            ////----------------------- Access Code 3010  ------------------------------------------------
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "مدیریت درخواست ها",
            //    Actionname = "ListUserRequest",
            //    Controller = "UserRequest",
            //    Area = "BusDriving",
            //    ParentId = 0,
            //    IsEnable = true,
            //    ShowName = "مدیریت درخواست ها",
            //    AccessCode = "3010"
            //});
            //context.SaveChanges();
            //int RequestId = context.Actions.FirstOrDefault(c => c.AccessCode == "3010").Id;
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "صفحه لیست درخواست ها",
            //    Actionname = "GetListUserRequest",
            //    Controller = "UserRequest",
            //    Area = "BusDriving",
            //    ParentId = RequestId,
            //    IsEnable = true,
            //    ShowName = "صفحه لیست درخواست ها",
            //    AccessCode = "3010001"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "لیست  درخواست های کاربر",
            //    Actionname = "ListUserRequests",
            //    Controller = "UserRequest",
            //    Area = "BusDriving",
            //    ParentId = RequestId,
            //    IsEnable = true,
            //    ShowName = "لیست  درخواست های کاربر",
            //    AccessCode = "3010002"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "صفحه ثبت درخواست",
            //    Actionname = "GetAddUserRequest",
            //    Controller = "UserRequest",
            //    Area = "BusDriving",
            //    ParentId = RequestId,
            //    IsEnable = true,
            //    ShowName = "صفحه ثبت درخواست",
            //    AccessCode = "3010003"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "افزودن  درخواست کاربر",
            //    Actionname = "AddUserRequest",
            //    Controller = "UserRequest",
            //    Area = "BusDriving",
            //    ParentId = RequestId,
            //    IsEnable = true,
            //    ShowName = "افزودن  درخواست کاربر",
            //    AccessCode = "3010004"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "حذف  درخواست کاربر",
            //    Actionname = "DeleteUserRequest",
            //    Controller = "UserRequest",
            //    Area = "BusDriving",
            //    ParentId = RequestId,
            //    IsEnable = true,
            //    ShowName = "حذف  درخواست کاربر",
            //    AccessCode = "3010005"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "افزودن شماره پیگیری درخواست کاربر",
            //    Actionname = "AddNumTracking",
            //    Controller = "UserRequest",
            //    Area = "BusDriving",
            //    ParentId = RequestId,
            //    IsEnable = true,
            //    ShowName = "افزودن شماره پیگیری درخواست کاربر",
            //    AccessCode = "3010006"
            //});
            //context.SaveChanges();

            //#endregion Request User

            //#region Subject Requests 
            ////----------------------- Access Code 3011  ------------------------------------------------
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "مدیریت موضوعات درخواست",
            //    Actionname = "IndexSubjectRequest",
            //    Controller = "SubjectRequest",
            //    Area = "BusDriving",
            //    ParentId = 0,
            //    IsEnable = true,
            //    ShowName = "مدیریت موضوعات درخواست",
            //    AccessCode = "3011"
            //});
            //context.SaveChanges();
            //int SubjectRequestId = context.Actions.FirstOrDefault(c => c.AccessCode == "3011").Id;
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    //Id = 2,
            //    Name = "صفحه موضوعات درخواست",
            //    Actionname = "IndexSubjectRequest",
            //    Controller = "SubjectRequest",
            //    Area = "BusDriving",
            //    ParentId = SubjectRequestId,
            //    IsEnable = true,
            //    ShowName = "صفحه موضوعات درخواست",
            //    AccessCode = "3011001"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "لیست موضوعات درخواست ها",
            //    Actionname = "ListSubjects",
            //    Controller = "SubjectRequest",
            //    Area = "BusDriving",
            //    ParentId = SubjectRequestId,
            //    IsEnable = true,
            //    ShowName = "لیست موضوعات درخواست ها",
            //    AccessCode = "3011002"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "افزودن موضوع درخواست",
            //    Actionname = "AddSubjectRequest",
            //    Controller = "SubjectRequest",
            //    Area = "BusDriving",
            //    ParentId = SubjectRequestId,
            //    IsEnable = true,
            //    ShowName = "افزودن موضوع درخواست",
            //    AccessCode = "3011003"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = " حذف موضوع درخواست",
            //    Actionname = "DeleteSubjectRequest",
            //    Controller = "SubjectRequest",
            //    Area = "BusDriving",
            //    ParentId = SubjectRequestId,
            //    IsEnable = true,
            //    ShowName = " حذف موضوع درخواست",
            //    AccessCode = "3011004"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = " ویرایش موضوع درخواست",
            //    Actionname = "EditSubjectRequest",
            //    Controller = "SubjectRequest",
            //    Area = "BusDriving",
            //    ParentId = SubjectRequestId,
            //    IsEnable = true,
            //    ShowName = " ویرایش موضوع درخواست",
            //    AccessCode = "3011005"
            //});
            //context.SaveChanges();
            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "لود اطلاعات برای ویرایش درخواست ها",
            //    Actionname = "LoadEdit",
            //    Controller = "SubjectRequest",
            //    Area = "BusDriving",
            //    ParentId = SubjectRequestId,
            //    IsEnable = true,
            //    ShowName = " لود اطلاعات برای ویرایش درخواست ها",
            //    AccessCode = "3011006"
            //});
            //context.SaveChanges();

            //#endregion Subject Requests 

            //#endregion

            //#region Menus

            //context.Menus.AddOrUpdate(p => p.Name,
            //    new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //    {
            //        Name = "صفحه اصلي",
            //        ActionId = context.Actions.Where(c => c.AccessCode == "1001").FirstOrDefault().Id,
            //        IsEnable = true,
            //        ParentId = 0,
            //        Code = "1001",
            //        RoleAccess = "Admin,Administrator,User,Modrator",
            //        IconName = "fa-home",
            //    });
            //context.SaveChanges();



            //#region Main Data
            ////------------------------ اطلاعات پایه Access Range Code : 3001 -------------------------------------
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "اطلاعات پایه",
            //    ActionId = LineId,
            //    IsEnable = true,
            //    ParentId = 0,
            //    Code = "3001",
            //    RoleAccess = "Admin,Administrator,Modrator",
            //    IconName = "fa-info",
            //});
            //context.SaveChanges();
            //int MainData = context.Menus.FirstOrDefault(c => c.Code == "3001").Id;
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "مدیریت خطوط",
            //    ActionId = LineId,
            //    IsEnable = true,
            //    ParentId = MainData,
            //    Code = "3001002",
            //    RoleAccess = "Admin,Administrator,Modrator"
            //});
            //context.SaveChanges();

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "مدیریت شیفت ها",
            //    ActionId = ShiftId,
            //    IsEnable = true,
            //    ParentId = MainData,
            //    Code = "3002001",
            //    RoleAccess = "Admin,Administrator,Modrator"
            //});
            //context.SaveChanges();

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "مدیریت سقف تعداد مرخصی",
            //    ActionId = MaxLeaveId,
            //    IsEnable = true,
            //    ParentId = MainData,
            //    Code = "3003001",
            //    RoleAccess = "Admin,Administrator,Modrator"
            //});
            //context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "روزهای نامعتبر",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "3004001").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = MainData,
            //    Code = "3004001",
            //    RoleAccess = "Admin,Administrator,Modrator"
            //});
            //context.SaveChanges();

            //#endregion

            //#region Leave

            ////-------------------مرخصی  Access Code 3002  -------------------------------------------
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "مرخصی",
            //    ActionId = LineId,
            //    IsEnable = true,
            //    ParentId = 0,
            //    Code = "3002",
            //    RoleAccess = "Admin,Administrator,Modrator,User",
            //    IconName = "fa-sign-out",
            //});
            //context.SaveChanges();
            //int LeaveData = context.Menus.FirstOrDefault(c => c.Code == "3002").Id;
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = " درخواست مرخصی",
            //    ActionId = LeaveRequestId,
            //    IsEnable = true,
            //    ParentId = LeaveData,
            //    Code = "3002002",
            //    RoleAccess = "Admin,Administrator,Modrator,User"
            //});
            //context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = " لیست مرخصی ها",
            //    ActionId = LeaveId,
            //    IsEnable = true,
            //    ParentId = LeaveData,
            //    Code = "3002003",
            //    RoleAccess = "Admin,Administrator,Modrator,User"
            //});
            //context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "بازه زمانی درخواست مرخصی",
            //    ActionId = PeriodLeaveId,
            //    IsEnable = true,
            //    ParentId = LeaveData,
            //    Code = "3002004",
            //    RoleAccess = "Admin,Administrator,Modrator"
            //});
            //context.SaveChanges();


            ////-------------------------

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "لیست ارجاع",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "3006006").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = LeaveData,
            //    Code = "3006006",
            //    RoleAccess = "Admin,Administrator,Modrator"
            //});
            //context.SaveChanges();

            //#endregion Leave

            //#region User
            ////----------------------------کاربران  Access Code 3003 ------------------------------------
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "کاربران",
            //    ActionId = LineId,
            //    IsEnable = true,
            //    ParentId = 0,
            //    Code = "3008",
            //    RoleAccess = "Admin,Administrator,Modrator,User",
            //    IconName = "fa-user"
            //});
            //context.SaveChanges();
            //int UserData = context.Menus.FirstOrDefault(c => c.Code == "3008").Id;
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = " ثبت کاربر",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "3008007").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = UserData,
            //    Code = "3008007",
            //    RoleAccess = "Admin,Administrator,Modrator"
            //});
            //context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = " ثبت کاربر از طریق اکسل",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "30080012").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = UserData,
            //    Code = "30080012",
            //    RoleAccess = "Admin,Administrator,Modrator"
            //});
            //context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "لیست کاربران",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "3008001").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = UserData,
            //    Code = "3008001",
            //    RoleAccess = "Admin,Administrator,Modrator"
            //});
            //context.SaveChanges();

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = " تغییر رمز عبور ",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "30080014").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = UserData,
            //    Code = "30080014",
            //    RoleAccess = "Admin,Administrator,Modrator,User"
            //});
            //#endregion User

            //#region Salary
            ////----------------------------فیش حقوقی  Access Code 3009 ----------------------------------

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "فیش حقوقی",
            //    ActionId = LineId,
            //    IsEnable = true,
            //    ParentId = 0,
            //    Code = "3009",
            //    RoleAccess = "Admin,Administrator,Modrator,User",
            //    IconName = "fa-file-text-o",
            //});
            //context.SaveChanges();
            //int SalaryData = context.Menus.FirstOrDefault(c => c.Code == "3009").Id;
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = " ثبت فیش حقوقی از طریق اکسل",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "3009002").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = SalaryData,
            //    Code = "3009002",
            //    RoleAccess = "Admin,Administrator,Modrator"
            //});
            //context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = " نمایش لیست فیش های کاربر",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "3009004").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = SalaryData,
            //    Code = "3009004",
            //    RoleAccess = "User"
            //});
            //context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "نمایش لیست فیش ها برای مدیر",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "3009006").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = SalaryData,
            //    Code = "3009006",
            //    RoleAccess = "Admin,Administrator,Modrator"
            //});
            //context.SaveChanges();

            //#endregion Salary

            //#region  Requests
            ////----------------------------درخواست ها  Access Code 3010 & 3011 ----------------------------------
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "درخواست ها",
            //    ActionId = LineId,
            //    IsEnable = true,
            //    ParentId = 0,
            //    Code = "3010",
            //    RoleAccess = "Admin,Administrator,Modrator,User",
            //    IconName = "fa-edit",
            //});
            //context.SaveChanges();
            //int RequestData = context.Menus.FirstOrDefault(c => c.Code == "3010").Id;

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "  موضوعات درخواست ها",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "3011001").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = RequestData,
            //    Code = "3011001",
            //    RoleAccess = "Admin,Administrator,Modrator"
            //});
            //context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = " لیست درخواست های ثبت شده",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "3010001").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = RequestData,
            //    Code = "3010001",
            //    RoleAccess = "Admin,Administrator,Modrator,User"
            //});
            //context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = " ثبت درخواست جدید",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "3010003").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = RequestData,
            //    Code = "3010003",
            //    RoleAccess = "Admin,Administrator,Modrator,User"
            //});
            //context.SaveChanges();
            //#endregion Requests


            #endregion menu

        }
    }
}
