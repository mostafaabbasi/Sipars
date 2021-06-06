using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.BaseTable;
using PrivateTraining.ServiceLayer.Interface.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.ServiceLayer.Interface.Security;

namespace PrivateTraining.ServiceLayer.Repository.Security
{
    public class StateRepository : IState
    {

        private readonly IUnitOfWork _uow;
        readonly IDbSet<State> _State;
        public StateRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _State = _uow.Set<State>();

        }

        public async Task AddState(State States)
        {
            _State.Add(States);

        }

        public async Task<IQueryable<State>> GetAllState()
        {
            return _State;
        }

        public async Task<State> GetState(int id)
        {
            return _State.Where(c => c.Id == id).FirstOrDefault();
        }

        public async Task<int> DeleteState(int StateId)
        {
            State State = _State.Find(StateId);
            _State.Remove(State);
            return await _uow.SaveAllChangesAsync();

        }
    }
}
