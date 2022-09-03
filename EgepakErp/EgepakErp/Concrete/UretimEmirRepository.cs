using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace EgePakErp.Concrete
{
    public class UretimEmirRepository : _GenericRepository<UretimEmir>
    {
        public override UretimEmir Get(int id)
        {
            return dbset
                .Include(x=>x.Makine)
                .Include(x => x.SiparisKalip)
               .FirstOrDefault(x => x.UretimEmirId== id);
        }
        public override UretimEmir Get(Expression<Func<UretimEmir, bool>> filter)
        {
            return dbset
                .Include(x=>x.Makine)
                .Include(x => x.SiparisKalip)
               .FirstOrDefault(filter);
        }
        public override IQueryable<UretimEmir> GetAll()
        {
            return dbset
                .Include(x => x.Makine)
                .Include(x => x.SiparisKalip)
               .AsQueryable();
        }

        public override IQueryable<UretimEmir> GetAll(Expression<Func<UretimEmir, bool>> filter)
        {
            return dbset
                .Include(x => x.Makine)
                .Include(x => x.SiparisKalip)
                .Where(filter)
               .AsQueryable();
        }
    }
}