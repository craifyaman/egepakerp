using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace EgePakErp.Concrete
{
    public class MakineRepository : _GenericRepository<Makine>
    {
        public override Makine Get(int id)
        {
            return dbset
                .Include(x=>x.UretimEmir)
               .FirstOrDefault(x => x.MakineId== id);
        }
        public override Makine Get(Expression<Func<Makine, bool>> filter)
        {
            return dbset
                .Include(x=>x.UretimEmir)
               .FirstOrDefault(filter);
        }
        public override IQueryable<Makine> GetAll()
        {
            return dbset
                .Include(x => x.UretimEmir)
               .AsQueryable();
        }

        public override IQueryable<Makine> GetAll(Expression<Func<Makine, bool>> filter)
        {
            return dbset
                .Include(x => x.UretimEmir)
                .Where(filter)
               .AsQueryable();
        }
    }
}