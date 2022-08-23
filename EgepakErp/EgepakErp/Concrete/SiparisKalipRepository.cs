using EgePakErp.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace EgePakErp.Concrete
{
    public class SiparisKalipRepository : _GenericRepository<SiparisKalip>
    {
        public override SiparisKalip Get(int id)
        {
            return dbset
               .Include(x=>x.Siparis)
               .FirstOrDefault(x=>x.SiparisKalipId == id);
        }
        public override SiparisKalip Get(Expression<Func<SiparisKalip, bool>> filter)
        {
            return dbset
               .Include(x => x.Siparis)
               .FirstOrDefault(filter);
        }
        public override IQueryable<SiparisKalip> GetAll()
        {
            return dbset
               .Include(x => x.Siparis)
               .AsQueryable();
        }

        public override IQueryable<SiparisKalip> GetAll(Expression<Func<SiparisKalip, bool>> filter)
        {
            return dbset
               .Include(x => x.Siparis)
               .Where(filter)
               .AsQueryable();
        }
    }
}