using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.BLL
{
    public class ConvertMinToTime
    {
        public string MinToTime(int NumberMin) {
            var NumberSecond = NumberMin * 60;
            int Day = NumberSecond / 26400;
            int Temp1 = NumberSecond % 26400;
            int H = Temp1 /3600 ;
            int Temp2= Temp1 % 3600;
            int min = Temp2 / 60;
            int s= Temp2 % 60;
            return ( Day + "روز و " + H +  "ساعت و" + min + "دقیقه و" + s + "ثانیه");
        }
    }
}
