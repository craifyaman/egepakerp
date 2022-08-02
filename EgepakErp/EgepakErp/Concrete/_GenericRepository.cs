using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using EgepakErp.Abstract;
using EgePakErp;

namespace EgepakErp.Concrete
{
    public class _GenericRepository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> dbset;
        protected Db db;
        public _GenericRepository()
        {
            db = new Db();
            dbset = db.Set<T>();
        }
        public void AddRange(List<T> items)
        {
            dbset.AddRange(items);
            db.SaveChanges(1);
        }

        public void Delete(T p)
        {
            dbset.Remove(p);
            db.SaveChanges();
        }

        public virtual T Get(int id)
        {
            return dbset.Find(id);
        }

        public virtual T Get(Expression<Func<T, bool>> filter)
        {
            return dbset.Where(filter).FirstOrDefault();
        }

        public virtual IQueryable<T> GetAll()
        {
            return dbset.AsQueryable();
        }

        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> filter)
        {
            return dbset.Where(filter).AsQueryable();
        }

        public void Insert(T p)
        {
            dbset.Add(p);
            db.SaveChanges();
        }

        public void Update(T p)
        {
            db.SaveChanges();
        }
    }
}