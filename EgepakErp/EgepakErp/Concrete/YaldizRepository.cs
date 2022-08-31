using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace EgePakErp.Concrete
{
    public class YaldizRepository : _GenericRepository<Yaldiz>
    {
        public override Yaldiz Get(int id)
        {
            return dbset
                .Include(x=>x.Cari)
               .FirstOrDefault(x => x.YaldizId== id);
        }
        public override Yaldiz Get(Expression<Func<Yaldiz, bool>> filter)
        {
            return dbset
                .Include(x=>x.Cari)
               .FirstOrDefault(filter);
        }
        public override IQueryable<Yaldiz> GetAll()
        {
            return dbset
                .Include(x => x.Cari)
               .AsQueryable();
        }

        public override IQueryable<Yaldiz> GetAll(Expression<Func<Yaldiz, bool>> filter)
        {
            return dbset
                .Include(x => x.Cari)
                .Where(filter)
               .AsQueryable();
        }
    }
}