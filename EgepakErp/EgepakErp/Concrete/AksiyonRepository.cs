using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace EgePakErp.Concrete
{
    public class AksiyonRepository : _GenericRepository<Aksiyon>
    {
        public override Aksiyon Get(int id)
        {
            return dbset
                .Include(x => x.AksiyonType)
                //.Include(x => x.UretimEmirAksiyonRel)
               .FirstOrDefault(x => x.AksiyonId == id);
        }
        public override Aksiyon Get(Expression<Func<Aksiyon, bool>> filter)
        {
            return dbset
                .Include(x => x.AksiyonType)
                //.Include(x => x.UretimEmirAksiyonRel)
               .FirstOrDefault(filter);
        }
        public override IQueryable<Aksiyon> GetAll()
        {
            return dbset
                .Include(x => x.AksiyonType)
                //.Include(x => x.UretimEmirAksiyonRel)
               .AsQueryable();
        }

        public override IQueryable<Aksiyon> GetAll(Expression<Func<Aksiyon, bool>> filter)
        {
            return dbset
                .Include(x => x.AksiyonType)
                //.Include(x => x.UretimEmirAksiyonRel)
                .Where(filter)
               .AsQueryable();
        }
    }
}