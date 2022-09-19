using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace EgePakErp.Concrete
{
    public class BoyaKaplamaRepository : _GenericRepository<BoyaKaplama>
    {
        public override BoyaKaplama Get(int id)
        {
            return dbset
                .Include(x=>x.BoyaKaplamaType)
               .FirstOrDefault(x => x.BoyaKaplamaId== id);
        }
        public override BoyaKaplama Get(Expression<Func<BoyaKaplama, bool>> filter)
        {
            return dbset
                .Include(x=>x.BoyaKaplamaType)
               .FirstOrDefault(filter);
        }
        public override IQueryable<BoyaKaplama> GetAll()
        {
            return dbset
                .Include(x=>x.BoyaKaplamaType)
               .AsQueryable();
        }

        public override IQueryable<BoyaKaplama> GetAll(Expression<Func<BoyaKaplama, bool>> filter)
        {
            return dbset
                .Include(x=>x.BoyaKaplamaType)
                .Where(filter)
               .AsQueryable();
        }
    }
}