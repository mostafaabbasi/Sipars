using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Interface.Security
{
    public interface IAccessLevel
    {
       IQueryable<AccessLevel> ListAccesslevel();
       Task<IQueryable<GroupPolicy>> GetAllIGroupPolicyAccess();
        Task<IQueryable<DomainClasses.Entities.FrameWork.Action>> GetListactionNotInGroup();
        Task AddAccessLevelGroup(AccessLevelGroup AccessLevelGroup);
       void  AddAccessLevelGroup2(AccessLevelGroup AccessLevelGroup);

        Task<IEnumerable<DomainClasses.Entities.FrameWork.Action>> GetSubAction(int ActionId);
        IEnumerable<DomainClasses.Entities.FrameWork.Action> GetSubAction2(int ActionId);

        Task<DomainClasses.Entities.FrameWork.Action> GetFathertAction(int ActionId);
        DomainClasses.Entities.FrameWork.Action GetFathertAction2(int ActionId);
        Task<int> GetRecord(int ActionId, int GroupId);
        int GetRecord2(int ActionId, int GroupId);

        Task<int> DeleteActionAccessLevel(int accesslevelgroupid);

        Task<IEnumerable<ApplicationUser>> GetListUsers();
        Task<IEnumerable<DomainClasses.Entities.FrameWork.Action>> GetListActionFromSpListAccessUsers(int UserId);
        Task AddUserAccess(AccessLevelUser AccessLevelUser);
        Task<int> GetUserRecord(int ActionId, int UserId);
        Task<int> DeleteUserAccess(int accessleveluserid,int userid);

        Task<int> GetListActionFromSpListAccessUsers2(int UserId);


    }
}
