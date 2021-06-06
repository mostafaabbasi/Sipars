using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Interface.PrivateTraining
{
    public interface IWorkUnit
    {
        Task AddWorkUnit(WorkUnit WorkUnits);
        Task<IQueryable<WorkUnit>> GetAllWorkUnit();
        Task<WorkUnit> GetWorkUnit(int id);
        Task<int> DeleteWorkUnit(int WorkUnitId);
    }
}
