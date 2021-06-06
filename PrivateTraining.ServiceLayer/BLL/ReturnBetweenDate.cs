using PrivateTraining.DomainClasses.EntitiesView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.BLL
{
    public static class ReturnBetweenDate
    {
        public static List<Days> ReturnDays(int NumberOfDays, int StartDay, string input_date)
        {
           
            List<Days> ListDays = new List<Days>();

            for (int i = 1; i < NumberOfDays +StartDay; i++)
            {
                Days Day = new Days();
                System.Globalization.PersianCalendar g = new System.Globalization.PersianCalendar();
                System.DateTime in_date = default(System.DateTime);
                string y = null;
                string m = null;
                string d = null;
                string r = null;

                try
                {
                    y = input_date.Substring(0, 4);
                    //جدا سازی 4 رقم عدد سال 
                    m = input_date.Substring(5, 2);
                    //جدا سازی دو رقم عدد ماه 
                    d = input_date.Substring(8, 2);
                    //جدا سازی 2 رقم عدد روز 
                    //------------------------------------------------- 
                    in_date = g.ToDateTime(Convert.ToInt32(y), Convert.ToInt32(m), Convert.ToInt32(d), 0, 0, 0, 0);
                    
                    in_date = g.AddDays(in_date, i);
                    r = g.GetYear(in_date).ToString() + "/" + zero_adder(g.GetMonth(in_date)) + "/" + zero_adder(g.GetDayOfMonth(in_date));
                    if (i >= StartDay)
                    {
                        Day.Date =r;
                        Day.DayOfWeek = in_date.DayOfWeek.ToString();
                        Day.Name = "";
                        ListDays.Add(Day);
                    }                  

                }
                catch (Exception ex)
                {
                    // Interaction.MsgBox("بروز خطا هنگام محاسبه تاریخ _ " + ex.ToString());
                    r = "نامشخص";
                }

            }

            return ListDays;
        }


        public static string zero_adder(int i)
        {
            string a = null;
            a = "";
            switch (i)
            {
                case 1:
                    a = "01";
                    break;
                case 2:
                    a = "02";
                    break;
                case 3:
                    a = "03";
                    break;
                case 4:
                    a = "04";
                    break;
                case 5:
                    a = "05";
                    break;
                case 6:
                    a = "06";
                    break;
                case 7:
                    a = "07";
                    break;
                case 8:
                    a = "08";
                    break;
                case 9:
                    a = "09";
                    break;
                default:
                    a = i.ToString();
                    break;
            }

            return a;
        }


    }
}
