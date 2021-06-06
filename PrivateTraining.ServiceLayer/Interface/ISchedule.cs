using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Interface
{
    public interface ISchedule
    {
        void Run();

        // ----- برای اجرای فرآیند باید این دوخط در پروژه اصلی گذاشته شود 
        // ISchedule myTask = new HelloSchedule();
        //  myTask.Run();
        //-------------
    }
}
