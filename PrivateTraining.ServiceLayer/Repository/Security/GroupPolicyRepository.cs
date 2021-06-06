using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.ServiceLayer.Interface.Security;
using PrivateTraining.ServiceLayer.Extention;

namespace PrivateTraining.ServiceLayer.Repository.Security
{
    public class GroupPolicyRepository : IGroupPolicy
    {

        private readonly IUnitOfWork _uow;
        readonly IDbSet<GroupPolicy> _group;
        private readonly IDbSet<GroupPolicyUser> _UserInGroup;
        private readonly IDbSet<ApplicationUser> _Users;
        public GroupPolicyRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _group = _uow.Set<GroupPolicy>();
            _UserInGroup = _uow.Set<GroupPolicyUser>();
            _Users = _uow.Set<ApplicationUser>();

        }

        #region Group Policy

        public async Task AddGroupPolicy(GroupPolicy group)
        {
            _group.Add(group);
        }

        public async Task<IList<GroupPolicy>> GetAllIGroupPolicy()
        {
            return await _group.ToListAsync();
            //return await _group.Include("GroupPolicyUsers").Include("GroupPolicyUsers.ApplicationUsers").ToListAsync();
        }

        public int GetAllIGroupPolicySearchName(string Name = "")
        {
            var r= _group.Where(c => c.Name == Name).FirstOrDefault();
            if (r != null)
                return r.Id;
            else
                return 0;

        }

        public async Task<int> GetAllIGroupPolicySearchName2(string Name = "")
        {
            var r = await _group.Where(c => c.Name == Name).FirstOrDefaultAsync();
            if (r != null)
                return r.Id;
            else
                return 0;

        }

        public async Task<int> DeleteGroupPolicy(int GroupId)
        {
            GroupPolicy GroupPolicy = _group.Find(GroupId);
            _group.Remove(GroupPolicy);
            return await _uow.SaveAllChangesAsync();
        }

        #endregion

        #region User In Group Policy

        public async Task AddGroupPolicyUser(GroupPolicyUser group)
        {
            _UserInGroup.Add(group);
        }


        public async Task<IQueryable<GroupPolicy>> GetAllIGroupPolicyUser()
        {
            return _group.Include("GroupPolicyUsers").Include("GroupPolicyUsers.ApplicationUsers");
        }

        public async Task<IEnumerable<ApplicationUser>> GetListuserNameNotInGroup(int groupid)
        {
            return await _Users.Include("GroupPolicyUsers").Where(x => x.Roles.Any(a => a.RoleId == (int)Roles.Modrator || a.RoleId == (int)Roles.User 
            || a.RoleId == (int)Roles.ServiceProvider)).ToListAsync();
        }

        public async Task<int> DeleteUserInGroupPolicy(int grouppolicyid)
        {
            GroupPolicyUser GroupPolicy = _UserInGroup.Find(grouppolicyid);
            _UserInGroup.Remove(GroupPolicy);
            return await _uow.SaveAllChangesAsync();
        }

        public void DeleteUserInGroupPolicyWithUserId(int UserId)
        {
            GroupPolicyUser GroupPolicy = _UserInGroup.Where(c => c.UserId == UserId).FirstOrDefault();
            _UserInGroup.Remove(GroupPolicy);
            _uow.SaveAllChangesAsync();
        }

        public async Task<GroupPolicyUser> SearchUserInGroupPolicyUser(int UserId=0)
        {
            return _UserInGroup.Where(c => c.UserId == UserId).FirstOrDefault();
        }

        #endregion

    }
}
