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
namespace PrivateTraining.Areas.Framework.Controllers
{
    public partial class MessageController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected MessageController(Dummy d) { }

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
        public virtual System.Web.Mvc.JsonResult AddMessages()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.AddMessages);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Threading.Tasks.Task<System.Web.Mvc.JsonResult> DeleteMessages()
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.DeleteMessages);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.JsonResult);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Threading.Tasks.Task<System.Web.Mvc.JsonResult> ReadMessages()
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ReadMessages);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.JsonResult);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public MessageController Actions { get { return MVC.Framework.Message; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Framework";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Message";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Message";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string MessagForAdmin = "MessagForAdmin";
            public readonly string ListMessages = "ListMessages";
            public readonly string ListJsonMessages = "ListJsonMessages";
            public readonly string AddMessages = "AddMessages";
            public readonly string DeleteMessages = "DeleteMessages";
            public readonly string ReadMessages = "ReadMessages";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string MessagForAdmin = "MessagForAdmin";
            public const string ListMessages = "ListMessages";
            public const string ListJsonMessages = "ListJsonMessages";
            public const string AddMessages = "AddMessages";
            public const string DeleteMessages = "DeleteMessages";
            public const string ReadMessages = "ReadMessages";
        }


        static readonly ActionParamsClass_ListJsonMessages s_params_ListJsonMessages = new ActionParamsClass_ListJsonMessages();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListJsonMessages ListJsonMessagesParams { get { return s_params_ListJsonMessages; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListJsonMessages
        {
            public readonly string UserId = "UserId";
            public readonly string ReadMessages = "ReadMessages";
        }
        static readonly ActionParamsClass_AddMessages s_params_AddMessages = new ActionParamsClass_AddMessages();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_AddMessages AddMessagesParams { get { return s_params_AddMessages; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_AddMessages
        {
            public readonly string Message = "Message";
        }
        static readonly ActionParamsClass_DeleteMessages s_params_DeleteMessages = new ActionParamsClass_DeleteMessages();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_DeleteMessages DeleteMessagesParams { get { return s_params_DeleteMessages; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_DeleteMessages
        {
            public readonly string MessageId = "MessageId";
        }
        static readonly ActionParamsClass_ReadMessages s_params_ReadMessages = new ActionParamsClass_ReadMessages();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ReadMessages ReadMessagesParams { get { return s_params_ReadMessages; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ReadMessages
        {
            public readonly string MessageId = "MessageId";
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
                public readonly string _DelMessages = "_DelMessages";
                public readonly string _ShowMessages = "_ShowMessages";
                public readonly string ListMessages = "ListMessages";
                public readonly string MessagForAdmin = "MessagForAdmin";
            }
            public readonly string _DelMessages = "~/Areas/Framework/Views/Message/_DelMessages.cshtml";
            public readonly string _ShowMessages = "~/Areas/Framework/Views/Message/_ShowMessages.cshtml";
            public readonly string ListMessages = "~/Areas/Framework/Views/Message/ListMessages.cshtml";
            public readonly string MessagForAdmin = "~/Areas/Framework/Views/Message/MessagForAdmin.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_MessageController : PrivateTraining.Areas.Framework.Controllers.MessageController
    {
        public T4MVC_MessageController() : base(Dummy.Instance) { }

        [NonAction]
        partial void MessagForAdminOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> MessagForAdmin()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.MessagForAdmin);
            MessagForAdminOverride(callInfo);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }

        [NonAction]
        partial void ListMessagesOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> ListMessages()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ListMessages);
            ListMessagesOverride(callInfo);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }

        [NonAction]
        partial void ListJsonMessagesOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, int UserId, byte ReadMessages);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.JsonResult> ListJsonMessages(int UserId, byte ReadMessages)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ListJsonMessages);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "UserId", UserId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ReadMessages", ReadMessages);
            ListJsonMessagesOverride(callInfo, UserId, ReadMessages);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.JsonResult);
        }

        [NonAction]
        partial void AddMessagesOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, PrivateTraining.DomainClasses.Entities.FrameWork.Message Message);

        [NonAction]
        public override System.Web.Mvc.JsonResult AddMessages(PrivateTraining.DomainClasses.Entities.FrameWork.Message Message)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.AddMessages);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "Message", Message);
            AddMessagesOverride(callInfo, Message);
            return callInfo;
        }

        [NonAction]
        partial void DeleteMessagesOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, string[] MessageId);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.JsonResult> DeleteMessages(string[] MessageId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.DeleteMessages);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "MessageId", MessageId);
            DeleteMessagesOverride(callInfo, MessageId);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.JsonResult);
        }

        [NonAction]
        partial void ReadMessagesOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, int MessageId);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.JsonResult> ReadMessages(int MessageId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.ReadMessages);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "MessageId", MessageId);
            ReadMessagesOverride(callInfo, MessageId);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.JsonResult);
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
