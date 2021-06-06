using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateTraining.Areas.PrivateTrain.Models
{
    public class PrivateViewModel
    {
    }

    public class PaymentListViewModel
    {
        public int Price { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public int TransactionNumber { get; set; }

        // تایید شده 1 ,  رد شده 0
        public byte Status { get; set; }

        public int MemberId { get; set; }

        public int ModratorId { get; set; }

        public string CodeBank { get; set; }

        public int DeptId { get; set; }

        public int verified { get; set; }

        public int paymentId { get; set; }
        public int DebtId { get; set; }
        public int? ServiceProviderId { get; set; }
        public int? ServiceReceiverServiceLocationId { get; set; }
        public int CalculatePricePayment { get; set; }
        public double TotalCostDebt { get; set; }
        public int PercentOfSharesDebt { get; set; }
        public double CompanyCostDebt { get; set; }

        public string ModratorName { get; set; }
        public string MemberName { get; set; }

    }
}