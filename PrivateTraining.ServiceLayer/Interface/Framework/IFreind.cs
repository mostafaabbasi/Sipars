using PrivateTraining.DomainClasses.Entities.FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Interface.Framework
{
    public interface IFreind
    {
        IQueryable<Freind> GetAllFreind(int FreindId);
        Task AddFreind(Freind Model);
        void DeleteFreind(int MesseageId);
        void InactiveFreind(int Id);

    }
}
