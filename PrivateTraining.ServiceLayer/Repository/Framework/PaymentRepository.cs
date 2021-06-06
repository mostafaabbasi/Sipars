using Microsoft.AspNet.Identity;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.ServiceLayer.Extention;
using PrivateTraining.ServiceLayer.Interface.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Repository.Framework
{
    public class PaymentRepository : IPayment
    {

        private readonly IUnitOfWork _uow;
        readonly IDbSet<PaymentOrder> _Payment;
        readonly IDbSet<PaymentOrderDetail> _paymentDetail;
        private readonly IIdentity _identity;

        public PaymentRepository(IUnitOfWork uow, IIdentity identity)
        {
            _uow = uow;
            _Payment = _uow.Set<PaymentOrder>();
            _identity = identity;
            _paymentDetail = _uow.Set<PaymentOrderDetail>();
        }

        public async Task AddPayment(PaymentOrder Model)
        {
            _Payment.Add(Model);
        }

        public IQueryable<PaymentOrder> GetAllPayment(int Id = 0)
        {
            if (Id != 0)
                return _Payment.Where(c => c.Id == Id);
            else
                return _Payment;
        }

        public void DeletePayment(int Id)
        {
            var Query2 = _paymentDetail.Find(Id);
            var Query = _Payment.Find(Query2.PaymentOrderId);
            Query.IsEnable = false;
            Query2.IsEnable = false;
            var Status = _uow.SaveAllChanges();

        }

        public void InactivePayment(int Id, byte Active = 0)
        {
            // var Query = _Payment.Where(c => c.Id == Id).FirstOrDefault();
            var Query = _paymentDetail.Where(c => c.Id == Id).FirstOrDefault();
            if (Query != null)
            {
                if (Active == Convert.ToByte(ActivePayment.No))
                    Query.ActivePayment = Convert.ToByte(ActivePayment.No);
                else if (Active == Convert.ToByte(ActivePayment.Ok))
                    Query.ActivePayment = Convert.ToByte(ActivePayment.Ok);
                else if (Active == Convert.ToByte(ActivePayment.Pending))
                    Query.ActivePayment = Convert.ToByte(ActivePayment.Pending);

                PersianDate Pd = new PersianDate();
                Query.SaveDateActivePayment = Pd.PersianDateLow();
                _uow.SaveAllChanges();
            }
        }

        public async Task<int> StatusProgramRegister(int Id, byte Param, string RejectDesc = "")
        {
            //  var Query = _Payment.Where(c => c.Id == Id).FirstOrDefault();
            var Query = _paymentDetail.Where(c => c.Id == Id).FirstOrDefault();
            Query.ActivePayment = Param;

            if (RejectDesc != null && RejectDesc != "")
            {
                Query.DesOfReject = RejectDesc;
                PersianDate PD = new PersianDate();
                Query.SaveDateActivePayment = PD.PersianDateLow() + " " + PD.CurrentTime();
                //SetProgramRegister(Id,Query);
            }
            return _uow.SaveAllChanges();
        }

        //  public async void ReturnPriceToPanelUser(int Id, PaymentOrderDetail Query)
        public async Task<int> ReturnPriceToPanelUser(int Id, int ProgramId, decimal price, decimal Fine, int RegisterUserId, string DesOfReject, string SaveDateActivePayment, byte ActivePayment)
        {
            PersianDate PD = new PersianDate();
            PaymentOrder Payment = new PaymentOrder();
            Payment.SaleOrderId = 0;
            Payment.OrderId = ProgramId;
            Payment.AllPrice = price;
            Payment.TransactionType = Convert.ToByte(TransactionType.Back);
            Payment.PaymentType = Convert.ToByte(PaymentType.Panel);
            Payment.SaveDatePayment = PD.PersianDateLow();
            Payment.UserIdPayment = _identity.GetUserId<int>();
            Payment.verified = Convert.ToInt32(Paymentverified.NotOnline);
            Payment.TransactionName = Convert.ToByte(TransactionName.Panel);
            await AddPayment(Payment);
            var t = _uow.SaveAllChanges();

            DataLayer.Context.ApplicationDbContext context = new DataLayer.Context.ApplicationDbContext();
            context.Database.ExecuteSqlCommand("exec SP_InsertPaymentOrderDetail " + Payment.Id + "," + ProgramId + "," + _identity.GetUserId<int>() + "," + RegisterUserId + ",'" + PD.PersianDateLow() + "'," + price + "," + Fine + "," + Convert.ToByte(PaymentType.Panel) + "," +
                 Convert.ToByte(TransactionType.Back) + "," + ActivePayment + ",'" + DesOfReject + "','" + SaveDateActivePayment +
                 "'," + Convert.ToByte(TransactionName.Panel));

            return 1;
        }

        public IQueryable<PaymentOrderDetail> GetAllPaymentDetail(int Id = 0)
        {
            if (Id != 0)
                return _paymentDetail.Where(c => c.Id == Id);
            else
                return _paymentDetail;
        }

        /// <summary>
        /// نمایش تعداد ثبت نام های جدید در آلارم ها
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        public int GetCountRegister(string Role = "")
        {
            var U = _identity.GetUserId<int>();
            var s = Convert.ToByte(ActivePayment.Pending);
            var d = Convert.ToByte(ProgramStatus.Show);

            if (Role == "Modrator")  //  نمایش ثبت نامی های باشگاه های مربوط ب مدیر باشگاه
                return _paymentDetail.Where(c => c.ActivePayment == s && c.Programs.Status == d &&
                                            c.Programs.Clubs.ClubManagers.Any(x => x.UserId == U && x.ClubId == c.Programs.Clubs.Id && x.IsEnable == true)
                                            && c.IsEnable == true).Count();
            else
                return _paymentDetail.Where(c => c.ActivePayment == s && c.Programs.Status == d && c.IsEnable==true).Count();
        }

        /// <summary>
        /// نمایش تعداد تراکنش های جدید در آلارم ها
        /// </summary>
        /// <returns></returns>
        public int GetCountTransaction()
        {
            var T = Convert.ToInt32(TransactionType.Back);
            var R = Convert.ToInt32(TransactionType.Received);
            var N = Convert.ToInt32(TransactionName.Panel);
            var s = Convert.ToByte(ActivePayment.Pending);
            var A = Convert.ToInt32(TransactionType.Account);

            return _paymentDetail.Where(c => (c.TransactionType == A || (c.TransactionName == N && c.TransactionType == R)) && c.ActivePayment == s).Count();
        }

        #region Golbahar

        public IQueryable<PaymentOrder> GetAllPaymentUser(int UserId)
        {
                return _Payment.Where(c => c.OrderId  == UserId);
        }

        #endregion


    }
}
