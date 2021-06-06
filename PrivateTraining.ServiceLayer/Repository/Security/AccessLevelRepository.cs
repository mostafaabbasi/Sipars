using PrivateTraining.ServiceLayer.Interface.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.DataLayer.Context;
using System.Data.Entity;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.ServiceLayer.Extention;
using PrivateTraining.DomainClasses.Entities.FrameWork;

namespace PrivateTraining.ServiceLayer.Repository.Security
{
    public class AccessLevelRepository : IAccessLevel
    {
        private readonly IUnitOfWork _uow;
        readonly IDbSet<AccessLevel> _access;
        readonly IDbSet<GroupPolicy> _group;
        readonly IDbSet<DomainClasses.Entities.FrameWork.Action> _action;
        readonly IDbSet<AccessLevelGroup> _accessLevelGroup;
        readonly IDbSet<AccessLevelUser> _accessleveluser;
        private readonly IDbSet<ApplicationUser> _Users;



        public AccessLevelRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _access = _uow.Set<AccessLevel>();
            _group = _uow.Set<GroupPolicy>();
            _action = _uow.Set<DomainClasses.Entities.FrameWork.Action>();
            _accessLevelGroup = _uow.Set<AccessLevelGroup>();
            _Users = _uow.Set<ApplicationUser>();
            _accessleveluser = _uow.Set<AccessLevelUser>();

        }

        public IQueryable<AccessLevel> ListAccesslevel()
        {
            return _access;
        }

        #region ActionInGroup

        public async Task<IQueryable<GroupPolicy>> GetAllIGroupPolicyAccess()
        {
            return _group.Include("AccessLevels").Include("AccessLevels.Actions");
        }

        public async Task<IQueryable<DomainClasses.Entities.FrameWork.Action>> GetListactionNotInGroup()
        {
            return _action.Include("AccessLevels");
        }

        public async Task AddAccessLevelGroup(AccessLevelGroup AccessLevelGroup)
        {
            _accessLevelGroup.Add(AccessLevelGroup);
        }

        public void  AddAccessLevelGroup2(AccessLevelGroup AccessLevelGroup)
        {
            _accessLevelGroup.Add(AccessLevelGroup);
        }

        public async Task<IEnumerable<DomainClasses.Entities.FrameWork.Action>> GetSubAction(int ActionId)
        {
            return await _action.Include("AccessLevels").Where(c => c.ParentId == ActionId).ToListAsync();
        }

        public  IEnumerable<DomainClasses.Entities.FrameWork.Action> GetSubAction2(int ActionId)
        {
            return  _action.Include("AccessLevels").Where(c => c.ParentId == ActionId).ToList();
        }

        public async Task<DomainClasses.Entities.FrameWork.Action> GetFathertAction(int ActionId)
        {
            var ParentId = _action.Include("AccessLevels").Where(c => c.Id == ActionId).FirstOrDefault().ParentId;
            return await _action.Include("AccessLevels").Where(c => c.Id == ParentId).FirstOrDefaultAsync();
        }

        public  DomainClasses.Entities.FrameWork.Action GetFathertAction2(int ActionId)
        {
            var ParentId = _action.Include("AccessLevels").Where(c => c.Id == ActionId).FirstOrDefault().ParentId;
            return  _action.Include("AccessLevels").Where(c => c.Id == ParentId).FirstOrDefault();
        }

        public async Task<int> GetRecord(int ActionId, int GroupId)
        {
            return _accessLevelGroup.Include("AccessLevels").Where(c => c.ActionId == ActionId && c.GroupId == GroupId).Count();
        }

        public int GetRecord2(int ActionId, int GroupId)
        {
            return _accessLevelGroup.Include("AccessLevels").Where(c => c.ActionId == ActionId && c.GroupId == GroupId).Count();
        }

