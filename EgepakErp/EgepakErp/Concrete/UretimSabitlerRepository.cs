using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EgePakErp.Concrete
{
    public class UretimSabitlerRepository : _GenericRepository<UretimSabitler>
    {
        public override UretimSabitler Get(int id)
        {
            return dbset
               .FirstOrDefault(x => x.UretimSabitlerId == id);
        }
        public override UretimSabitler Get(Expression<Func<UretimSabitler, bool>> filter)
        {
            return dbset
               .FirstOrDefault(filter);
        }
        public override IQueryable<UretimSabitler> GetAll()
        {
            return dbset
               .AsQueryable();
        }

        public override IQueryable<UretimSabitler> GetAll(Expression<Func<UretimSabitler, bool>> filter)
        {
            return dbset
                .Where(filter)
               .AsQueryable();
        }
    }
}