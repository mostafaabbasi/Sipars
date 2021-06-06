using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using PrivateTraining.ServiceLayer.Interface;

namespace PrivateTraining.ServiceLayer.BLL
{
    public class Scheduler : ISchedule
    {
        public void Run()
        {
            //DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(null, 2);
            DateTimeOffset startTime = DateBuilder.FutureDate(2, IntervalUnit.Second);

            IJobDetail job = JobBuilder.Create<ScheduleJobs>()
                                       .WithIdentity("job1")
                                       .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                             .WithIdentity("trigger1")
                                             .StartAt(startTime)
                                             .WithSimpleSchedule(x => x.WithIntervalInSeconds(300).WithRepeatCount(1))
                                             .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(job, trigger);

            sc.Start();
        }
    }

}