        public async Task<int> DeleteActionAccessLevel(int accesslevelgroupid)
        {
            var status = 0;

            var ActionId = _accessLevelGroup.Where(c => c.Id == accesslevelgroupid).FirstOrDefault().ActionId;
            var ListAction = await GetSubAction(ActionId);
            if (ListAction.Count() > 0) // 
            {
                foreach (var item in ListAction)
                {
                    var AccessLevelGroupId = _accessLevelGroup.Where(c => c.ActionId == item.Id);

                    if (AccessLevelGroupId.Count() > 0)
                    {
                        AccessLevelGroup GroupPolicy2 = _accessLevelGroup.Find(AccessLevelGroupId.FirstOrDefault().Id);
                        if (GroupPolicy2 != null)
                        {
                            _accessLevelGroup.Remove(GroupPolicy2);
                            status = await _uow.SaveAllChangesAsync();
                        }

                    }
                }
            }

            AccessLevelGroup GroupPolicy = _accessLevelGroup.Find(accesslevelgroupid);
            if (GroupPolicy != null)
            {
                _accessLevelGroup.Remove(GroupPolicy);
                status = await _uow.SaveAllChangesAsync();
            }
            return status;

        }

        #endregion

        #region UserAccess

        public async Task<IEnumerable<ApplicationUser>> GetListUsers()
        {
            return await _Users.Include("AccessLevelUsers").Where(x => x.Roles.Any(a => a.RoleId == (int)Roles.Modrator || a.RoleId == (int)Roles.User)).ToListAsync();
        }

        public async Task AddUserAccess(AccessLevelUser AccessLevelUser)
        {
            _accessleveluser.Add(AccessLevelUser);
        }

        public async Task<int> GetUserRecord(int ActionId, int UserId)
        {
            var List =await _accessleveluser.FirstOrDefaultAsync(c => c.ActionId == ActionId && c.UserId == UserId);
            if (List != null)
            {
                List.IsEnable = true;
                await _uow.SaveAllChangesAsync();
                return 1;
            }
            else
                return 0;

        }

        public async Task<int> DeleteUserAccess(int accessleveluserid, int userid)
        {
            var ActionId = 0;
            var status = 0;
            var list = _accessleveluser.FirstOrDefault(c => c.ActionId == accessleveluserid && c.UserId == userid);

            if (list != null) {
                list.IsEnable = false;
                ActionId = list.ActionId;
                await _uow.SaveAllChangesAsync();
            }
            else
            {
                AccessLevelUser UserAccess = new AccessLevelUser();
                UserAccess.ActionId = accessleveluserid;
                UserAccess.UserId = userid;
                UserAccess.IsEnable = false;
                _accessleveluser.Add(UserAccess);
                await  _uow.SaveAllChangesAsync();
            }
            //if (list.Count() > 0)
            // ActionId = list.FirstOrDefault().ActionId;



            var ListAction = await GetSubAction(ActionId);
            if (ListAction.Count() > 0) // 
            {
                foreach (var item in ListAction)
                {
                    var AccessLevelGroupId = _accessleveluser.Where(c => c.ActionId == item.Id && c.UserId == userid);

                    if (AccessLevelGroupId.Count() > 0)
                    {
                        AccessLevelUser GroupPolicy2 = _accessleveluser.Find(AccessLevelGroupId.FirstOrDefault().Id);
                        if (GroupPolicy2 != null)
                        {
                            GroupPolicy2.IsEnable = false;
                            //  _accessleveluser.Remove(GroupPolicy2);
                            status = await _uow.SaveAllChangesAsync();
                        }

                    }
                }
            }

            //AccessLevelUser GroupPolicy = _accessleveluser.Find(accessleveluserid);
            //if (GroupPolicy != null)
            //{
            //    GroupPolicy.IsEnable = false;
            //    // _accessleveluser.Remove(GroupPolicy);
            //    status = await _uow.SaveAllChangesAsync();
            //}
            return status;

        }

        public async Task<IEnumerable<DomainClasses.Entities.FrameWork.Action>> GetListActionFromSpListAccessUsers(int UserId = 0)
        {
            object[] Param = new object[1];
            Param[0] = UserId;
            return _uow.GetRows<DomainClasses.Entities.FrameWork.Action>("exec ListAccessUsers " + UserId, Param);
        }

        public async Task<int> GetListActionFromSpListAccessUsers2(int UserId = 0)
        {
            ApplicationDbContext Context = new ApplicationDbContext();
            var s = Context.Database.SqlQuery<DomainClasses.Entities.FrameWork.Action>("exec ListAccessUsers " + UserId).ToList();
            return 0;
        }


        #endregion
    }
}
