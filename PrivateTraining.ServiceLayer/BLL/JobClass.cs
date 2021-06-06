using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using Quartz;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using System.Web;

namespace ScheduledTaskExample.ScheduledTasks
{
    public class JobClass : IJob
    {
        private IUnitOfWork _uow;
        private IDbSet<Setting> _Setting;
        public JobClass(IUnitOfWork uow)
        {
            _uow = uow;
            _Setting = _uow.Set<Setting>();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //// dummy 1ms sleep
            //await Task.Delay(1);

            try
            {
                PrivateTraining.ServiceLayer.IrSms.API_SMSServer sms = new PrivateTraining.ServiceLayer.IrSms.API_SMSServer();
                var Query = _Setting.Where(c => c.Subject == "sms" && c.Value4 == "On").FirstOrDefault();
                if (Query != null)
                {
                    var Results = sms.Recive_sms(Query.Value3, Query.Value1, Query.Value2, "1");

                    string[] Result = Results.Split(new string[] { "*#@#@#*" }, StringSplitOptions.None);

                    var ReciveText = Result[0];
                    var ReciveDate = Result[1];
                    var Sender = Result[2];

                    HttpContext.Current.Response.Redirect("/Framework/Sms/ReceiveSms?from=" + Sender + "&text=" + ReciveText + "&to=" + Query.Value3);
                }
            }
            catch (Exception e)
            {
                // return e.Message;
            }
        }
    }
}