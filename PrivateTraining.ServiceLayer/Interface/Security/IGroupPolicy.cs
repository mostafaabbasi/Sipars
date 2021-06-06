using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.DomainClasses.Security;

namespace PrivateTraining.ServiceLayer.Interface.Security
{
    public interface IGroupPolicy
    {

        #region Group Policy
        Task AddGroupPolicy(GroupPolicy group);
        Task<IList<GroupPolicy>> GetAllIGroupPolicy();
         int GetAllIGroupPolicySearchName(string Name = "");
        Task<int> GetAllIGroupPolicySearchName2(string Name = "");

        #endregion

        #region User In Group

        Task AddGroupPolicyUser(GroupPolicyUser group);
        Task<IQueryable<GroupPolicy>> GetAllIGroupPolicyUser();
        Task<IEnumerable<ApplicationUser>> GetListuserNameNotInGroup(int groupid);
        Task<int> DeleteUserInGroupPolicy(int grouppolicyid);
        Task<int> DeleteGroupPolicy(int GroupId);
        void DeleteUserInGroupPolicyWithUserId(int UserId);
        Task<GroupPolicyUser> SearchUserInGroupPolicyUser(int UserId = 0);

        #endregion

    }
}
