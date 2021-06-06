using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Repository.PrivateTraining
{
    public class WorkUnitRepository : IWorkUnit
    {

        private readonly IUnitOfWork _uow;
        readonly IDbSet<WorkUnit> _WorkUnit;
        public WorkUnitRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _WorkUnit = _uow.Set<WorkUnit>();

        }

        public async Task AddWorkUnit(WorkUnit WorkUnits)
        {
            _WorkUnit.Add(WorkUnits);

        }

        public async Task<IQueryable<WorkUnit>> GetAllWorkUnit()
        {
            return _WorkUnit.Include("ServiceWorkUnits");
        }

        public async Task<WorkUnit> GetWorkUnit(int id)
        {
            return _WorkUnit.Where(c => c.Id == id).FirstOrDefault();
        }

        public async Task<int> DeleteWorkUnit(int WorkUnitId)
        {
            WorkUnit WorkUnit = _WorkUnit.Find(WorkUnitId);
            _WorkUnit.Remove(WorkUnit);
            return await _uow.SaveAllChangesAsync();

        }
    }
}
