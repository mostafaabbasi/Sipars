using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.ServiceLayer.Interface.Framework;


namespace PrivateTraining.ServiceLayer.Repository.Framework
{
    public class MenuRepository : IMenu
    {

        private readonly IUnitOfWork _uow;
        readonly IDbSet<Menu> _meun;
        public MenuRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _meun = _uow.Set<Menu>();
        }

        #region Group Policy

       
        #endregion
        
        public IQueryable<Menu> ListMenu(string Role)
        {
            return _meun.Include("Action").Where(c=>c.RoleAccess.Contains(Role));
        }
    }
}
