using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.ServiceLayer.Extention;
using PrivateTraining.ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.BLL
{
    public class SendSms
    {
        private IUnitOfWork _uow;

        private IDbSet<Setting> _Setting;
        private IDbSet<SMSSended> _SMSSended;
        private IDbSet<SMSReceived> _SMSReceived;
        //private IDbSet<Debt> _DebtDb;
        //private IDbSet<DebtServiceProvider> _DebtServiceProvider;
        //private IDbSet<DebtServiceReceiverServiceLocation> _DebtServiceReceiverServiceLocation;

        public SendSms(IUnitOfWork uow /*, IApplicationUserManager userManager*/)
        {
            _uow = uow;
            //_userManager = userManager;
            _Setting = _uow.Set<Setting>();
            _SMSSended = _uow.Set<SMSSended>();
            _SMSReceived = _uow.Set<SMSReceived>();
            //_DebtDb = _uow.Set<Debt>();
            //_DebtServiceProvider = _uow.Set<DebtServiceProvider>();
            //_DebtServiceReceiverServiceLocation = _uow.Set<DebtServiceReceiverServiceLocation>();

        }

        public string SendSmsClass(string NumTo, string Text, int UserId = 0)
        {

            PersianDate PD = new PersianDate();
            try
            {
                IrSms.API_SMSServer sms = new IrSms.API_SMSServer();


                var Query = _Setting.Where(c => c.Subject == "sms" && c.Value4 == "On").FirstOrDefault();

                string User = Query.Value1;
                string Pass = Query.Value2;
                string Number = Query.Value3;
                string sUTF8 = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Convert(System.Text.Encoding.UTF8, System.Text.Encoding.UTF8, System.Text.Encoding.UTF8.GetBytes(Text)));
                string tmp = "";
                string resault = null;

                if (NumTo.Length >= 11)
                    if (Convert.ToByte(NumTo.Substring(0, 1)) == 0)
                        NumTo = NumTo.Substring(1, 10);

                if (NumTo.Length == 10)
                {
                    resault = sms.sendsms(Number, User, Pass, sUTF8, NumTo, "udh");

                    tmp = "پيام کوتاه شما بدرستي ارسال شده است.(" + resault + ")";

                    switch (resault)
                    {
                        case "1":
                            tmp = "نام کاربری یا کلمه عبور اشتباه می باشد.";
                            break;
                        case "2":
                            tmp = "دسترسی API فعال نمی باشد.";
                            break;
                        case "3":
                            tmp = "ثبت کننده خط نامعتبر می باشد.";
                            break;
                        case "4":
                            tmp = "اعتبار خط شما تمام شده است.";
                            break;
                        case "5":
                            tmp = "متن ارسالی خالی می باشد.";
                            break;
                        case "6":
                            tmp = "شماره گیرنده خالی می باشد.";
                            break;
                        case "7":
                            tmp = "شماره گیرندها بیشتر از 60 عدد می باشد.";
                            break;
                        case "8":
                            tmp = "شماره گیرنده اشتباه می باشد";
                            break;
                        case "9":
                            tmp = "اعتبار خط شما تمام شده است.";
                            break;
                        case "10":
                            tmp = "خطا در تراکنش های مالی.";
                            break;
                        case "11":
                            tmp = "خطا در ارتباط با مخابرات.";
                            break;
                        default:
                            tmp = "پيام کوتاه شما بدرستي ارسال شده است.(" + resault + ")";
                            break;
                    }
                }
                else
                {
                    tmp = "شماره اشتباه می باشد";
                }

                SMSSended temp = new SMSSended();
                temp.Content = Text;
                temp.UserId = UserId;
                //   temp.ReceiverNumber = _userManager.FindById(UserId).Mobile;
                temp.ReceiverNumber = NumTo;
                temp.SenderNumber = Number;
                temp.Date = PD.PersianDateLow();
                temp.Time = PD.CurrentTime();
                temp.StatusType = Convert.ToByte(TypeStatusDeliverSms.Success);
                temp.Status = tmp;
                _SMSSended.Add(temp);
                _uow.SaveAllChanges();
                return tmp;

            }

            catch (Exception ex)
            {
                //SMSSended temp = new SMSSended();
                //temp.Content = Text;
                //temp.UserId = UserId;
                //// temp.ReceiverNumber = _userManager.FindById(UserId).Mobile;
                //temp.ReceiverNumber = NumTo;
                //temp.SenderNumber = "0";
                //temp.Date = PD.PersianDateLow();
                //temp.Time = PD.CurrentTime();
                //temp.StatusType = Convert.ToByte(TypeStatusDeliverSms.Error);
                //temp.Status = ex.Message;
                //_SMSSended.Add(temp);
                //_uow.SaveAllChanges();

                return ex.Message.ToString();
            }
        }

        public void SensSmsRegister(string Domain, string Name, string Family, string UserName, string Password, string Mobile, int UserId, string Title)
        {
            string Body = "کاربر گرامی " + Name + " عزیز ثبت نام شما در سایت " + Title + "  با موفقیت انجام گردید" + "\n";
            Body += " نام کاربری : " + UserName + "\n";
            Body += "کلمه عبور : " + Password + "\n";
            Body += " http://" + Domain;
            SendSmsClass(Mobile, Body, UserId);
        }

        public void SensSmsRegisterReciver(string Domain, string Name, string Family, string UserName, string Password, string Mobile, int UserId, string Title,
            decimal ServiceReceiverServiceLocationId, bool Sex,string Type="")
        {
            string s = "آقای ";
            if (Sex)
                s = "خانم ";
            if (Type == "new")
            {
                string Body =  Name + " " + Family + " " + " عزیز ، به جمع مشتریان ارجمند سی پارس خوش آمدید درخواست شما با کد  " + ServiceReceiverServiceLocationId + "ثبت شد.به زودی با شما تماس می گیریم. لطفا جهت ارتباط بهتر، به پنل اختصاصی خود در سایت وارد شوید" + "\n";
                Body += " نام کاربری : " + UserName + " ، ";
                Body += "رمز عبور : " + Password + "\n";
                Body += " http://" + Domain;
                SendSmsClass(Mobile, Body, UserId);

                Body = s + Name + " " + Family + " " + "عزیز! لطفا خدمات را صرفا از طریق شرکت درخواست و پیگیری فرمایید. بدیهی است درقبال درخواست مستقیم از خدمت دهندگان، مسئول نخواهیم بود.پنل اختصاصی شما، مسیری آسان جهت درخواست خدمت است.";
                SendSmsClass(Mobile, Body, UserId);
            }
            else
            {
                string Body = s + Name + " " + Family + " " + " عزیز ، درخواست شما با کد  " + ServiceReceiverServiceLocationId + "ثبت شد.به زودی با شما تماس می گیریم. لطفا جهت ارتباط بهتر، به پنل اختصاصی خود در سایت وارد شوید" + "\n";
                Body += " http://" + Domain;
                SendSmsClass(Mobile, Body, UserId);
            }
        }

        public void SensSmsRegisterProvider(string Domain, string Name, string Family, string UserName, string Password, string Mobile, int UserId, string Title, bool Sex)
        {

            string s = "آقای ";
            if (Sex)
                s = "خانم ";

            string Body = s + Name + " "+ Family+" " + "عزیز اطلاعات اولیه شمادر سایت سی پارس ثبت شد. جهت تکمیل اطلاعات به پنل اختصاصی خود وارد شوید. " + "\n";
            Body += " نام کاربری : " + UserName + " ، ";
            Body += "کلمه عبور : " + Password + "\n";
            Body += " http://" + Domain;
            SendSmsClass(Mobile, Body, UserId);
        }

        //public string ContentSmsAndSendSms(string TypeSms, string NumTo, string Text, int UserId = 0)
        //{
        //    try
        //    {
        //        var ListSms = _Setting.Where(c => c.Value2 == "sms").ToList();

        //        //switch (TypeSms)
        //        //{
        //        //    case "[debt]":

        //        //        double Sumdebt = 0;
        //        //        var DebtSRSL = _DebtServiceReceiverServiceLocation.Include("ServiceReceiverServiceLocations").Include("ApplicationProviderUsers").Where(c => c.ServiceReceiverServiceLocations.ApplicationProviderUsers.Id == UserId && c.Status == 0).ToList();
        //        //        foreach (var item in DebtSRSL)
        //        //        {
        //        //            Sumdebt += item.CompanyCost;
        //        //        }
        //        //        var DebtSP = _DebtServiceProvider.Where(c => c.ServiceProviderId == UserId && c.Status == 0).ToList();
        //        //        foreach (var item2 in DebtSP)
        //        //        {
        //        //            Sumdebt += item2.CompanyCost;
        //        //        }
        //        //        Text = Text.Replace("[debt]", Sumdebt.ToString());
        //        //        break;

        //        //    case "[name]":
        //        //        var User = _userManager.FindById(UserId);
        //        //        Text = Text.Replace("[name]", User.Name);
        //        //        break;

        //        //    case "[family]":
        //        //        var User2 = _userManager.FindById(UserId);
        //        //        Text = Text.Replace("[family]", User2.Family);
        //        //        break;
        //        //}
        //        //   Text.Contains(strStart)

        //        if (Text.Contains("[debt]"))
        //        {
        //            double Sumdebt = 0;
        //            var DebtSRSfL = _DebtServiceReceiverServiceLocation.Where(c => c.ServiceReceiverServiceLocations.ApplicationProviderUsers.Id == UserId && c.Status == 0).ToList();

        //            var DebtSRSL = _DebtServiceReceiverServiceLocation.Include("ServiceReceiverServiceLocations").Where(c => c.ServiceReceiverServiceLocations.ApplicationProviderUsers.Id == UserId && c.Status == 0).ToList();
        //            foreach (var item in DebtSRSL)
        //            {
        //                Sumdebt += item.CompanyCost;
        //            }
        //            var DebtSP = _DebtServiceProvider.Where(c => c.ServiceProviderId == UserId && c.Status == 0).ToList();
        //            foreach (var item2 in DebtSP)
        //            {
        //                Sumdebt += item2.CompanyCost;
        //            }
        //            Text = Text.Replace("[debt]", Sumdebt.ToString("N0"));
        //        }

        //        if (Text.Contains("[name]"))
        //        {
        //            var User = _userManager.FindById(UserId);
        //            Text = Text.Replace("[name]", User.Name);
        //        }

        //        if (Text.Contains("[family]"))
        //        {
        //            var User2 = _userManager.FindById(UserId);
        //            Text = Text.Replace("[family]", User2.Family);
        //        }

        //        SendSms SendSms = new SendSms(_uow, _userManager);
        //        var result= SendSms.SendSmsClass(NumTo, Text, UserId);

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message.ToString();
        //    }
        //}
    }



}
