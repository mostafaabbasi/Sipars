using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Quartz;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.ServiceLayer.Interface;

namespace PrivateTraining.ServiceLayer.BLL
{
    public class ScheduleJobs : IJob
    {
        private IUnitOfWork _uow;
        private readonly IApplicationUserManager _userManager;

        public ScheduleJobs(IUnitOfWork uow, IApplicationUserManager userManager)
        {
            _uow = uow;
            _userManager = userManager;
        }

        public void Execute(IJobExecutionContext context)
        {
            // PrivateTraining Pt = new PrivateTraining(_uow,_userManager);

            // فراخوانی پروسیجر برای غیر فعال کردن رکوردهای درخواستی که دقیقه شان از 5 دقیقه گذشته
            // فرآیند مانند فرستادن پیامک عدد 12 توسط خدمت دهنده است


        }
    }

}
