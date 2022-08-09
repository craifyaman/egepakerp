using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EgepakErp.Concrete
{
    public class FiyatRepository : _GenericRepository<Fiyat>
    {
        public override Fiyat Get(int id)
        {
            return dbset
               .FirstOrDefault(x => x.FiyatId == id);
        }
        public override Fiyat Get(Expression<Func<Fiyat, bool>> filter)
        {
            return dbset
               .FirstOrDefault(filter);
        }
        public override IQueryable<Fiyat> GetAll()
        {
            return dbset
               .AsQueryable();
        }

        public override IQueryable<Fiyat> GetAll(Expression<Func<Fiyat, bool>> filter)
        {
            return dbset
                .Where(filter)
               .AsQueryable();
        }
    }
}