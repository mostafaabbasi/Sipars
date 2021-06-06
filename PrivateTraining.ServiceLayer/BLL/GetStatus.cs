using PrivateTraining.ServiceLayer.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.BLL
{
    public class GetStatus
    {
        public string GetStatusLeaveRequest(StatusColorLeave status)
        {
            string StatusLeaveRequest = "";
            switch (status)
            {
                case StatusColorLeave.Gray:
                    StatusLeaveRequest = "مرخصی در این تاریخ مجاز نیست";
                    break;
                case StatusColorLeave.Green:
                    StatusLeaveRequest = "درخواست مرخصی مجاز می باشد.";
                    break;
                case StatusColorLeave.Orange:
                    StatusLeaveRequest = "در حال بررسی مرخصی های ثبت شده";
                    break;
                case StatusColorLeave.Red:
                    StatusLeaveRequest = "ظرفیت تکمیل شده است.";
                    break;
            }

            return StatusLeaveRequest;
        }
    }
}
