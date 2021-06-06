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
            context.Configuration.ProxyCreationEnabled = false;
            
            #region Actions

            #region MainPage 1001 , 1002

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Id = 1,
                Name = "صفحه اصلی سایت",
                Actionname = "Index",
                Controller = "Home",
                Area = "",
                ParentId = 0,
                IsEnable = true,
                ShowName = "صفحه اصلی سایت",
                AccessCode = "1001"
            });
            context.SaveChanges();

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
                AccessCode = "1002"
            });
            context.SaveChanges();
            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Id = 1,
                Name = "اطلاعات پایه",
                Actionname = "GetStates",
                Controller = "BaseInfo",
                Area = "BaseInfo",
                ParentId = 0,
                IsEnable = true,
                ShowName = "اطلاعات پایه",
                AccessCode = "1008"
            });
            context.SaveChanges();

            #region States and City
            int AccessId = context.Actions.FirstOrDefault(c => c.AccessCode == "1008").Id;

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "لیست استان ها",
                Actionname = "GetStates",
                Controller = "BaseInfo",
                Area = "BaseInfo",
                ParentId = AccessId,
                IsEnable = true,
                ShowName = "لیست استان ها",
                AccessCode = "1008001"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "لیست شهر ها",
                Actionname = "GetCitys",
                Controller = "BaseInfo",
                Area = "BaseInfo",
                ParentId = AccessId,
                IsEnable = true,
                ShowName = "لیست شهر ها",
                AccessCode = "1008002"
            });
            context.SaveChanges();

            #endregion States and City


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
                Name = " اطلاعات تکمیلی خدمتیار",
                Actionname = "GetAddEditServiceProviderInfo",
                Controller = "Account",
                Area = "",
                ParentId = Infos,
                IsEnable = true,
                ShowName = " اطلاعات تکمیلی خدمتیار",
                AccessCode = "1003003"
            });
            context.SaveChanges();


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
            int AccessId2 = context.Actions.FirstOrDefault(c => c.AccessCode == "2001").Id;

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Id = 2,
                Name = "تعريف گروههاي دسترسي",
                Actionname = "UserInGroup",
                Controller = "GroupPolicy",
                Area = "Security",
                ParentId = AccessId2,
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
                ParentId = AccessId2,
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

            //context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            //{
            //    Name = "صفحه ثبت نام کاربر",
            //    Actionname = "Register",
            //    Controller = "Account",
            //    Area = "",
            //    ParentId = UserId,
            //    IsEnable = true,
            //    ShowName = "صفحه ثبت نام کاربر",
            //    AccessCode = "10002002"
            //});
            //context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " ثبت نام اولیه خدمتیار",
                Actionname = "GetAddEditServiceProvider",
                Controller = "Account",
                Area = "",
                ParentId = UserId,
                IsEnable = true,
                ShowName = " ثبت نام  اولیه خدمتیار",
                AccessCode = "10002003"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " مدیریت خدمتیار ها",
                Actionname = "ListServiceProvider",
                Controller = "Account",
                Area = "",
                ParentId = UserId,
                IsEnable = true,
                ShowName = " مدیریت خدمتیار ها",
                AccessCode = "10002005"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " مدیریت مشتری ها",
                Actionname = "ListServiceReceiver",
                Controller = "Account",
                Area = "",
                ParentId = UserId,
                IsEnable = true,
                ShowName = " مدیریت مشتری ها",
                AccessCode = "10002006"
            });
            context.SaveChanges();

            #endregion User

            #region Framework Message 10008

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

            #endregion

            #region PrivateTraining 10001 , 10003,  10004 , 10005 , 10007,10009,10010

            #region Services  and Location and  and workUnit       

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Id = 2,
                Name = "مدیریت خدمات",
                Actionname = "Index",
                Controller = "ServiceProperties",
                Area = "PrivateTrain",
                ParentId = 0,
                IsEnable = true,
                ShowName = "مدیریت خدمات",
                AccessCode = "10001"
            });
            context.SaveChanges();


            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Id = 2,
                Name = "محل",
                Actionname = "Location",
                Controller = "ServiceLocation",
                Area = "PrivateTrain",
                ParentId = 0,
                IsEnable = true,
                ShowName = "محل",
                AccessCode = "10003"
            });
            context.SaveChanges();
            AccessId2 = context.Actions.FirstOrDefault(c => c.AccessCode == "10003").Id;

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Id = 2,
                Name = "مدیریت محل ها",
                Actionname = "Location",
                Controller = "ServiceLocation",
                Area = "PrivateTrain",
                ParentId = AccessId2,
                IsEnable = true,
                ShowName = "مدیریت محل ها",
                AccessCode = "10003001"
            });
            context.SaveChanges();


            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Id = 2,
                Name = "مدیریت خدمت محل ها",
                Actionname = "getlistservicelocation",
                Controller = "ServiceLocation",
                Area = "PrivateTrain",
                ParentId = AccessId2,
                IsEnable = true,
                ShowName = "مدیریت خدمت محل ها",
                AccessCode = "10003002"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Id = 2,
                Name = "مدیریت واحد کار",
                Actionname = "Index",
                Controller = "WorkUnit",
                Area = "PrivateTrain",
                ParentId = 0,
                IsEnable = true,
                ShowName = "مدیریت واحد کار",
                AccessCode = "10006"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Id = 2,
                Name = "تنظیمات",
                Actionname = "Setting",
                Controller = "PrivateTraining",
                Area = "",
                ParentId = 0,
                IsEnable = true,
                ShowName = "تنظیمات",
                AccessCode = "10009"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Id = 2,
                Name = "تعریف سطح خدمات",
                Actionname = "Index",
                Controller = "ServiceLevels",
                Area = "PrivateTrain",
                ParentId = 0,
                IsEnable = true,
                ShowName = "تعریف سطح خدمات",
                AccessCode = "10010"
            });
            context.SaveChanges();


            #endregion

            #region ServiceProvider and ServiceReceiver

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " مدیریت خدمات کاربران ",
                Actionname = "ServicesServiceProvider",
                Controller = "ServiceReceiverServiceLocation",
                Area = "PrivateTrain",
                ParentId = 0,
                IsEnable = true,
                ShowName = " مدیریت خدمات کاربران ",
                AccessCode = "10004"
            });
            context.SaveChanges();

            int ActionIdUserServices = context.Actions.FirstOrDefault(c => c.AccessCode == "10004").Id;

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "  خدمات جدید خدمتیار ",
                Actionname = "NewServicesServiceProvider",
                Controller = "ServiceReceiverServiceLocation",
                Area = "PrivateTrain",
                ParentId = ActionIdUserServices,
                IsEnable = true,
                ShowName = "  خدمات جدید",
                AccessCode = "10004001"
            });
            context.SaveChanges();
            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " خدمات قطعی شده  خدمتیار",
                Actionname = "CertainServiceServiceProvider",
                Controller = "ServiceReceiverServiceLocation",
                Area = "PrivateTrain",
                ParentId = ActionIdUserServices,
                IsEnable = true,
                ShowName = " خدمات قطعی شده",
                AccessCode = "10004002"
            });
            context.SaveChanges();
            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " خدمات تمام شده  خدمتیار",
                Actionname = "FinishedServicesServiceProvider",
                Controller = "ServiceReceiverServiceLocation",
                Area = "PrivateTrain",
                ParentId = ActionIdUserServices,
                IsEnable = true,
                ShowName = " خدمات تمام شده",
                AccessCode = "10004003"
            });
            context.SaveChanges();
            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " مدیریت مشتری ",
                Actionname = "ServicesServiceReceiver",
                Controller = "ServiceReceiverServiceLocation",
                Area = "PrivateTrain",
                ParentId = UserId,
                IsEnable = true,
                ShowName = " مدیریت مشتری",
                AccessCode = "10002007"
            });
            context.SaveChanges();
            #endregion ServiceProvider and ServiceReceiver

            #region admin services
            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " مدیریت خدمات مدیر ",
                Actionname = "aa",
                Controller = "ServiceReceiverServiceLocation",
                Area = "PrivateTrain",
                ParentId = 0,
                IsEnable = true,
                ShowName = " مدیریت خدمات مدیر ",
                AccessCode = "10005"
            });
            context.SaveChanges();

            int ActionIdAdminServices = context.Actions.FirstOrDefault(c => c.AccessCode == "10005").Id;

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "خدمات غیر اتوماسیون ",
                Actionname = "ServicesNonAutomation",
                Controller = "ServiceReceiverServiceLocation",
                Area = "PrivateTrain",
                ParentId = ActionIdAdminServices,
                IsEnable = true,
                ShowName = "خدمات غیر اتوماسیون",
                AccessCode = "10005001"
            });
            context.SaveChanges();
            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "خدمات غیر اتوماسیون و قطعی ",
                Actionname = "ServicesNonAutomationAndCertain",
                Controller = "ServiceReceiverServiceLocation",
                Area = "PrivateTrain",
                ParentId = ActionIdAdminServices,
                IsEnable = true,
                ShowName = "خدمات غیر اتوماسیون و قطعی",
                AccessCode = "10005002"
            });
            context.SaveChanges();
            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "خدمات اتوماسیون ",
                Actionname = "ServicesAutomation",
                Controller = "ServiceReceiverServiceLocation",
                Area = "PrivateTrain",
                ParentId = ActionIdAdminServices,
                IsEnable = true,
                ShowName = "خدمات اتوماسیون",
                AccessCode = "10005003"
            });
            context.SaveChanges();

            #endregion admin services

            #region debts
            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = " مدیریت بدهی ها ",
                Actionname = "GetListDebts",
                Controller = "Debt",
                Area = "PrivateTrain",
                ParentId = 0,
                IsEnable = true,
                ShowName = " مدیریت بدهی ها ",
                AccessCode = "10007"
            });
            context.SaveChanges();

            int ActionIdDebt = context.Actions.FirstOrDefault(c => c.AccessCode == "10007").Id;

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "لیست بدهی ها ",
                Actionname = "GetListDebts",
                Controller = "Debt",
                Area = "PrivateTrain",
                ParentId = ActionIdDebt,
                IsEnable = true,
                ShowName = "لیست بدهی ها",
                AccessCode = "10007001"
            });
            context.SaveChanges();

            context.Actions.AddOrUpdate(p => p.AccessCode, new PrivateTraining.DomainClasses.Entities.FrameWork.Action
            {
                Name = "لیست پرداختی ها ",
                Actionname = "GetListPayments",
                Controller = "Debt",
                Area = "PrivateTrain",
                ParentId = ActionIdDebt,
                IsEnable = true,
                ShowName = "لیست پرداختی ها",
                AccessCode = "10007002"
            });
            context.SaveChanges();


            #endregion debts

            #endregion

            #endregion

            //***********************************************************************************************************//

            #region Menus

            #region MainPage

            context.Menus.AddOrUpdate(p => p.Name,
                new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
                {
                    Name = "صفحه اصلی سایت",
                    ActionId = context.Actions.Where(c => c.AccessCode == "1001").FirstOrDefault().Id,
                    IsEnable = true,
                    ParentId = 0,
                    Code = "1001",
                    RoleAccess = "Admin,Administrator,User,Modrator,User,ServiceProvider",
                    IconName = "fa-external-link-square",
                    Sort = 1,
                });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name,
                new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
                {
                    Name = "صفحه اصلی پنل",
                    ActionId = context.Actions.Where(c => c.AccessCode == "1002").FirstOrDefault().Id,
                    IsEnable = true,
                    ParentId = 0,
                    Code = "1002",
                    RoleAccess = "Admin,Administrator,User,Modrator,User,ServiceProvider",
                    IconName = "fa-home",
                    Sort = 2,
                });
            context.SaveChanges();

            #endregion

            #region Basic Information

            //------------------------ اطلاعات پایه Access Range Code : 5001 -------------------------------------
            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "اطلاعات پایه",
                ActionId = AccessId,
                IsEnable = true,
                ParentId = 0,
                Code = "5001",
                RoleAccess = "Admin,Administrator,Modrator,",
                IconName = "fa-info",
                Sort=6,
            });
            context.SaveChanges();
            int MainData = context.Menus.FirstOrDefault(c => c.Code == "5001").Id;
            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "مدیریت استان ها",
                ActionId = context.Actions.Where(c => c.AccessCode == "1008001").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = MainData,
                Code = "1008001",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();
            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "مدیریت شهرها",
                ActionId = context.Actions.Where(c => c.AccessCode == "1008002").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = MainData,
                Code = "1008002",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();
            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "مدیریت واحد کار",
                ActionId = context.Actions.Where(c => c.AccessCode == "10006").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = MainData,
                Code = "10006",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "تنظیمات",
                ActionId = context.Actions.Where(c => c.AccessCode == "10009").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = MainData,
                Code = "10009",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "تعریف سطح خدمات",
                ActionId = context.Actions.Where(c => c.AccessCode == "10010").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = MainData,
                Code = "10010",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();


            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "مدیریت خدمات",
                ActionId = context.Actions.Where(c => c.AccessCode == "10001").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = MainData,
                Code = "10001",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            AccessId = context.Actions.FirstOrDefault(c => c.AccessCode == "10003001").Id;
            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "مدیریت محل ها",
                ActionId = context.Actions.Where(c => c.AccessCode == "10003001").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = MainData,
                Code = "10003001",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();
            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "  مدیریت خدمت محل ها",
                ActionId = context.Actions.Where(c => c.AccessCode == "10003002").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = MainData,
                Code = "10003002",
                RoleAccess = "Admin,Administrator,Modrator,User"
            });
            context.SaveChanges();

            #endregion  Basic Information

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
                Sort=4,
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

            #region User management
            //------------------------ Usere Code : 5002 -------------------------------------
            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "کاربران",
                ActionId = 1,
                IsEnable = true,
                ParentId = 0,
                Code = "5002",
                RoleAccess = "Admin,Administrator",
                IconName = "fa-users",
                Sort=5,
                
            });
            context.SaveChanges();
            int UserData = context.Menus.FirstOrDefault(c => c.Code == "5002").Id;

            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "لیست کاربران",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "10002001").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = UserData,
            //    Code = "10002001",
            //    RoleAccess = "Admin,Administrator"
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

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "مدیریت خدمتیار ها",
                ActionId = context.Actions.Where(c => c.AccessCode == "10002005").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = UserData,
                Code = "10002005",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();
            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "مدیریت مشتری ها",
                ActionId = context.Actions.Where(c => c.AccessCode == "10002006").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = UserData,
                Code = "10002006",
                RoleAccess = "Admin,Administrator,Modrator"
            });
            context.SaveChanges();

            #endregion User management

            #region  Management Services Users
            //------------------------ User Code : 5004 -------------------------------------

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "مدیریت خدمات خدمتیار ",
                ActionId = 1,
                IsEnable = true,
                ParentId = 0,
                Code = "5004",
                RoleAccess = "ServiceProvider",
                IconName = "fa-cogs",
                //TypeUser = 0,
                Sort=8,

            });
            context.SaveChanges();

            int ParentServiceProvider = context.Menus.FirstOrDefault(c => c.Code == "5004").Id;

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "خدمات جدید ",
                ActionId = context.Actions.Where(c => c.AccessCode == "10004001").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = ParentServiceProvider,
                Code = "10004001",
                RoleAccess = "Admin,Administrator,Modrator,ServiceProvider",
                TypeUser = 1,
            });
            context.SaveChanges();
            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "خدمات قطعی شده ",
                ActionId = context.Actions.Where(c => c.AccessCode == "10004002").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = ParentServiceProvider,
                Code = "10004002",
                RoleAccess = "Admin,Administrator,Modrator,ServiceProvider",
                TypeUser = 1,
            });
            context.SaveChanges();
            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "خدمات تمام شده ",
                ActionId = context.Actions.Where(c => c.AccessCode == "10004003").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = ParentServiceProvider,
                Code = "10004003",
                RoleAccess = "Admin,Administrator,Modrator,ServiceProvider",
                TypeUser = 1,

            });
            context.SaveChanges();


            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "مدیریت خدمات مشتری ",
                ActionId = context.Actions.Where(c => c.AccessCode == "10002007").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = 0,
                Code = "10002007",
                RoleAccess = "User",
                IconName = "fa-cogs",
                 TypeUser = 2,
                Sort = 8,

            });
            context.SaveChanges();
            //context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            //{
            //    Name = "مدیریت خدمات مشتری ",
            //    ActionId = context.Actions.Where(c => c.AccessCode == "10002007").FirstOrDefault().Id,
            //    IsEnable = true,
            //    ParentId = ParentServiceProvider,
            //    Code = "10002007",
            //    RoleAccess = "Admin,Administrator,Modrator,User",
            //    TypeUser = 2,

            //});
            //context.SaveChanges();

            #endregion  Management Services Users

            #region Management Services Admin

            //-----------------------admin ServiceCode :5005-------------------------------------

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "مدیریت خدمات مدیر ",
                ActionId = 1,
                IsEnable = true,
                ParentId = 0,
                Code = "5005",
                RoleAccess = "Admin,Administrator,Modrator",
                IconName = "fa-cogs",
                Sort=7,
            });
            context.SaveChanges();

            int ParentadminServices = context.Menus.FirstOrDefault(c => c.Code == "5005").Id;

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "خدمات غیر اتوماسیون ",
                ActionId = context.Actions.Where(c => c.AccessCode == "10005001").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = ParentadminServices,
                Code = "10005001",
                RoleAccess = "Admin,Administrator,Modrator",
            });
            context.SaveChanges();
            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "خدمات غیر اتوماسیون و قطعی ",
                ActionId = context.Actions.Where(c => c.AccessCode == "10005002").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = ParentadminServices,
                Code = "10005002",
                RoleAccess = "Admin,Administrator,Modrator",
            });
            context.SaveChanges();
            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "خدمات اتوماسیون ",
                ActionId = context.Actions.Where(c => c.AccessCode == "10005003").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = ParentadminServices,
                Code = "10005003",
                RoleAccess = "Admin,Administrator,Modrator",
            });
            context.SaveChanges();
            #endregion Management Services Admin

            #region User Info 

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "اطلاعات کاربر",
                ActionId = 1,
                IsEnable = true,
                ParentId = 0,
                Code = "1003",
                RoleAccess = "Admin,Administrator,Modrator,User,ServiceProvider",
                IconName = "fa-edit",
                Sort=3,
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
                RoleAccess = "Admin,Administrator,Modrator,User,ServiceProvider",
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = " اطلاعات تکمیلی خدمتیار",
                ActionId = context.Actions.Where(c => c.AccessCode == "1003003").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = UInfos,
                Code = "1003003",
                RoleAccess = "ServiceProvider"
            });
            context.SaveChanges();

            #endregion User Info

            #region Message

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "مدیریت پیام ها",
                ActionId = 1,
                IsEnable = true,
                ParentId = 0,
                Code = "10008",
                RoleAccess = "Admin,Administrator,Modrator,User,ServiceProvider",
                IconName = "fa-envelope",
                Sort=10,
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
                    RoleAccess = "Admin,Administrator,User,Modrator,ServiceProvider",
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
                    RoleAccess = "Admin,Administrator,User,Modrator,ServiceProvider",
                    IconName = "fa-envelope",
                });
            context.SaveChanges();

            #endregion

            #region debts

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "گزارشات ",
                ActionId = 1,
                IsEnable = true,
                ParentId = 0,
                Code = "5007",
                RoleAccess = "Admin,Administrator,Modrator,ServiceProvider",
                IconName = "fa-dollar",
                Sort=9,
            });
            context.SaveChanges();

            int ParentDebts = context.Menus.FirstOrDefault(c => c.Code == "5007").Id;

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "لیست بدهی ها",
                ActionId = context.Actions.Where(c => c.AccessCode == "10007001").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = ParentDebts,
                Code = "10007001",
                RoleAccess = "Admin,Administrator,Modrator,ServiceProvider",
            });
            context.SaveChanges();

            context.Menus.AddOrUpdate(p => p.Name, new PrivateTraining.DomainClasses.Entities.FrameWork.Menu
            {
                Name = "لیست پرداختی ها",
                ActionId = context.Actions.Where(c => c.AccessCode == "10007002").FirstOrDefault().Id,
                IsEnable = true,
                ParentId = ParentDebts,
                Code = "10007002",
                RoleAccess = "Admin,Administrator,Modrator,ServiceProvider",
            });
            context.SaveChanges();

            #endregion debts      

            #endregion menu

            //***********************************************************************************************************//

            #region state and city 

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
            //context.Settings.AddOrUpdate(p => p.Subject, new PrivateTraining.DomainClasses.Entities.FrameWork.Setting
            //{
            //    Id = 2,
            //    Subject = "sms",
            //    Value1 = "megatech",
            //    Value2 = "mega123",
            //    Value3 = "111111",
            //    Value4 = "On",

            //});
            //context.SaveChanges();
            //context.Settings.AddOrUpdate(p => p.Subject, new PrivateTraining.DomainClasses.Entities.FrameWork.Setting
            //{
            //    Id = 3,
            //    Subject = "RequestPaymentDebt",
            //    Value1 = "مانده بدهی شما [debt]  تومان می باشد لطفا به پرداخت بدهی خود اقدام نمایید",
            //    Value2 = "sms",
            //    Value3 = "0",
            //    Value4 = "0",

            //});
            //context.SaveChanges();
            //context.Settings.AddOrUpdate(p => p.Subject, new PrivateTraining.DomainClasses.Entities.FrameWork.Setting
            //{
            //    Id = 3,
            //    Subject = "ConfirmPaymentDebt",
            //    Value1 = "پرداخت شما با موفقیت انجام شد . مانده بدهی شما [debt] می باشد.",
            //    Value2 = "sms",
            //    Value3 = "0",
            //    Value4 = "0",

            //});
            //context.SaveChanges();

            #endregion

   //         #region Define Store Procedure with Sql Query


   //         context.Database.ExecuteSqlCommand(
   //"IF EXISTS ( SELECT * FROM   sysobjects  WHERE  id = object_id(N'[dbo].[ListAccessUsers]') " +
   //"and OBJECTPROPERTY(id, N'IsProcedure') = 1 ) BEGIN DROP PROCEDURE [dbo].[ListAccessUsers] END   "

   //);
   //         context.Database.ExecuteSqlCommand("create procedure [dbo].[ListAccessUsers] " +
   //"( @UserId int)  as " +
   //"( SELECT Framework.Actions.*, Security.AccessLevels.ActionId as Id,Framework.Actions.Name as Name " +
   //"FROM    Security.GroupPolicyUsers INNER JOIN  Security.AccessLevelGroups " +
   //"ON Security.GroupPolicyUsers.GroupPolicyId = Security.AccessLevelGroups.GroupId INNER JOIN " +
   //"Security.AccessLevels ON Security.AccessLevelGroups.Id = Security.AccessLevels.Id INNER JOIN " +
   //"Framework.Actions ON Security.AccessLevels.ActionId=Framework.Actions.Id WHERE  " +
   //" (Security.GroupPolicyUsers.UserId = @UserId) and Security.AccessLevels.IsEnable=1   and Security.AccessLevels.ActionId not in (SELECT  Security.AccessLevels.ActionId   FROM  Security.AccessLevelUsers INNER JOIN " +
   //" Security.AccessLevels ON Security.AccessLevelUsers.Id = Security.AccessLevels.Id " +
   //" INNER JOIN Framework.Actions ON Security.AccessLevels.ActionId = Framework.Actions.Id WHERE " +
   //" Security.AccessLevels.IsEnable = 0 and Security.AccessLevelUsers.UserId = @UserId) ) union " +
   //" (SELECT  Framework.Actions.*,Security.AccessLevels.ActionId as Id ,Framework.Actions.Name as Name  FROM " +
   //" Security.AccessLevelUsers INNER JOIN Security.AccessLevels ON Security.AccessLevelUsers.Id = " +
   //" Security.AccessLevels.Id INNER JOIN Framework.Actions ON Security.AccessLevels.ActionId=Framework.Actions.Id WHERE" +
   //" (Security.AccessLevelUsers.UserId = @UserId) and Security.AccessLevels.IsEnable=1 ) ");

   //         //Defined Stored procedure Payment and peymentDetails


   //         context.Database.ExecuteSqlCommand(
   //           "IF EXISTS ( SELECT * FROM   sysobjects  WHERE  id = object_id(N'[PrivateTraining].[SP_CalculationPayment]') " +
   //           " and OBJECTPROPERTY(id, N'IsProcedure') = 1 ) BEGIN DROP PROCEDURE [PrivateTraining].[SP_CalculationPayment] END   "

   //           );
   //         context.Database.ExecuteSqlCommand("create procedure [PrivateTraining].[SP_CalculationPayment] " +
   //          " @MemberId as int, @ModratorId as int, @TransactionNumber as nvarchar(100), @StatusPayment as tinyint, @ListDebtId AS varchar(MAX)" +
   //          " AS BEGIN" +
   //          " declare @Id as numeric(18, 0)" +
   //          " DECLARE @IsEnable bit " +
   //          " declare @Date as varchar(20)" +
   //          " declare @Time as varchar(8)" +
   //          " declare @CompanyCost as int" +
   //          " select @CompanyCost=sum(CompanyCost) from PrivateTraining.Debts where Id in  (select item from[dbo].[FN_ConvertStringToTable](@ListDebtId, ','))" +

   //          " SET @IsEnable = 'true'; " +
   //          " set @Date = dbo.FarsiDate(getdate())" +
   //          " set @Time = CONVERT(VARCHAR(8), GETDATE(), 108) " +
   //          " insert into PrivateTraining.payments " +
   //          " (Price,[Date],[Time], TransactionNumber,[Status], MemberId, IsEnable, ModratorId) " +
   //          " values(@CompanyCost, @Date, @Time, @TransactionNumber, @StatusPayment, @MemberId, @IsEnable, @ModratorId) " +

   //          " SELECT @Id =@@IDENTITY " +
   //          " exec[PrivateTraining].[Sp_CalculationPaymentDetail] @Id, @ListDebtId;" +
   //          " END"
   //          );


   //         //**************************************************************

   //         context.Database.ExecuteSqlCommand(
   //           "IF EXISTS ( SELECT * FROM   sysobjects  WHERE  id = object_id(N'[PrivateTraining].[Sp_CalculationPaymentDetail]') " +
   //           "and OBJECTPROPERTY(id, N'IsProcedure') = 1 ) BEGIN DROP PROCEDURE [PrivateTraining].[Sp_CalculationPaymentDetail] END   "

   //           );


   //         context.Database.ExecuteSqlCommand("create procedure [PrivateTraining].[Sp_CalculationPaymentDetail] " +
   //          " @paymentId as Int, @ListDebtId AS varchar(MAX)" +
   //          " AS BEGIN" +
   //          " delete from paymentDetails where paymentId = @paymentId" +
   //          " declare  @CalculatePricePayment as int" +
   //          " declare  @MemberId as int " +
   //          " declare  @ModratorId as int" +
   //          " select @CalculatePricePayment = Price, @MemberId = MemberId, @ModratorId = ModratorId" +
   //          " from [PrivateTraining].[payments]" +
   //          " where Id = @paymentId" +
   //          " DECLARE @Id int; DECLARE @CompanyCost int;  DECLARE @TotalCost int; DECLARE @PercentOfShares int; DECLARE @ServiceReceiverServiceLocationId int;  DECLARE @ServiceProviderId  int; DECLARE @IsEnable bit; SET @IsEnable = 'true'; " +
   //          " DECLARE vendor_cursor CURSOR FOR " +
   //          " select D.Id as Id_x , " +
   //                   " D.CompanyCost as CompanyCost_x, " +
   //                   " D.TotalCost as TotalCost_x," +
   //                   " D.PercentOfShares as PercentOfShares_x, " +
   //                   " DS.ServiceReceiverServiceLocationId, " +
   //                   " DSP.ServiceProviderId" +
   //          " from PrivateTraining.Debts as D" +
   //             " left JOIN PrivateTraining.DebtServiceReceiverServiceLocations as DS  ON D.Id = DS.Id" +
   //             " left JOIN PrivateTraining.DebtServiceProviders as DSP  ON D.Id = DSP.Id" +
   //          " where D.Id in (select item from[dbo].[FN_ConvertStringToTable](@ListDebtId, ','))" +

   //          " OPEN vendor_cursor" +
   //          " FETCH NEXT FROM vendor_cursor" +
   //          " INTO @Id, @CompanyCost, @TotalCost, @PercentOfShares, @ServiceReceiverServiceLocationId, @ServiceProviderId" +
   //          " WHILE @@FETCH_STATUS = 0  BEGIN" +
   //          " insert into[PrivateTraining].[paymentDetails] (CompanyCostDebt,[Status],IsEnable,DebtId,paymentId,ServiceProviderId,ServiceReceiverServiceLocationId,CalculatePricePayment,MemberId,ModratorId,TotalCostDebt,PercentOfSharesDebt)" +
   //          " values(@CompanyCost,0, @IsEnable, @Id, @paymentId, @ServiceProviderId, @ServiceReceiverServiceLocationId, @CalculatePricePayment, @MemberId, @ModratorId, @TotalCost, @PercentOfShares)" +
   //          " FETCH NEXT FROM vendor_cursor" +
   //          " INTO @Id, @CompanyCost, @TotalCost, @PercentOfShares, @ServiceReceiverServiceLocationId, @ServiceProviderId" +
   //          " END" +
   //          " CLOSE vendor_cursor;" +
   //          " DEALLOCATE vendor_cursor;" +
   //          " END"
   //          );

   //         //**************************************************************

   //         context.Database.ExecuteSqlCommand(
   //           "IF EXISTS ( SELECT * FROM   sysobjects  WHERE  id = object_id(N'[PrivateTraining].[SP_ListServiceProviderBySL]') " +
   //           "and OBJECTPROPERTY(id, N'IsProcedure') = 1 ) BEGIN DROP PROCEDURE [PrivateTraining].[SP_ListServiceProviderBySL] END   "

   //           );


   //         context.Database.ExecuteSqlCommand("create procedure [PrivateTraining].[SP_ListServiceProviderBySL] " +
   //          " @ServiceId as int,@LocationId as int " +
   //          " AS BEGIN " +
   //          " select U.Id,U.Name,U.Family,U.Sex,U.Picture,U.Path,S.Resume,S.Level " +
   //          " from " +
   //          " [Security].[Users] as U  " +
   //          "  join [Security].[ServicesProviderInfo] as S on U.Id=S.Id " +
   //          "  join [PrivateTraining].[UserServiceLocations] as US on S.Id=US.UserId " +
   //          " where US.ServiceId=@ServiceId and US.LocationId=@LocationId and IsEnable=1 and US.StatusServiceLocationUser=1 " +
   //          " END"
   //          );



   //         //**************************************************************

   //         context.Database.ExecuteSqlCommand(
   //           "IF EXISTS ( SELECT * FROM   sysobjects  WHERE  id = object_id(N'[PrivateTraining].[SP_Debts]') " +
   //           "and OBJECTPROPERTY(id, N'IsProcedure') = 1 ) BEGIN DROP PROCEDURE [PrivateTraining].[SP_Debts] END   "

   //           );


   //         context.Database.ExecuteSqlCommand("create procedure [PrivateTraining].[SP_Debts] " +
   //        " @StateId As nvarchar(10), \r\n" +
   //        " @CityId As  nvarchar(10), \r\n" +
   //        " @LocationId As  nvarchar(10), \r\n" +
   //        " @ServiceId As  nvarchar(10), \r\n" +
   //        " @UserId As  nvarchar(10), \r\n" +
   //        " @UserType As  nvarchar(10), \r\n" +
   //        " @Status As  nvarchar(10), \r\n" +
   //        " @TypeDebt As  nvarchar(10), \r\n" +
   //        " @Name As  nvarchar(50), \r\n" +
   //        " @PriceDebtMoreThan  As  nvarchar(50), \r\n" +
   //        " @CountDebtMoreThan  As  nvarchar(50), \r\n" +
   //        " @DateDebtMoreThan  As  nvarchar(50) \r\n" +
   //        " AS BEGIN \r\n" +
   //        " SET NOCOUNT ON; \r\n" +
   //        " DECLARE @CountDetailDebt nvarchar(3000) \r\n" +
   //        " DECLARE @STR nvarchar(4000) \r\n" +
   //        " DECLARE @STRdp nvarchar(4000) \r\n" +
   //        " DECLARE @STRdpSelect nvarchar(4000) \r\n" +
   //        " DECLARE @STRdpWhere nvarchar(4000) \r\n" +
   //        " DECLARE @STRdpGroupBy nvarchar(4000) \r\n" +
   //        " DECLARE @STRds nvarchar(4000) \r\n" +
   //        " DECLARE @STRdsSelect nvarchar(4000) \r\n" +
   //        " DECLARE @STRdsWhere nvarchar(4000) \r\n" +
   //        " DECLARE @STRdsGroupBy nvarchar(4000) \r\n" +
   //        "  DECLARE @text1 nvarchar(100) set @text1 = N'ارائه خدمت' DECLARE @text2 nvarchar(100) set @text2 = N'ثبت نام خدمتیار' \r\n" +
   //        " IF(@TypeDebt = 0 ) \r\n  BEGIN \r\n" +
   //        " SET @STRdsSelect = ' SELECT D.Id, D.TotalCost, D.PercentOfShares, D.CompanyCost, D.TotalCostReceived, D.Status, D.StatusServiceReceiverServiceLocation, SRSL.ServiceProviderId, SV.Title'+' + '' '' + '+'L.Name As ServiceLocationName, UP.Name'+' + '' '' + '+'UP.Family As ProviderFullName, UR.Name'+' + '' '' + '+'UR.Family As RecevierFullName, D.Date As Date, N'''+@text1+''' AS ReasonDebt FROM[PrivateTraining].[Debts] AS D \r\n" +
   //        " RIGHT OUTER  JOIN[PrivateTraining].[DebtServiceReceiverServiceLocations]  AS DSL  ON D.Id = DSL.Id \r\n" +
   //        " JOIN[PrivateTraining].[ServiceReceiverServiceLocations] AS SRSL ON DSL.ServiceReceiverServiceLocationId = SRSL.ID \r\n" +
   //        " JOIN[PrivateTraining].[ServiceLocations] AS SL ON SRSL.ServiceLocationId = SL.Id \r\n" +
   //        " JOIN[BaseInfo].[Cities] AS C ON C.Id = SL.CityId \r\n" +
   //        " JOIN[BaseInfo].[States] AS S ON S.Id = C.StateId \r\n" +
   //        " JOIN[PrivateTraining].[Services] AS SV ON SV.Id = SL.ServiceId \r\n" +
   //        " JOIN[PrivateTraining].[Locations] AS L ON L.Id = SL.LocationId \r\n" +
   //        " JOIN[Security].[Users] AS UP ON UP.Id = SRSL.ServiceProviderId \r\n" +
   //        " JOIN[Security].[Users] AS UR ON UR.Id = SRSL.ServiceReceiverId' \r\n" +
   //        " SET @STRdsWhere = ' WHERE D.Status=' + @Status \r\n " +
   //        " SET @STRds = @STRdsSelect + @STRdsWhere \r\n" +
   //        " SET @STRdpSelect = ' \r\n" +
   //        " SELECT D.Id, D.TotalCost, D.PercentOfShares, D.CompanyCost, D.TotalCostReceived, D.Status, D.StatusServiceReceiverServiceLocation, DP.ServiceProviderId, '' - '' As ServiceLocationName, '' - ''  As ServiceLocationName, '' - '' As RecevierFullName, D.Date As Date, N'''+@text2+''' AS ReasonDebt FROM[PrivateTraining].[Debts] AS D \r\n " +
   //        "  RIGHT OUTER  JOIN[PrivateTraining].[DebtServiceProviders] AS DP  ON D.Id = DP.Id \r\n" +
   //        " JOIN[Security].[Users] AS UP ON UP.Id = dp.ServiceProviderId  ' \r\n" +
   //        " SET @STRdpWhere = ' WHERE D.Status=' + @Status \r\n" +
   //        " SET @STRdp = @STRdpSelect + @STRdpWhere \r\n" +
   //        " IF(@UserType = 2  and @UserId != 0) \r\n" +
   //            " BEGIN \r\n" +
   //            " SET @STRdp = '' \r\n" +
   //            " SET @STRds = @STRds + ' AND SRSL.ServiceReceiverId=' + @UserId \r\n" +
   //            " END \r\n" +
   //        " ELSE IF(@UserType = 1 and @UserId != 0) \r\n" +
   //            " BEGIN \r\n" +
   //            "  SET @STRdp = @STRdp + ' AND DP.ServiceProviderId=' + @UserId \r\n" +
   //            "  SET @STRds = @STRds + ' AND SRSL.ServiceProviderId=' + @UserId \r\n" +
   //            "  END \r\n" +
   //        "  IF(@StateId != 0 AND @CityId = 0 AND @LocationId = 0) \r\n " +
   //            " BEGIN \r\n" +
   //            "   SET @STRdp = @STRdp + ' AND UP.StateId=' + @StateId \r\n" +
   //            "  SET @STRds = @STRds + ' AND S.Id= ' + @StateId \r\n" +
   //            "  END \r\n" +
   //       " ELSE IF(@StateId != 0 AND @CityId != 0 AND @LocationId = 0) \r\n" +
   //           "  BEGIN \r\n" +
   //           "   SET @STRdp = @STRdp + ' AND UP.CityId=' + @CityId \r\n" +
   //           "  SET @STRds = @STRds + ' AND SL.CityId=' + @CityId \r\n" +
   //           "  END \r\n" +
   //       " ELSE IF(@StateId != 0 AND @CityId != 0 AND @LocationId != 0) \r\n" +
   //           "   BEGIN \r\n" +
   //           "  SET @STRdpSelect = @STRdpSelect + ' JOIN  [PrivateTraining].[UserLocations] AS UL ON UL.UserId=DP.ServiceProviderId '; \r\n" +
   //           "  SET @STRdpWhere = @STRdpWhere + ' AND UL.LocationId=' + @LocationId \r\n" +
   //           "  SET @STRdp = @STRdpSelect + @STRdpWhere \r\n" +
   //           "   SET @STRds = @STRds + ' AND SL.LocationId=' + @LocationId \r\n" +
   //           "   END \r\n" +
   //       " IF(@ServiceId != 0) \r\n" +
   //           "  BEGIN \r\n" +
   //           " SET @STRdp = '' \r\n" +
   //           "  SET @STRds = @STRds + ' AND SL.ServiceId=' + @ServiceId; \r\n" +
   //           "  END \r\n" +
   //       " IF(@Name != '') \r\n" +
   //           " BEGIN \r\n" +
   //           "   IF(@STRdp != '')\r\n " +
   //           "   BEGIN \r\n" +
   //           "   SET @STRdp = @STRdp + ' AND ( UP.Name' + '+'' ''+' + 'Up.Family ' + ' ' + ' LIKE N' + '''%' + @Name + '%'')'; \r\n" +
   //           "  END \r\n" +
   //           "  SET @STRds = @STRds + ' AND ( UP.Name' + '+'' ''+' + 'Up.Family LIKE N' + '''%' + @Name + '%'')'; \r\n" +
   //           "    END \r\n" +
   //       "   IF(@STRds != '' AND @STRdp != '') \r\n" +
   //           "  BEGIN \r\n" +
   //           "  SET @STR = @STRds + ' UNION ALL ' + @STRdp \r\n " +
   //           "  END \r\n" +
   //       "  ELSE IF(@STRds != '' AND @STRdp = '') \r\n" +
   //           "  BEGIN \r\n" +
   //           "  SET @STR = @STRds \r\n" +
   //           "  END \r\n" +
   //       "  ELSE IF(@STRds = '' AND @STRdp != '') \r\n" +
   //           "   BEGIN \r\n" +
   //           "   SET @STR = @STRdp \r\n" +
   //           "  END \r\n" +
   //       "  IF(@PriceDebtMoreThan != '') \r\n" +
   //           "   BEGIN \r\n" +
   //           "  SET @STR = @STR + ' AND D.CompanyCost>=' + @CountDebtMoreThan \r\n" +
   //           "  END \r\n" +
   //       "  IF(@DateDebtMoreThan != '') \r\n" +
   //           "  BEGIN \r\n" +
   //           "  SET @STR = @STR + ' AND D.Date>=' + '''' + @DateDebtMoreThan + '''' \r\n" +
   //           "  END \r\n" +
   //       "  IF(@CountDebtMoreThan != '') \r\n" +
   //           "  BEGIN \r\n " +
   //           "  SET @STR = @STR + ' AND D.CompanyCost>=' + @CountDebtMoreThan \r\n" +
   //           "   END \r\n" +
   //       "  END \r\n" +
   //       "  ELSE IF(@TypeDebt = 1) \r\n" +
   //       "  BEGIN \r\n" +
   //       "  SET @STRdsSelect = '  \r\n" +

   //          "  SELECT Max(Date) as LastDate, Count(D.Id) as CountA, Sum(D.TotalCost) AS TotalCost, sum(D.CompanyCost) As CompanyCost, sum(D.TotalCostReceived) AS TotalCostReceived, UP.Id, UP.Name'+' + '' '' + '+'UP.Family As ProviderFullName FROM[PrivateTraining].[Debts] AS D \r\n" +
   //          "  RIGHT OUTER  JOIN[PrivateTraining].[DebtServiceReceiverServiceLocations]  AS DSL  ON D.Id = DSL.Id \r\n" +
   //          "  JOIN[PrivateTraining].[ServiceReceiverServiceLocations] AS SRSL ON DSL.ServiceReceiverServiceLocationId = SRSL.ID \r\n" +
   //          "   RIGHT OUTER JOIN[PrivateTraining].[ServiceLocations] AS SL ON SRSL.ServiceLocationId = SL.Id \r\n" +
   //          "  JOIN[BaseInfo].[Cities] AS C ON C.Id = SL.CityId \r\n" +
   //          "  JOIN[BaseInfo].[States] AS S ON S.Id = C.StateId \r\n" +
   //          "  JOIN[PrivateTraining].[Services] AS SV ON SV.Id = SL.ServiceId \r\n" +
   //          "  JOIN[PrivateTraining].[Locations] AS L ON L.Id = SL.LocationId \r\n" +
   //          "  JOIN[Security].[Users] AS UP ON UP.Id = SRSL.ServiceProviderId \r\n" +
   //          "  JOIN[Security].[Users] AS UR ON UR.Id = SRSL.ServiceReceiverId' \r\n" +
   //          "  SET @STRdsWhere = ' WHERE D.Status=' + @Status \r\n" +
   //          "  SET @STRdsGroupBy = ' GROUP BY UP.Id,UP.Name,UP.Family ' \r\n" +

   //          " SET @STRdpSelect = ' \r\n " +
   //          "  SELECT Max(Date) as LastDate, Count(D.Id) as CountA, Sum(D.TotalCost) AS TotalCost, sum(D.CompanyCost) As CompanyCost, sum(D.TotalCostReceived) AS TotalCostReceived, UP.Id, UP.Name'+' + '' '' + '+'UP.Family As ProviderFullName FROM[PrivateTraining].[Debts] AS D \r\n" +
   //          "  RIGHT OUTER  JOIN[PrivateTraining].[DebtServiceProviders] AS DP  ON D.Id = DP.Id \r\n" +
   //          "  JOIN[Security].[Users] AS UP ON UP.Id = DP.ServiceProviderId ' \r\n" +
   //          "  SET @STRdpWhere = ' WHERE D.Status=' + @Status \r\n" +
   //          "  SET @STRdPGroupBy = ' GROUP BY UP.Id,UP.Name,UP.Family ' \r\n" +

   //          "  IF(@StateId != 0 AND @CityId = 0 AND @LocationId = 0) \r\n " +
   //             "  BEGIN \r\n" +
   //             "  SET @STRdpWhere = @STRdpWhere + ' AND UP.StateId=' + @StateId \r\n " +
   //             "  SET @STRdsWhere = @STRdsWhere + ' AND S.Id= ' + @StateId  \r\n" +
   //             "   END \r\n" +
   //          "  ELSE IF(@StateId != 0 AND @CityId != 0 AND @LocationId = 0) \r\n" +
   //             "   BEGIN \r\n" +
   //             "  SET @STRdpWhere = @STRdpWhere + ' AND UP.CityId=' + @CityId \r\n" +
   //             "  SET @STRdsWhere = @STRdsWhere + ' AND SL.CityId=' + @CityId \r\n" +
   //             "  END \r\n" +
   //          "  ELSE IF(@StateId != 0 AND @CityId != 0 AND @LocationId != 0) \r\n " +
   //             "   BEGIN \r\n" +
   //             "  SET @STRdpSelect = @STRdpSelect + ' JOIN  [PrivateTraining].[UserLocations] AS UL ON UL.UserId=DP.ServiceProviderId ';\r\n " +
   //             "   SET @STRdpWhere = @STRdpWhere + ' AND UL.LocationId=' + @LocationId \r\n " +
   //             "  SET @STRdsWhere = @STRdsWhere + ' AND SL.LocationId=' + @LocationId  \r\n" +
   //             "  END \r\n" +
   //         " IF(@ServiceId != 0) \r\n" +
   //             "  BEGIN \r\n" +
   //             "  SET @STRdpSelect = '' \r\n" +
   //             "  SET @STRdpWhere = '' \r\n" +
   //             "  SET @STRdPGroupBy = '' \r\n" +
   //             "   SET @STRdsWhere = @STRdsWhere + ' AND SL.ServiceId=' + @ServiceId; \r\n " +
   //             "  END \r\n" +
   //      " SET @STRds = @STRdsSelect + @STRdsWhere + @STRdsGroupBy \r\n" +
   //      " SET @STRdp = @STRdpSelect + @STRdpWhere + @STRdPGroupBy \r\n" +

   //       " IF(@Name != '') \r\n " +
   //           " BEGIN \r\n" +
   //           "  IF(@STRdp != '') \r\n" +
   //               "  BEGIN \r\n" +
   //               " SET @STRdpWhere = @STRdpWhere + ' AND ( UP.Name' + '+'' ''+' + 'Up.Family ' + ' ' + ' LIKE N' + '''%' + @Name + '%'')'; \r\n" +
   //               "  END \r\n" +
   //          " SET @STRdsWhere = @STRdsWhere + ' AND ( UP.Name' + '+'' ''+' + 'Up.Family LIKE N' + '''%' + @Name + '%'')'; \r\n" +
   //           "   END \r\n" +

   //         "  SET @STRds = @STRdsSelect + @STRdsWhere + @STRdsGroupBy \r\n " +
   //         "  SET @STRdp = @STRdpSelect + @STRdpWhere + @STRdPGroupBy \r\n " +

   //        " IF(@STRds != '' AND @STRdp != '') \r\n" +
   //            "   BEGIN \r\n" +
   //            "   SET @STR = ' SELECT Max(LastDate) as LastDate,sum(CountA) as CountA,Sum(TotalCost) AS TotalCost, sum(CompanyCost) As CompanyCost,sum(TotalCostReceived) AS TotalCostReceived ,id As ServiceProviderId , ProviderFullName  FROM  (' \r\n" +
   //                 "  + @STRds + ' UNION ALL ' + @STRdp + ' \r\n " +
   //                 " ) AS A \r\n" +
   //                 " GROUP BY ID,ProviderFullName \r\n" +
   //                 "  having  sum(CountA) != 0' \r\n" +
   //            "  END \r\n" +
   //      " ELSE IF(@STRds != '' AND @STRdp = '') \r\n" +
   //        "  BEGIN \r\n" +
   //        "  SET @STR = ' SELECT Max(LastDate) as LastDate,sum(CountA) as CountA,Sum(TotalCost) AS TotalCost, sum(CompanyCost) As CompanyCost,sum(TotalCostReceived) AS TotalCostReceived ,id As ServiceProviderId , ProviderFullName  FROM  (' \r\n" +
   //           "  + @STRds + ' \r\n " +
   //           " ) AS A \r\n" +
   //           " GROUP BY ID,ProviderFullName \r\n " +
   //           "  having  sum(CountA) != 0'  \r\n " +
   //          "  END  \r\n " +
   //      " ELSE IF(@STRds = '' AND @STRdp != '')  \r\n " +
   //         "   BEGIN  \r\n " +
   //         "  SET @STR = ' SELECT Max(LastDate) as LastDate,sum(CountA) as CountA,Sum(TotalCost) AS TotalCost, sum(CompanyCost) As CompanyCost,sum(TotalCostReceived) AS TotalCostReceived ,id As ServiceProviderId , ProviderFullName  FROM  ('  \r\n " +
   //              "   + @STRdp + '   \r\n " +
   //              " ) AS A  \r\n " +
   //              " GROUP BY ID,ProviderFullName  \r\n  " +
   //              " having  sum(CountA) != 0'  \r\n " +
   //         " END  \r\n " +
   //       " IF(@CountDebtMoreThan != '')  \r\n " +
   //            "  BEGIN  \r\n " +
   //            "   SET @STR = @STR + ' AND sum(CountA)>=' + @CountDebtMoreThan  \r\n " +
   //            "  END  \r\n " +
   //       "  IF(@PriceDebtMoreThan != '')  \r\n " +
   //            " BEGIN  \r\n " +
   //            "  SET @STR = @STR + ' AND Sum(CompanyCost)>=' + @PriceDebtMoreThan  \r\n " +
   //            " END  \r\n " +
   //       "  IF(@DateDebtMoreThan != '')  \r\n " +
   //            " BEGIN  \r\n " +
   //            " SET @STR = @STR + ' AND Max(LastDate)>=' + '''' + @DateDebtMoreThan + '''';  \r\n " +
   //            "  END  \r\n " +
   //    " END  \r\n " +
   //  " exec(@STR)  \r\n " +
   //  " END "
   //          );







   //         #endregion

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


        }
    }
}
