using PrivateTraining.DomainClasses.Entities.BaseTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.Interface.Security
{
    public interface ICity
    {
        Task AddCity(City Citys);
        Task<IQueryable<City>> GetAllCity();
        Task<City> GetCity(int id);
        Task<int> DeleteCity(int CityId);
    }
}
