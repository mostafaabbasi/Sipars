using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.BaseTable;
using PrivateTraining.ServiceLayer.Interface.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Repository.Security
{
     public class CityRepository : ICity
    {

        private readonly IUnitOfWork _uow;
        readonly IDbSet<City> _City;
        public CityRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _City = _uow.Set<City>();

        }

        public async Task AddCity(City Citys)
        {
            _City.Add(Citys);

        }

        public async Task<IQueryable<City>> GetAllCity()
        {
            return _City;
        }

        public async Task<City> GetCity(int id)
        {
            return _City.Where(c => c.Id == id).FirstOrDefault();
        }

        public async Task<int> DeleteCity(int CityId)
        {
            City City = _City.Find(CityId);
            _City.Remove(City);
            return await _uow.SaveAllChangesAsync();

        }
    }
}
