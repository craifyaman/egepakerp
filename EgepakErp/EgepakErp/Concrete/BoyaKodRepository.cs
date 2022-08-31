using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EgePakErp.Concrete
{
    public class BoyaKodRepository : _GenericRepository<BoyaKod>
    {
        public override BoyaKod Get(int id)
        {
            return dbset
               .FirstOrDefault(x => x.BoyaKodId== id);
        }
        public override BoyaKod Get(Expression<Func<BoyaKod, bool>> filter)
        {
            return dbset
               .FirstOrDefault(filter);
        }
        public override IQueryable<BoyaKod> GetAll()
        {
            return dbset
               .AsQueryable();
        }

        public override IQueryable<BoyaKod> GetAll(Expression<Func<BoyaKod, bool>> filter)
        {
            return dbset
                .Where(filter)
               .AsQueryable();
        }
    }
}