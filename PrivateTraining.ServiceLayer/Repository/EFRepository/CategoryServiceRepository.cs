using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.ServiceLayer.Interface;

namespace PrivateTraining.ServiceLayer.Repository.EFRepository
{
    //public class CategoryServiceRepository : ICategoryService
    //{
    //    IUnitOfWork _uow;
    //    readonly IDbSet<Category> _categories;
    //    public CategoryServiceRepository(IUnitOfWork uow)
    //    {
    //        _uow = uow;
    //        _categories = _uow.Set<Category>();
    //    }

    //    public void  AddNewCategory(Category category)
    //    {
    //        _categories.Add(category);
    //    }

    //    public async  Task<IList<Category>> GetAllCategoriesAsync()
    //    {
    //        return (await _categories.ToListAsync());
    //    }

    //    public IList<Category> GetAllCategories()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
