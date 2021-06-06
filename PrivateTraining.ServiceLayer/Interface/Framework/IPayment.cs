using PrivateTraining.DomainClasses.Entities.FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Interface.Framework
{
    public interface IPayment
    {
        IQueryable<PaymentOrder> GetAllPayment(int Id);
        Task AddPayment(PaymentOrder Model);
        void DeletePayment(int Id);
        void InactivePayment(int Id,byte Active=0);
        Task<int> StatusProgramRegister(int Id, byte Param, string RejectDesc = "");
        // void ReturnPriceToPanelUser(int Id, PaymentOrderDetail Query);
        Task<int> ReturnPriceToPanelUser(int Id, int ProgramId, decimal price, decimal Fine, int RegisterUserId, string DesOfReject, string SaveDateActivePayment,byte ActivePayment);
        IQueryable<PaymentOrderDetail> GetAllPaymentDetail(int Id);
        int GetCountRegister(string Role="");
        int GetCountTransaction();

        #region  Golbahar

        IQueryable<PaymentOrder> GetAllPaymentUser(int UserId );

        #endregion

    }
}
