using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Data.Entity;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.ServiceLayer.BLL;

namespace PrivateTraining.ServiceLayer
{
    public class EmailService : IIdentityMessageService
    {
        //public Task SendAsync(IdentityMessage message)
        //{
        //    // Plug in your email service here to send an email.
        //    return Task.FromResult(0);
        //}

        private IDbSet<DomainClasses.Entities.FrameWork.Setting> _Setting;
        private IDbSet<DomainClasses.Entities.FrameWork.SaveEmail> _SaveEmail;

        private IUnitOfWork _uow;

        public EmailService(IUnitOfWork uow)
        {
            _uow = uow;
            _Setting = _uow.Set<DomainClasses.Entities.FrameWork.Setting>();
            _SaveEmail = _uow.Set<DomainClasses.Entities.FrameWork.SaveEmail>();
        }

        public Task SendAsync(IdentityMessage message)
        {
            //string From = "main@mega-tech.ir";
            //string Password = "mega123";
            //string Smtp = "smtp.mega-tech.ir";
            //string MailHost = "mail.mega-tech.ir";
            PersianDate PD = new PersianDate();
            string From = "";
            string Password = "";
            string Smtp = "";
            string MailHost = "";
            try
            {
                var Query = _Setting.Where(c => c.Subject == "Email").Where(c=>c.IsEnable==true).FirstOrDefault();

                if (Query != null)
                {
                    From = Query.Value1;
                    Password = Query.Value2;
                    Smtp = Query.Value3;
                    MailHost = Query.Value4;
                }

                MailAddress SendTo = new MailAddress(message.Destination);
                MailAddress SendFrom = new MailAddress(From);
                System.Net.Mail.MailMessage Email = new System.Net.Mail.MailMessage(SendFrom, SendTo);
                //--------------
                Email.Subject = message.Subject;
                //--------------
                Email.Body = message.Body;
                //--------------
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                //mailClient.Port = Convert.ToInt32(Query.Value5);
                System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(From, Password);
                SmtpClient smtp = new SmtpClient(Smtp);
                Email.IsBodyHtml = true;
                mailClient.Host = MailHost;
                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = basicAuthenticationInfo;
                mailClient.Send(Email);

                DomainClasses.Entities.FrameWork.SaveEmail Send = new DomainClasses.Entities.FrameWork.SaveEmail();
                Send.Subject = message.Subject;
                Send.Body = message.Body;
                Send.ToEmail = message.Destination;
                Send.FromEmail = From;
                Send.Status = "OK";
                Send.Date = PD.PersianDateLow();
                _SaveEmail.Add(Send);
                var Status = _uow.SaveAllChanges();
            }
            catch (Exception Ex)
            {

                DomainClasses.Entities.FrameWork.SaveEmail Send = new DomainClasses.Entities.FrameWork.SaveEmail();
                Send.Subject = message.Subject;
                Send.Body = message.Body;
                Send.ToEmail = message.Destination;
                Send.FromEmail = From;
                Send.Status = Ex.Message;
                Send.Date = PD.PersianDateLow();
                _SaveEmail.Add(Send);
                var Status = _uow.SaveAllChanges();
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// ارسال ایمیل خوش آمدگویی به کاربر زمان ثبت نام
        /// </summary>
        /// <param name="Domain"></param>
        /// <param name="Name"></param>
        /// <param name="Family"></param>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="Email"></param>
        public async void SendEmailRegister(string Domain,string Name,string Family,string UserName,string Password,string Email,string Title)
        {
            IdentityMessage Message = new IdentityMessage();
            string Body = "<div style='Direction:rtl;'>" +
                "<br /><div>کاربر گرامی " + Name + " " + Family + " عزیز ثبت نام شما در سایت " + Title + "  با موفقیت انجام گردید</div>";

            Body += "<br /><div>نام کاربری : " + UserName + " </div>";
            Body += "<br /><div>کلمه عبور : " + Password + " </div>";
            Body += "<br /><div><a href='http://" + Domain + "' target='_blank'>" + Domain + "</a></div>" + "</div>";

            Message.Body = Body;
            Message.Subject = " ثبت نام در سایت " + Domain;
            Message.Destination = Email;

            PrivateTraining.ServiceLayer.EmailService email = new PrivateTraining.ServiceLayer.EmailService(_uow);
            await email.SendAsync(Message);
        }
    }
}
