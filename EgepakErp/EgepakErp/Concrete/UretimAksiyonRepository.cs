using EgePakErp.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace EgePakErp.Concrete
{
    public class UretimAksiyonRepository : _GenericRepository<UretimAksiyon>
    {
        private IQueryable<UretimAksiyon> AllInclude()
        {

            return dbset
               .Include(x => x.UretimEmir)
               .Include(x => x.Makine)
               .AsQueryable();
        }
        public override UretimAksiyon Get(int id)
        {
            return AllInclude().FirstOrDefault(x => x.UretimAksiyonId== id);
        }
        public override UretimAksiyon Get(Expression<Func<UretimAksiyon, bool>> filter)
        {
            return AllInclude().FirstOrDefault(filter);
        }
        public override IQueryable<UretimAksiyon> GetAll()
        {
            return AllInclude();
        }

        public override IQueryable<UretimAksiyon> GetAll(Expression<Func<UretimAksiyon, bool>> filter)
        {
            return AllInclude().Where(filter);
        }
    }
}