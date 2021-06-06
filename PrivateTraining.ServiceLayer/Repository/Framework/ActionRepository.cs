using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.ServiceLayer.Interface.Framework;
using PrivateTraining.DataLayer.Context;
using System.Data.Entity;

namespace PrivateTraining.ServiceLayer.Repository.Framework
{
    public class ActionRepository : IAction
    {
        private readonly IUnitOfWork _uow;
        readonly IDbSet<DomainClasses.Entities.FrameWork.Action> _Action;
        public ActionRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _Action = _uow.Set<DomainClasses.Entities.FrameWork.Action>();
        }

        public async Task<IQueryable<DomainClasses.Entities.FrameWork.Action>> ListActions()
        {
            return _Action;
        }

        public async Task<int> GetIdWithActionName(string name="")
        {
            var Query = await _Action.Where(c => c.Name == name).FirstOrDefaultAsync();
            if (Query != null)
                return Query.Id;
            else return 0;
        }

        public int GetIdWithActionName2(string name = "")
        {
            var Query =  _Action.Where(c => c.Name == name).FirstOrDefault();
            if (Query != null)
                return Query.Id;
            else return 0;
        }



    }
}
