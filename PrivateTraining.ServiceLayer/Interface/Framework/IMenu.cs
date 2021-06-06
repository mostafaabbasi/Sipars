using PrivateTraining.DomainClasses.Entities.FrameWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PrivateTraining.ServiceLayer.Interface.Framework
{
    public interface IMenu
    {

        #region Menus
        //Task AddGroupPolicy(GroupPolicy group);
        IQueryable<Menu> ListMenu(string Role);
        #endregion
        
    }
}
