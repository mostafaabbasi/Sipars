using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web;
using PrivateTraining.ServiceLayer.Interface.Security;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.ServiceLayer.Interface.Framework;
using System.Web.Configuration;

namespace PrivateTraining.ServiceLayer.BLL
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeUserAttribute : AuthorizeAttribute //FilterAttribute, IExceptionFilter //AuthorizeAttribute
    {

        public IAccessLevel _access { get; set; }
        public IGroupPolicy _grouppolicy { get; set; }
        public IAction _action { get; set; }
        bool IsAjax = false;
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            IsAjax = httpContext.Request.IsAjaxRequest();
            return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //// Check Activation Code
            //try
            //{
            //    string Name = Cryptography.EncryptString("sepas");// WebConfigurationManager.AppSettings["Name"];
            //    string ActivationCodeWebconfig = WebConfigurationManager.AppSettings["ActivationCode"];
            //    MachineInfo machin = new MachineInfo();
            //    string HDD = Cryptography.EncryptString(machin.GetInfoSerialNumber());
            //    string ActivationCode = Cryptography.EncryptString(HDD + Name + "Sepas!@#");

            //    if (ActivationCodeWebconfig == ActivationCode)
            //        return;
            //    else
            //    {
            //        if (IsAjax)
            //        {
            //            string ActiveCode = "نرم افزار شما Active نمی باشد";
            //            ActiveCode += "کد:" + machin.GetInfoSerialNumber();
            //            // For AJAX requests, we're throwing custome applicationexception that will be skiped in Application_Error event
            //            // & will be handled in java script common code on _LayOut.cshtml to redirect user to login page.
            //            filterContext.Result = new EmptyResult();
            //            filterContext.HttpContext.Response.Clear();
            //            //filterContext.HttpContext.Response.Write("<div class='alert alert-dismissible alert-danger'>کاربر گرامی <button type='button' class='close' data-dismiss='alert'>×</button> شما سطح دسترسی برای انجام عملیات را ندارید - کد دسترسی " + Code + "</div>");
            //            filterContext.HttpContext.Response.Write(ActiveCode);
            //            filterContext.HttpContext.Response.StatusCode = 100;
            //        }
            //        else
            //        {
            //            string ActiveCode = "نرم افزار شما Active نمی باشد";
            //            ActiveCode += "کد:" + machin.GetInfoSerialNumber();
            //            ViewResult result = new ViewResult();
            //            result.ViewName = "~/Views/Shared/Error.cshtml";
            //            result.ViewBag.ErrorMessage = ActiveCode;
            //            filterContext.Result = result;
            //            return;
                        
            //        }
            //    }

            //}
            //catch (Exception)
            //{

            //    string ActiveCode = "خطا به دلیل نا مشخص به وجود آمده است";
            //    ViewResult result = new ViewResult();
            //    result.ViewName = "~/Views/Shared/Error.cshtml";
            //    result.ViewBag.ErrorMessage = ActiveCode;
            //    filterContext.Result = result;
            //    return;
            //}


            try
            {
                //--------------- Call Action  ------------------------------------
                string ActionName = filterContext.ActionDescriptor.ActionName.ToString();
                string Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                var ActionList = GetActionId(ActionName, Controller).Result;
                //--------------- Call Action  ------------------------------------
                if (Controller == "Menu")
                    return;

                bool ResualtAccessGroup = false;

                if (filterContext.ActionDescriptor.GetCustomAttributes(true).Any(rec => rec is AllowAnonymousAttribute))
                    return;

                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    HttpContext.Current.Response.Redirect("~/Account/Login", true);
                }
                else if (HttpContext.Current.User.IsInRole("Admin") || HttpContext.Current.User.IsInRole("Administrator"))
                {
                    return;
                }
                else
                {



                    int UserId = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserId());

                    var GroupPolicy = GetGroupPolicy(UserId).Result;

                    var Access = _access.ListAccesslevel();
                    // سطح دسترسی در سطح کاربر
                    var AccessUser = Access.OfType<AccessLevelUser>().Where(c => c.UserId == UserId && c.ActionId == ActionList.Id).FirstOrDefault();
                    // اگر در جدول کاربر این سطح دسترسی وارد نشده بود گروه های کاربری را چک میکنیم
                    if (AccessUser == null)
                    {
                        var AccessGroup = Access.OfType<AccessLevelGroup>().Where(c => GroupPolicy.Contains(c.GroupId) && c.ActionId == ActionList.Id).ToList();
                        ResualtAccessGroup = CheckAccessGroupPolicy(AccessGroup);
                    }
                    else  // سطح دسترسی در سطح کاربر
                    {
                        if (AccessUser.IsEnable)
                            ResualtAccessGroup = true;
                    }

                    if (!ResualtAccessGroup)
                    {
                        string Code = "";
                        if (ActionList != null)
                            Code = ActionList.AccessCode;
                        else
                            Code = "Action مورد نظر در بانک اطلاعاتی ذخیره نگردیده است";

                        if (IsAjax)
                        {
                            // For AJAX requests, we're throwing custome applicationexception that will be skiped in Application_Error event
                            // & will be handled in java script common code on _LayOut.cshtml to redirect user to login page.
                            filterContext.Result = new EmptyResult();
                            filterContext.HttpContext.Response.Clear();
                            //filterContext.HttpContext.Response.Write("<div class='alert alert-dismissible alert-danger'>کاربر گرامی <button type='button' class='close' data-dismiss='alert'>×</button> شما سطح دسترسی برای انجام عملیات را ندارید - کد دسترسی " + Code + "</div>");
                            filterContext.HttpContext.Response.Write("<div>کاربر گرامی شما سطح دسترسی برای انجام عملیات را ندارید - کد دسترسی " + Code + "</div>");
                            filterContext.HttpContext.Response.StatusCode = 403;
                        }
                        else
                        {
                            //---------- فعلا برداشته شد
                            ViewResult result = new ViewResult();
                            result.ViewName = "~/Views/Shared/Error.cshtml";
                            result.ViewBag.ErrorMessage = " کاربر گرامی شما سطح دسترسی برای نمایش ندارید - کد دسترسی" + Code;
                            filterContext.Result = result;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public async Task<List<int>> GetGroupPolicy(int UserId)
        {
            var List = await _grouppolicy.GetAllIGroupPolicyUser();
            //return List.Where(c => c.GroupPolicyUsers.Any(a => a.UserId == UserId)).ToList();
            var ListInt = List.Where(c => c.GroupPolicyUsers.Any(a => a.UserId == UserId)).ToList();
            var Listintint = ListInt.Select(b => b.Id).ToList();
            return Listintint;
        }

        public static bool CheckAccessGroupPolicy(List<AccessLevelGroup> GroupPolicy)
        {
            bool Bool = false;
            foreach (var item in GroupPolicy)
            {
                if (item.IsEnable)
                    return true;
                else
                    Bool = false;
            }
            return Bool;
        }


        public async Task<global::PrivateTraining.DomainClasses.Entities.FrameWork.Action> GetActionId(string ActionName, string Controller)
        {
            try {
                var ListActions = await _action.ListActions();
                return ListActions.Where(c => c.Actionname == ActionName && c.Controller == Controller).FirstOrDefault();
            }
            catch(Exception ex)
            {
                global::PrivateTraining.DomainClasses.Entities.FrameWork.Action a = new global::PrivateTraining.DomainClasses.Entities.FrameWork.Action();
                return a;
            }

        }

        //public static IContainer Initialize()
        //{
        //    ObjectFactory
        //}

    }
}
