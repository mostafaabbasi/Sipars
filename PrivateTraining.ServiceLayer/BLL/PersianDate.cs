using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.BLL
{

    public class PersianDate
    {
        /// <summary>
        /// تاریخ امروز شمسی
        /// </summary>
        /// <returns></returns>
        public string PersianDateLow()
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime thisDate = DateTime.Now;


            return pc.GetYear(thisDate) + "/" + zero_adder(pc.GetMonth(thisDate)) + "/" + zero_adder(pc.GetDayOfMonth(thisDate));
        }

        public string PersianDayNameLow(DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime thisDate = DateTime.Now;

            var DayNameMiladi = pc.GetDayOfWeek(date).ToString();
            return PersianDayName(DayNameMiladi);
        }

        /// <summary>
        /// ساعت جاری
        /// </summary>
        /// <returns></returns>
        public string CurrentTime()
        {
            //string time = System.DateTime.Now.ToLongTimeString().Replace(" ", "").Replace("P", "").Replace("A", "").Replace("M", "");
            string time = DateTime.Now.ToString("HH:mm:ss");

            if (time.Length < 8)
            {
                time = time.Substring(0, 7);
                time = "0" + time;
            }
            else
                time = time.Substring(0, 8);

            return time;
        }

        #region Increase-Date

        /// <summary>
        /// افزودن و کاهش روز - ماه - سال به تاریخ شمسی
        /// اگر می خواهید مثلا فقط روز اضافه کنید در اینصورت آرگومان مربوط به سال و ماه را صفر بگذارید
        /// </summary>
        /// <param name="input_date">تاریخ به شمسی در این قالب: 1391/01/01</param>
        /// <param name="year_add_count">تعداد سالی که می خواهیم کم یا زیاد نماییم . اگر می خواهیم سال اضافه شود عدد را مثبت و اگر بخواهیم کم شود عدد را منفی می نویسیم</param>
        /// <param name="month_add_count">تعداد ماه که می خواهیم کم یا زیاد نماییم . اگر می خواهیم ماه اضافه شود عدد را مثبت و اگر بخواهیم کم شود عدد را منفی می نویسیم</param>
        /// <param name="day_add_count"> تعداد روز که می خواهیم کم یا زیاد نماییم . اگر می خواهیم روز اضافه شود عدد را مثبت و اگر بخواهیم کم شود عدد را منفی می نویسیم</param>
        /// <returns>خروجی تابع بصورت استرینگ بوده و تاریخ شمسی بعد از افزودن یا کاهش می باشد</returns>
        public string date_adder_top_new(string input_date, int year_add_count, int month_add_count, int day_add_count)
        {
            string y = null;
            string m = null;
            string d = null;
            string r = null;
            System.Globalization.PersianCalendar g = new System.Globalization.PersianCalendar();
            System.DateTime in_date = default(System.DateTime);

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
                in_date = g.AddYears(in_date, year_add_count);
                in_date = g.AddMonths(in_date, month_add_count);
                in_date = g.AddDays(in_date, day_add_count);
                r = g.GetYear(in_date).ToString() + "/" + zero_adder(g.GetMonth(in_date)) + "/" + zero_adder(g.GetDayOfMonth(in_date));
            }
            catch (Exception ex)
            {
                // Interaction.MsgBox("بروز خطا هنگام محاسبه تاریخ _ " + ex.ToString());
                r = "نامشخص";
            }
            return r;
        }

        public string zero_adder(int i)
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

        #endregion

        public string ConvertPersianNember(string Nember)
        {
            if (Nember != "" && Nember != null)
                Nember = Nember.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");
            return Nember;
        }

        public string ConvertEnToFaNumber(string Nember)
        {
            if (Nember != "" && Nember != null)
                Nember = Nember.Replace("0", "۰").Replace("1", "۱").Replace("2", "۲").Replace("3", "۳").Replace("4", "۴").Replace("5", "۵").Replace("6", "۶").Replace("7", "۷").Replace("8", "۸").Replace("9", "۹");
            return Nember;
        }

        public string ConvertFaToEnNumber(string Nember)
        {
            if (Nember != "" && Nember != null)
                Nember = Nember.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");
            return Nember;
        }

        public string PersianDayName(string name)
        {
            string DayName = "";
            switch (name)
            {
                case "Saturday":
                    DayName = "شنبه";
                    break;
                case "Sunday":
                    DayName = "یکشنبه";
                    break;
                case "Monday":
                    DayName = "دوشنبه";
                    break;
                case "Tuesday":
                    DayName = "سه شنبه";
                    break;
                case "Wednesday":
                    DayName = "چهارشنبه";
                    break;
                case "Thursday":
                    DayName = "پنج شنبه";
                    break;
                case "Friday":
                    DayName = "جمعه";
                    break;
            }

            return DayName;
        }

        public string DayNameOfWeek(byte Day)
        {
            string Name = "";
            switch (Day)
            {
                case 1:
                    Name = "فروردین";
                    break;
                case 2:
                    Name = "اردیبهشت";
                    break;
                case 3:
                    Name = "خرداد";
                    break;
                case 4:
                    Name = "تیر";
                    break;
                case 5:
                    Name = "مرداد";
                    break;
                case 6:
                    Name = "شهریور";
                    break;
                case 7:
                    Name = "مهر";
                    break;
                case 8:
                    Name = "آبان";
                    break;
                case 9:
                    Name = "آذر";
                    break;
                case 10:
                    Name = "دی";
                    break;
                case 11:
                    Name = "بهمن";
                    break;
                case 12:
                    Name = "اسفند";
                    break;

            }
            return Name;
        }

        public DateTime shamsiToMiladi(string date)
        {
            PersianCalendar pc = new PersianCalendar();
            var DateEn = ConvertFaToEnNumber(date);
            DateTime persianDateTime = DateTime.Parse(DateEn);
            var temp = pc.ToDateTime(persianDateTime.Year, persianDateTime.Month, persianDateTime.Day, persianDateTime.Hour, persianDateTime.Minute, persianDateTime.Second, persianDateTime.Millisecond);
            return temp;
        }

        /// <summary>
        /// بدست آوردن مدت زمان بین دو تاریخ شمسی
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public TimeSpan CalculateTime(string FromDate, string ToDate)
        {
            DateTime First = shamsiToMiladi(FromDate);
            DateTime Second = shamsiToMiladi(ToDate);
            TimeSpan Ts = Second - First;
            return Ts;
        }

        /// <summary>
        /// بدست آوردن مدت زمان امروز و تاریخ آینده
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public TimeSpan CalculateTimes(string FromDatee)
        {
            DateTime First = shamsiToMiladi(PersianDateLow() + " " + CurrentTime());
            DateTime Second = shamsiToMiladi(FromDatee);
            TimeSpan Ts = Second - First;
            return Ts;
        }

        public string ConvertToCurrency(int Money)
        {
            string result = "0";
            // if (Money > 0)
            result = String.Format("{0:##,##}", Money);
            return result;
        }

        public string ConvertToCurrencyDouble(Nullable<double> Money)
        {
            string result = "0";
            //  if (Money > 0)
            if (Money != null)
                result = String.Format("{0:##,##}", Money);
            return result;
        }

        public string ConvertToCurrencyDecimal(decimal Money)
        {
            string result = "0";
            // if (Money > 0)
            result = String.Format("{0:##,##}", Money);
            return result;
        }

        public string ConvertToEasternArabicNumerals(string input)
        {
            System.Text.UTF8Encoding utf8Encoder = new UTF8Encoding();
            System.Text.Decoder utf8Decoder = utf8Encoder.GetDecoder();
            System.Text.StringBuilder convertedChars = new System.Text.StringBuilder();
            char[] convertedChar = new char[1];
            byte[] bytes = new byte[] { 217, 160 };
            char[] inputCharArray = input.ToCharArray();
            foreach (char c in inputCharArray)
            {
                if (char.IsDigit(c))
                {
                    bytes[1] = Convert.ToByte(160 + char.GetNumericValue(c));
                    utf8Decoder.GetChars(bytes, 0, 2, convertedChar, 0);
                    convertedChars.Append(convertedChar[0]);
                }
                else
                {
                    convertedChars.Append(c);
                }
            }
            return convertedChars.ToString();
        }

    }
}
