using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.ServiceLayer.Interface.Security;
using PrivateTraining.BussinessLayer.Generic;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Microsoft.AspNet.SignalR;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.ServiceLayer.BLL;

namespace PrivateTraining.Areas.Security.Controllers
{
    public partial class GroupPolicyController : BaseController
    {
        private readonly IGroupPolicy _group;
        private readonly IUnitOfWork _uow;
        public GroupPolicyController(IUnitOfWork uow, IGroupPolicy group)
        {
            _uow = uow;
            _group = group;
        }


        #region Add Group Policy


        // GET: Security/GroupPolicy
        public virtual async Task<ActionResult> Index()
        {
            return View();
        }

        /// <summary>
        /// ثبت گروه
        /// </summary>
        /// <param name="GroupPolicy"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> SaveGroup(GroupPolicy GroupPolicy)
        {
            try
            {
                await _group.AddGroupPolicy(GroupPolicy);
                int Status = await _uow.SaveAllChangesAsync();
                //grouplist = await _group.GetAllIGroupPolicy(),
                return Json(new { Resualt = true, Messages = Status });
            }
            catch (Exception)
            {
                return Json(new { Resualt = false, Messages = "" });
            }

        }

        [HttpPost]
        public virtual async Task<JsonResult> GetGroupPolicy()
        {
            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);

                var List = await _group.GetAllIGroupPolicy();
                datatable.data = List.Select(a => new string[]
                {
                 a.Id.ToString(),
                 a.Name,
                }).ToList();

                datatable.recordsTotal = datatable.data.Count();
                datatable.data = datatable.data.Skip(datatable.skip).Take(datatable.pageSize).ToList();

                return Json(datatable);
            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = "" });
            }

        }

        /// <summary>
        /// حذف  گروه
        /// </summary>
        /// <param name="grouppolicyid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> DeleteGroupPolicy(int GroupId)
        {
            try
            {
                await _group.DeleteGroupPolicy(GroupId);
                return Json(new { Resualt = true, Messages = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = "" });
            }
        }

        #endregion

        #region Add User In Group

        public virtual async Task<ActionResult> UserInGroup()
        {
            return View();
        }

        /// <summary>
        /// نمایش کاربران گروه انتخاب شده
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> GetListUserNames(int groupId, string word = "", int RoleId = 0)
        {
            try
            {

                var List1 = await _group.GetListuserNameNotInGroup(groupId);
                var List = List1.Where(c => c.Deleted == 0).ToList();

                var Collection = List.Select(a => new
                {
                    id = a.Id,
                    username = a.UserName + "   ( " + a.Name + " " + a.Family + " ) ",
                    group = a.GroupPolicyUsers.Select(b => new
                    {
                        Userid = b.UserId,
                        GroupPolicyid = b.GroupPolicyId
                    }),
                    Role = a.Roles.FirstOrDefault().RoleId,
                }).Where(c => !c.group.Any(g => g.Userid == c.id && g.GroupPolicyid == groupId));
                //List = List.Where(c => c.GroupPolicyUsers.Any(b => b.UserId == c.Id));

                if (word != "")
                    Collection = Collection.Where(c => c.username.Contains(word));

                if (RoleId != 0)
                {
                    Collection = Collection.Where(c => c.Role == RoleId);
                }

                return Json(new { Resualt = true, userList = Collection, Messages = 0 });
            }
            catch (Exception)
            {
                return Json(new { Resualt = false, Messages = "" });
            }

        }

        /// <summary>
        /// ثبت کاربر در گروه انتخاب شده
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> AddUserInGroupPolicy(int UserId, int GroupId)
        {
            try
            {
                GroupPolicyUser GroupPolicyUser = new GroupPolicyUser();
                GroupPolicyUser.UserId = UserId;
                GroupPolicyUser.GroupPolicyId = GroupId;

                await _group.AddGroupPolicyUser(GroupPolicyUser);
                int Status = await _uow.SaveAllChangesAsync();

                return Json(new { Resualt = true, grouplist = await GetUserInGroupPolicy(), Messages = Status });
            }
            catch (Exception)
            {
                return Json(new { Resualt = false, Messages = "" });
            }

        }

        /// <summary>
        /// نمایش لیست گروه ها و کاربران معین شده
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> GetUserInGroupPolicy(string word = "")
        {
            try
            {
                var List = await _group.GetAllIGroupPolicyUser();
                var Collection = List.Select(a => new
                {
                    id = a.Id,
                    items = a.GroupPolicyUsers.Select(x => new
                    {
                        grouppolicyid = x.Id,
                        username = x.ApplicationUsers.UserName + "   ( " + x.ApplicationUsers.Name + " " + x.ApplicationUsers.Family + " ) ",
                        userid = x.ApplicationUsers.Id
                    }).Where(c => c.username.Contains(word)),
                    name = a.Name,
                    isEnable = a.IsEnable
                });
                // || c.items.Select(v=>v.username).Contains(word)
                //if (word != "")
                //    Collection = Collection.Where(c => c.name.Contains(word) );

                return Json(Collection);
            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = "" });
            }
        }

        /// <summary>
        ///   حذف کاربر از گروه
        /// </summary>
        /// <param name="grouppolicyid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> DeleteUserInGroupPolicy(int grouppolicyid)
        {
            try
            {
                await _group.DeleteUserInGroupPolicy(grouppolicyid);
                return Json(new { Resualt = true, Messages = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = "" });
            }
        }

        #endregion


    }
}