using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Interface.PrivateTraining
{
    public interface IServiceWaitingList
    {
        /// <summary>
        /// دریافت لیست خدمتیاران لیست انتظار سرویس
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <returns></returns>
        Task<List<ServiceWaitingList>> GetWaitListService(int ServiceId);

        /// <summary>
        /// تائید درخواست سرویس
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="ProviderId"></param>
        /// <returns></returns>
        Task ProviderAcceptRequest(int ServiceId, int ProviderId);

        /// <summary>
        /// تائید اولیه درخواست توسط خدمتیار جهت مذاکره با مشتری
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="ProviderId"></param>
        /// <returns></returns>
        Task ProviderPendRequest(int ServiceId, int ProviderId);

        /// <summary>
        /// افزودن لیست خدمتیاران به لیست انتظار
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="ProviderId"></param>
        /// <returns></returns>
        Task AddProviderToWaitListService(int ServiceId, List<int> ProviderId);

        /// <summary>
        /// حذف یا غیر فعال کردن لیست خدمتیاران از لیست انتظار
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="ProviderId"></param>
        /// <returns></returns>
        Task DeactiveProviderFromWaitListService(int ServiceId, List<int> ProviderId);
    }
}
