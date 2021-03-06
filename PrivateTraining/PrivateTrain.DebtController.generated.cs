// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace PrivateTraining.Areas.PrivateTrain.Controllers
{
    public partial class DebtController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected DebtController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult AddPayment()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AddPayment);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Threading.Tasks.Task<System.Web.Mvc.JsonResult> LoadUserforsms()
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.LoadUserforsms);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.JsonResult);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public DebtController Actions { get { return MVC.PrivateTrain.Debt; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "PrivateTrain";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Debt";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Debt";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string GetListDebts = "GetListDebts";
            public readonly string ListDetailDebts = "ListDetailDebts";
            public readonly string ListDebtUsers = "ListDebtUsers";
            public readonly string AddPayment = "AddPayment";
            public readonly string LoadUserforsms = "LoadUserforsms";
            public readonly string GetListPayments = "GetListPayments";
            public readonly string StiPrintPayment = "StiPrintPayment";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string GetListDebts = "GetListDebts";
            public const string ListDetailDebts = "ListDetailDebts";
            public const string ListDebtUsers = "ListDebtUsers";
            public const string AddPayment = "AddPayment";
            public const string LoadUserforsms = "LoadUserforsms";
            public const string GetListPayments = "GetListPayments";
            public const string StiPrintPayment = "StiPrintPayment";
        }


        static readonly ActionParamsClass_ListDetailDebts s_params_ListDetailDebts = new ActionParamsClass_ListDetailDebts();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListDetailDebts ListDetailDebtsParams { get { return s_params_ListDetailDebts; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListDetailDebts
        {
            public readonly string StateId = "StateId";
            public readonly string CityId = "CityId";
            public readonly string LocationId = "LocationId";
            public readonly string ServiceId = "ServiceId";
            public readonly string TypeDebt = "TypeDebt";
            public readonly string Name = "Name";
            public readonly string PriceDebtMoreThan = "PriceDebtMoreThan";
            public readonly string CountDebtMoreThan = "CountDebtMoreThan";
            public readonly string DateDebtMoreThan = "DateDebtMoreThan";
        }
        static readonly ActionParamsClass_ListDebtUsers s_params_ListDebtUsers = new ActionParamsClass_ListDebtUsers();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListDebtUsers ListDebtUsersParams { get { return s_params_ListDebtUsers; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListDebtUsers
        {
            public readonly string StateId = "StateId";
            public readonly string CityId = "CityId";
            public readonly string LocationId = "LocationId";
            public readonly string ServiceId = "ServiceId";
        }
        static readonly ActionParamsClass_AddPayment s_params_AddPayment = new ActionParamsClass_AddPayment();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_AddPayment AddPaymentParams { get { return s_params_AddPayment; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_AddPayment
        {
            public readonly string ListId = "ListId";
        }
        static readonly ActionParamsClass_LoadUserforsms s_params_LoadUserforsms = new ActionParamsClass_LoadUserforsms();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_LoadUserforsms LoadUserforsmsParams { get { return s_params_LoadUserforsms; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_LoadUserforsms
        {
            public readonly string DebtId = "DebtId";
        }
        static readonly ActionParamsClass_GetListPayments s_params_GetListPayments = new ActionParamsClass_GetListPayments();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetListPayments GetListPaymentsParams { get { return s_params_GetListPayments; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetListPayments
        {
            public readonly string PaymentType = "PaymentType";
            public readonly string UserId = "UserId";
            public readonly string FromTime = "FromTime";
            public readonly string ToTime = "ToTime";
        }
        static readonly ActionParamsClass_StiPrintPayment s_params_StiPrintPayment = new ActionParamsClass_StiPrintPayment();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_StiPrintPayment StiPrintPaymentParams { get { return s_params_StiPrintPayment; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_StiPrintPayment
        {
            public readonly string PaymentType = "PaymentType";
            public readonly string UserId = "UserId";
            public readonly string FromTime = "FromTime";
            public readonly string ToTime = "ToTime";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string _ListBank = "_ListBank";
                public readonly string _SendSms = "_SendSms";
                public readonly string GetListDebts = "GetListDebts";
                public readonly string GetListPayments = "GetListPayments";
            }
            public readonly string _ListBank = "~/Areas/PrivateTrain/Views/Debt/_ListBank.cshtml";
            public readonly string _SendSms = "~/Areas/PrivateTrain/Views/Debt/_SendSms.cshtml";
            public readonly string GetListDebts = "~/Areas/PrivateTrain/Views/Debt/GetListDebts.cshtml";
            public readonly string GetListPayments = "~/Areas/PrivateTrain/Views/Debt/GetListPayments.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_DebtController : PrivateTraining.Areas.PrivateTrain.Controllers.DebtController
    {
        public T4MVC_DebtController() : base(Dummy.Instance) { }

        [NonAction]
        partial void GetListDebtsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult GetListDebts()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GetListDebts);
            GetListDebtsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ListDetailDebtsOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, int StateId, int CityId, int LocationId, int ServiceId, int TypeDebt, string Name, int PriceDebtMoreThan, int CountDebtMoreThan, string DateDebtMoreThan);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.JsonResult> ListDetailDebts(int StateId, int CityId, int LocationId, int ServiceId, int TypeDebt, string Name, int PriceDebtMoreThan, int CountDebtMoreThan, string DateDebtMoreThan)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ListDetailDebts);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "StateId", StateId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "CityId", CityId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "LocationId", LocationId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ServiceId", ServiceId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "TypeDebt", TypeDebt);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "Name", Name);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "PriceDebtMoreThan", PriceDebtMoreThan);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "CountDebtMoreThan", CountDebtMoreThan);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "DateDebtMoreThan", DateDebtMoreThan);
            ListDetailDebtsOverride(callInfo, StateId, CityId, LocationId, ServiceId, TypeDebt, Name, PriceDebtMoreThan, CountDebtMoreThan, DateDebtMoreThan);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.JsonResult);
        }

        [NonAction]
        partial void ListDebtUsersOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, int StateId, int CityId, int LocationId, int ServiceId);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.JsonResult> ListDebtUsers(int StateId, int CityId, int LocationId, int ServiceId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ListDebtUsers);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "StateId", StateId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "CityId", CityId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "LocationId", LocationId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ServiceId", ServiceId);
            ListDebtUsersOverride(callInfo, StateId, CityId, LocationId, ServiceId);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.JsonResult);
        }

        [NonAction]
        partial void AddPaymentOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int[] ListId);

        [NonAction]
        public override System.Web.Mvc.ActionResult AddPayment(int[] ListId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AddPayment);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ListId", ListId);
            AddPaymentOverride(callInfo, ListId);
            return callInfo;
        }

        [NonAction]
        partial void LoadUserforsmsOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, int[] DebtId);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.JsonResult> LoadUserforsms(int[] DebtId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.LoadUserforsms);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "DebtId", DebtId);
            LoadUserforsmsOverride(callInfo, DebtId);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.JsonResult);
        }

        [NonAction]
        partial void GetListPaymentsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, byte PaymentType, int UserId, string FromTime, string ToTime);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> GetListPayments(byte PaymentType, int UserId, string FromTime, string ToTime)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GetListPayments);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "PaymentType", PaymentType);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "UserId", UserId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "FromTime", FromTime);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ToTime", ToTime);
            GetListPaymentsOverride(callInfo, PaymentType, UserId, FromTime, ToTime);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }

        [NonAction]
        partial void StiPrintPaymentOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, byte PaymentType, int UserId, string FromTime, string ToTime);

        [NonAction]
        public override System.Web.Mvc.ActionResult StiPrintPayment(byte PaymentType, int UserId, string FromTime, string ToTime)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.StiPrintPayment);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "PaymentType", PaymentType);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "UserId", UserId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "FromTime", FromTime);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ToTime", ToTime);
            StiPrintPaymentOverride(callInfo, PaymentType, UserId, FromTime, ToTime);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
