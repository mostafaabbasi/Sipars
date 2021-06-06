using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PrivateTraining.ServiceLayer.Interface.Framework
{
    public interface IAction
    {
        Task<IQueryable<DomainClasses.Entities.FrameWork.Action>> ListActions();
        Task<int> GetIdWithActionName(string name="");
        int GetIdWithActionName2(string name = "");
    }
}
