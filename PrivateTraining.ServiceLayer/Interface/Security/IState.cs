using PrivateTraining.DomainClasses.Entities.BaseTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Interface.Security
{
    public interface  IState
    {
        Task AddState(State States);
        Task<IQueryable<State>> GetAllState();
        Task<State> GetState(int id);
        Task<int> DeleteState(int StateId);
    }
}
