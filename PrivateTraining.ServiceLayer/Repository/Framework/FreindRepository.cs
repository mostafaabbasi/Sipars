using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.ServiceLayer.Interface.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Repository.Framework
{
    public class FreindRepository : IFreind
    {
        private readonly IUnitOfWork _uow;
        readonly IDbSet<Freind> _Freind;

        public FreindRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _Freind = _uow.Set<Freind>();
        }

        public async Task AddFreind(Freind Model)
        {
            _Freind.Add(Model);
        }

        public IQueryable<Freind> GetAllFreind(int FreindId = 0)
        {
            if (FreindId != 0)
                return _Freind.Where(c => c.Id == FreindId);
            else
                return _Freind;
        }

        public void DeleteFreind(int MesseageId)
        {
            var Query = _Freind.Find(MesseageId);
            Query.IsEnable = false;
            var Status = _uow.SaveAllChanges();
        }

        public void InactiveFreind(int Id)
        {
            var Query = _Freind.Where(c => c.Id == Id).FirstOrDefault();
            if (Query != null)
            {
                if (Query.IsEnable)
                    Query.IsEnable = false;
                else
                    Query.IsEnable = true;

                _uow.SaveAllChanges();

            }
        }
    }
}
