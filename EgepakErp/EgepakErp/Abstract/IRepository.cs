using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EgepakErp.Abstract
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        T Get(Expression<Func<T, bool>> filter);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter);

        void Insert(T p);
        void Update(T p);
        void Delete(T p);
        void AddRange(List<T> items);
    }
}
