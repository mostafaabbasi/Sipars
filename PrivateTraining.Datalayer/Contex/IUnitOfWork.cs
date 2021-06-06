using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DataLayer.Context
{
    public interface IUnitOfWork : IDisposable
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveAllChanges();
        Task<int> SaveAllChangesAsync();
        void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class;
        IList<T> GetRows<T>(string sql, params object[] parameters) where T : class;
        IList<T> GetRowsWithoutParam<T>(string sql) where T : class;
        IEnumerable<TEntity> AddThisRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void ForceDatabaseInitialize();
    }
}
