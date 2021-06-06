using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.ServiceLayer.Interface.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PrivateTraining.ServiceLayer.Interface;
using PrivateTraining.BussinessLayer.Generic;
using PrivateTraining.DomainClasses.Entities;

namespace PrivateTraining.Areas.Framework.Controllers
{
    public partial class MessageController : BaseController
    {

        private readonly IMessage _message;
        private readonly IApplicationUserManager _userManager;
        private readonly IUnitOfWork _uow;
        //   readonly IDbSet<ApplicationUser> _user;

        public MessageController(IUnitOfWork uow, IMessage message, IApplicationUserManager userManager)
        {
            _uow = uow;
            _message = message;
            //    _user = _uow.Set<ApplicationUser>();
            _userManager = userManager;
        }

        #region Message For Admin

        public virtual async Task<ActionResult> MessagForAdmin()
        {
            //   ViewBag.ListUsers = _user.Where(c => !(c is ServiceProviderInfo) && !(c is ServiceReceiverInfo)).ToList();
            ViewBag.ListUsers = await GetUserList(1);
            return View();
        }

        public async Task<IQueryable<ApplicationUser>> GetUserList(byte Param = 0)
        {
            IQueryable<ApplicationUser> List = null;
            //var roles = await _userManager.GetRolesAsync(Convert.ToInt32(User.Identity.GetUserId()));
            //if (roles != null)
            //{
            //    if (roles.FirstOrDefault() == "User" && Param==1)
            //        List = await _userManager.GetAllAdmins();
            //    else if (roles.FirstOrDefault() == "Admin")
            //        List = await _userManager.GetAllUsers();
            //}
            string role = await GetRoleName();
            if (role != "" && role != null)
            {
//                if (role == "User" && Param == 1)
//                    List = await _userManager.GetAllAdmins();
                 if (role == "Admin")
                    List = await _userManager.GetAllUsers();
                 else List = await _userManager.GetAllAdmins();
            }
            return List;
        }

        public async Task<string> GetRoleName()
        {
            string role = "";
            var roles = await _userManager.GetRolesAsync(Convert.ToInt32(User.Identity.GetUserId()));
            if (roles != null)
                role = roles.FirstOrDefault();
            return role;
        }

        public virtual async Task<ActionResult> ListMessages()
        {
            ViewBag.ListUsers = await GetUserList(2);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<JsonResult> ListJsonMessages(int UserId = 0, byte ReadMessages = 2)
        {
            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);
                var list = _message.GetAllMessage(0);

                UserId = UserId == 0 ? Convert.ToInt32(User.Identity.GetUserId()) : UserId;
                
                
                if (UserId != 0)
                    list = list.Where(x => x.ReciverUserId == UserId || x.SenderUserId == UserId);

                string role = await GetRoleName();
                if (role == "User")
                {
                    
                    list = list.Where(x => x.ReciverUserId == UserId || x.SenderUserId == UserId);
                }

                if (ReadMessages != 2)
                {
                    bool Read = Convert.ToBoolean(ReadMessages);
                    list = list.Where(x => x.ReadMessage == Read);
                }

                if (!string.IsNullOrEmpty(datatable.searchValue))
                {
                    list = list.Where(c => c.SenderUsers.Name.Contains(datatable.searchValue) ||
                                           c.SenderUsers.Family.Contains(datatable.searchValue) ||
                                           c.ReciverUsers.Name.Contains(datatable.searchValue) ||
                                           c.ReciverUsers.Family.Contains(datatable.searchValue) ||
                                           c.Subject.Contains(datatable.searchValue) ||
                                           c.Date.Contains(datatable.searchValue) ||
                                           c.Desc.Contains(datatable.searchValue)
                    );
                }

                //sort
                //if (!(string.IsNullOrEmpty(datatable.sortColumn) && string.IsNullOrEmpty(datatable.sortColumnDir)))
                //{
                //    //for make sort simpler we will add Syste.Linq.Dynamic reference
                //    list = list.OrderBy(datatable.sortColumn + " " + datatable.sortColumnDir);
                //}
                datatable.recordsTotal = list.Count();
                list = list.OrderByDescending(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize);

                datatable.data = list.ToList().Select(rec => new string[]
                {
                 "<div class=\"checkbox\" style=\"margin-right:-20px;padding-left:5px;\"><label><input type=\"checkbox\" ng-model=\"checkbox["+rec.Id+"]\" class=\"case\" ng-checked=\"all\" value=\""+rec.Id +"\" ><span class=\"text\"></span></lable></div>"
                +rec.Id,
                rec.SenderUsers.Name + " "+ rec.SenderUsers.Family ,
                rec.ReciverUsers.Name + " "+ rec.ReciverUsers.Family ,
                rec.Subject,
              //  rec.Desc,
                rec.Date,
                SHowMessages(rec.Id,rec.Desc),
                SendMessageForSender(rec.SenderUserId),
                StatusMessage(rec.ReadMessage),
                }).ToList();

                datatable.draw = datatable.draw;
                datatable.recordsFiltered = datatable.recordsTotal;
                datatable.recordsTotal = datatable.recordsTotal;

                return Json(datatable);
            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }

