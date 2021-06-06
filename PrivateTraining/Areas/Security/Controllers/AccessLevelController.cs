using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.ServiceLayer.Interface.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PrivateTraining.Areas.Security.Controllers
{
    public partial class AccessLevelController : BaseController
    {
        private readonly IAccessLevel _access;
        private readonly IUnitOfWork _uow;
        public AccessLevelController(IUnitOfWork uow, IAccessLevel access)
        {
            _uow = uow;
            _access = access;
        }

        #region ActionInGroup

        public virtual async Task<ActionResult> ActionInGroup()
        {
            return View();
        }

        /// <summary>
        /// نمایش لیست گروه ها و کاربران معین شده
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> GetActionAccessLevel(string word = "")
        {
            try
            {
                var List = await _access.GetAllIGroupPolicyAccess();
                var Collection = List.Select(a => new
                {
                    id = a.Id,
                    items = a.AccessLevelGroups.Select(x => new
                    {
                        accesslevelgroupid = x.Id,
                        actionname = x.Actions.Name,
                        actionid = x.Actions.Id,
                        parentId = x.Actions.ParentId,
                        groupid = x.GroupId,
                        accesscode = x.Actions.AccessCode,

                    }).Where(c => c.actionname.Contains(word)).OrderBy(c => c.accesscode),
                    name = a.Name,
                    isEnable = a.IsEnable
                });

                return Json(Collection);
            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = "" });
            }
        }

        /// <summary>
        /// نمایش اکشن های گروه انتخاب شده
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> GetListActionNames(int groupId, string word = "")
        {
            try
            {

                var List = await _access.GetListactionNotInGroup();

                var Collection = List.ToList().Where(c => c.ParentId == 0).Select(a => new
                {
                    id = a.Id,
                    actionname = a.Name,
                    accesscode = a.AccessCode,
                    group = a.AccessLevels.OfType<AccessLevelGroup>().Where(o => o.GroupId == groupId).Select(b => new
                    {
                        Id = b.GroupId
                    }),
                    sub = List.Where(j => j.ParentId == a.Id)
                        .Select(r => new
                        {
                            id = r.Id,
                            actionname = r.Name,
                            accesscode = r.AccessCode,
                            subgroup = r.AccessLevels.OfType<AccessLevelGroup>().Where(h => h.GroupId == groupId && h.ActionId == r.Id).Count(),
                            Selected = r.AccessLevels.OfType<AccessLevelGroup>().Any(i => i.ActionId == r.Id && i.GroupId == groupId),
                        }).Where(z =>/* z.subgroup == 0 && */ z.actionname.Contains(word)),
                    // نمایش تمام اکشن های زیر مجموعه بجای آن های که انتخاب نشده اند

                    Selected = a.AccessLevels.OfType<AccessLevelGroup>().Any(c => c.ActionId == a.Id && c.GroupId == groupId),
                });

                //if (word != "")
                //    Collection = Collection.Where(c => c.actionname.Contains(word));

                return Json(new { Resualt = true, userList = Collection, Messages = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = "" });
            }

        }

        /// <summary>
        /// نمایش اکشن های گروه انتخاب شده
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> GetsubListActionNames(int ActionId, string word = "")
        {
            try
            {

                var List = await _access.GetSubAction(ActionId);
                var Collection = List.Where(c => c.ParentId != 0).Select(a => new
                {
                    id = a.Id,
                    actionname = a.Name,
                    accesscode = a.AccessCode,
                    group = a.AccessLevels.Select(b => new
                    {
                        Id = b.Id,
                    })
                }).Where(c => !c.group.Any(g => g.Id == c.id));

                if (word != "")
                    Collection = Collection.Where(c => c.actionname.Contains(word));

                return Json(new { Resualt = true, SubList = Collection, Messages = 0 });
            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = "" });
            }

        }

        /// <summary>
        /// ثبت اکشن 
        /// </summary>
        /// <param name="ActionId"></param>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> AddActionInGroup(int ActionId, int GroupId)
        {
            try
            {
                int Status = 0;
                // چک کند اگر در جدول وجود داشت اضافه نکند
                var Access = await _access.GetRecord(ActionId, GroupId);
                var ListAction = await _access.GetSubAction(ActionId);

                if (ListAction.Count() > 0) // در صورتی ک سرگروه انتخاب شده
                {
                    // ثبت سر گروه
                    if (Access == 0)
                        Status = await InsertAccessLevelGroup(ActionId, GroupId);

                    foreach (var item in ListAction)
                    {
                        // ثبت زیر گروهها
                        var Access2 = await _access.GetRecord(item.Id, GroupId);
                        if (Access2 == 0)
                            Status = await InsertAccessLevelGroup(item.Id, GroupId);
                    }
                }
                else
                { // در صورتی که یکی از آیتم های گروه انتخاب شده چک میکند ک سرگروه باشد اگر نبود اضافه می کند

                    var ListActions = await _access.GetFathertAction(ActionId);

                    if (ListActions != null)
                    {
                        // ثبت سر گروه اکشن
                        var Access3 = await _access.GetRecord(ListActions.Id, GroupId);
                        if (Access3 == 0)
                            Status = await InsertAccessLevelGroup(ListActions.Id, GroupId);
                    }

                    // ثبت اکشن
                    if (Access == 0)
                        Status = await InsertAccessLevelGroup(ActionId, GroupId);
                }

                return Json(new { Resualt = true, grouplist = await GetActionAccessLevel(""), Messages = Status });
            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = ex.Message });
            }

        }

        public virtual async Task<int> InsertAccessLevelGroup(int ActionId, int GroupId)
        {
            AccessLevelGroup AccessLevelGroup = new AccessLevelGroup();
            AccessLevelGroup.ActionId = ActionId;
            AccessLevelGroup.GroupId = GroupId;
            await _access.AddAccessLevelGroup(AccessLevelGroup);
            var Status = await _uow.SaveAllChangesAsync();
            return Status;
        }

        /// <summary>
        ///   حذف اکشن از گروه
        /// </summary>
        /// <param name="grouppolicyid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<JsonResult> DeleteActionAccessLevel(int accesslevelgroupid)
        {
            try
            {
                var c = await _access.DeleteActionAccessLevel(accesslevelgroupid);
                return Json(new { Resualt = true, Messages = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = "" });
            }
        }

        #endregion

        #region UserAccess

        public virtual async Task<ActionResult> UserAccess()
        {
            return View();
        }

        [HttpPost]
        public virtual async Task<JsonResult> GetUserAccessLevel(string word = "")
        {
            try
            {
                var List = await _access.GetListUsers();
                var Collection = List.Select(a => new
                {
                    id = a.Id,
                    name = a.UserName + " ( " + a.Name + " " + a.Family + " ) ",
                    //items = a.AccessLevelUsers.Select(x => new
                    //{
                    //    accessleveluserid = x.Id,
                    //    actionname = x.Actions.Name,
                    //    actionid = x.Actions.Id,
                    //    parentId = x.Actions.ParentId,
                    //    accessCode = x.Actions.AccessCode,
                    //    isEnable = x.IsEnable

                    //}).OrderBy(c => c.accessCode),

                });

                Collection = Collection.Where(c => c.name.Contains(word));

                return Json(Collection);
            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = "" });
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> GetListUserActionNames(int groupId, string word = "")
        {
            try
            {



                var List2 = await _access.GetListActionFromSpListAccessUsers(groupId);
                var Collection2 = List2.ToList().Select(a => new
                {
                    id = a.Id,
                    actionname = a.Name,
                    accessleveluserid = a.Id,
                    //actionname = x.Actions.Name,
                    actionid = a.Id,
                    parentId = a.ParentId,
                    accessCode = a.AccessCode,
                    isEnable = a.IsEnable,
                }).OrderBy(c => c.accessCode).ToList();


                List<int> ActionLisId = List2.Select(a => a.Id).ToList();

                var List = await _access.GetListactionNotInGroup();

                var Collection = List.ToList().Where(c => c.ParentId == 0).Select(a => new
                {
                    id = a.Id,
                    actionname = a.Name,
                    parentid = a.ParentId,
                    group = a.AccessLevels.OfType<AccessLevelUser>().Where(c => c.UserId == groupId).Select(b => new
                    {
                        Id = b.UserId
                    }),
                    sub = List.Where(j => j.ParentId == a.Id)
                        .Select(r => new
                        {
                            id = r.Id,
                            actionname = r.Name,
                            subgroup = r.AccessLevels.OfType<AccessLevelUser>().Where(h => h.UserId == groupId && h.ActionId == r.Id).Count(),
                            Selected = r.AccessLevels.OfType<AccessLevelUser>().Any(c => c.ActionId == r.Id && c.UserId == groupId),
                        }).Where(z => z.actionname.Contains(word) /*&& !ActionLisId.Contains(z.id)*/),
                    // نمایش تمام اکشن های زیر مجموعه بجای آن های که انتخاب نشده اند

                    Selected = a.AccessLevels.OfType<AccessLevelUser>().Any(c => c.ActionId == a.Id && c.UserId == groupId),
                }).ToList();

                //if (word != "")
                //Collection = Collection.Where(c => c.actionname.Contains(word));
                //var Collection =List.Where(c => !ActionLisId.Contains(c.Id)).ToList();

                return Json(new { Resualt = true, userList = Collection, Messages = 0, NewListAction = Collection2 });
            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = "" });
            }

        }

        [HttpPost]
        public virtual async Task<JsonResult> AddUserAccess(int ActionId, int UserId)
        {
            try
            {
                int Status = 0;
                // چک کند اگر در جدول وجود داشت اضافه نکند
                var Access = await _access.GetUserRecord(ActionId, UserId);
                var ListAction = await _access.GetSubAction(ActionId);

                if (ListAction.Count() > 0) // در صورتی ک سرگروه انتخاب شده
                {
                    // ثبت سر گروه
                    if (Access == 0)
                        Status = await InsertUserAccess(ActionId, UserId);

                    foreach (var item in ListAction)
                    {
                        // ثبت زیر گروهها
                        var Access2 = await _access.GetUserRecord(item.Id, UserId);
                        if (Access2 == 0)
                            Status = await InsertUserAccess(item.Id, UserId);
                    }
                }
                else
                { // در صورتی که یکی از آیتم های گروه انتخاب شده چک میکند ک سرگروه باشد اگر نبود اضافه می کند

                    var ListActions = await _access.GetFathertAction(ActionId);

                    if (ListActions != null)
                    {
                        // ثبت سر گروه اکشن
                        var Access3 = await _access.GetUserRecord(ListActions.Id, UserId);
                        if (Access3 == 0)
                            Status = await InsertUserAccess(ListActions.Id, UserId);
                    }

                    // ثبت اکشن
                    if (Access == 0)
                        Status = await InsertUserAccess(ActionId, UserId);
                }

                return Json(new { Resualt = true, grouplist = await GetActionAccessLevel(""), Messages = Status });
            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = ex.Message });
            }

        }

        public virtual async Task<int> InsertUserAccess(int ActionId, int UserId)
        {
            AccessLevelUser AccessLevelUsers = new AccessLevelUser();
            AccessLevelUsers.ActionId = ActionId;
            AccessLevelUsers.UserId = UserId;
            await _access.AddUserAccess(AccessLevelUsers);
            var Status = await _uow.SaveAllChangesAsync();
            return Status;
        }

        [HttpPost]
        public virtual async Task<JsonResult> DeleteUserAccess(int accessleveluserid, int userid)
        {
            try
            {
                await _access.DeleteUserAccess(accessleveluserid, userid);
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