        public string SendMessageForSender(int SenderId = 0)
        {
            if (Convert.ToInt32(User.Identity.GetUserId()) != SenderId)
                //return "<button class=\"btn btn-info\" ng-click=\"SendMessageForSender(" + SenderId + ")\"  ><i class=\"fa  fa-reply\"></i></button>";
            return "<a href=\"javascript:void(0);\" class=\"btn btn-magenta  btn-circle btn-sm\" title=\"پاسخ\" ng-click=\"SendMessageForSender(" + SenderId + ")\" ><i class=\"fa  fa-reply\"></i></a>";
            else
                return "";
        }

        public string SHowMessages(int MessageId = 0, string Desc = "")
        {
            //return "<button class=\"btn btn-warning\" ng-click=\"SHowMessages(" + MessageId + ",'" + Desc + "')\"  ><i class=\"fa fa-comment-o\"></i></button>";
            return "<a href=\"javascript:void(0);\" class=\"btn btn-azure btn-circle btn-sm\" title=\"متن پیام\" ng-click=\"SHowMessages(" + MessageId + ",'" + Desc + "')\" ><i class=\"fa fa-comment-o\"></i></a>";

        }

        public string StatusMessage(bool StatusMessage = false)
        {
            if (StatusMessage)
                return "<a href=\"javascript:void(0);\" class=\"btn btn-success btn-circle btn-sm\" title=\"خوانده شده\"  ><i class=\"fa fa-check\"></i></a>";

            //return "<span class=\"btn btn-success\"><i class=\"fa fa-check\"></i></span>";
            else
                return "<a href=\"javascript:void(0);\" class=\"btn btn-warning btn-circle btn-sm\" title=\"خوانده نشده\"  ><i class=\"fa fa-times\"></i></a>";

            //return "<span class=\"btn btn-default\"><i class=\"fa fa-check\"></i></span>"; ;
        }

        #endregion

        #region Save Data

        [HttpPost]
        [AllowAnonymous]
        public virtual JsonResult AddMessages(Message Message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Message Table = new Message();
                    SetMessagesRecord(Table, Message);
                    var Query = _message.AddMessage(Table);
                    var Status = _uow.SaveAllChanges();

                    return Json(new { Resualt = true, Messages = "عملیات با موفقیت انجام شد." });
                }
                else
                    return Json(new { Resualt = false, Messages = "عملیات با شکست مواجه شد" });
            }
            catch (Exception Ex)
            {
                return Json(new { Resualt = false, Messages = Ex.Message });
            }
        }

        public Message SetMessagesRecord(Message Table, Message model)
        {
            PersianDate PD = new PersianDate();
            Table.SenderUserId = Convert.ToInt32(User.Identity.GetUserId());
            Table.ReciverUserId = model.ReciverUserId;
            Table.Desc = model.Desc;
            Table.Subject = model.Subject;
            Table.Date = PD.PersianDateLow() ;
            Table.Time = PD.CurrentTime();
            return Table;
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<JsonResult> DeleteMessages(string[] MessageId)
        {
            string IdS = "0";
            try
            {
                for (int i = 0; i <= MessageId.Length - 1; i++)
                {
                    IdS = MessageId[i].ToString();
                    _message.DeleteMessage(Convert.ToInt32(MessageId[i]));
                }
                return Json(new { Result = true, Message = "عملیات با موفقیت انجام شد" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "امکان غیرفعال کردن برای کد " + IdS + " وجود ندارد" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<JsonResult> ReadMessages(int MessageId)
        {
            try
            {
                var Query = _message.GetAllMessage(MessageId).FirstOrDefault();
                if (Query.ReciverUserId == Convert.ToInt32(User.Identity.GetUserId()))
                {
                    Query.ReadMessage = true;
                    var Status = _uow.SaveAllChanges();
                }

                return Json(new { Result = true, Message = "عملیات با موفقیت انجام شد" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "عملیات با شکست مواجه شد" });
            }
        }

        #endregion

    }
}