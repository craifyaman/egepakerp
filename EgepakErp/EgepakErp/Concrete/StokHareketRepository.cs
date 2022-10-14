using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace EgePakErp.Concrete
{
    public class StokHareketRepository : _GenericRepository<StokHareket>
    {
        public override IQueryable<StokHareket> AllInclude()
        {
            return dbset
                .Include(x => x.StokHareketType)
                .Include(x => x.Siparis)
                .Include(x => x.Siparis.Cari)
                .Include(x => x.SiparisKalip)
                .Include(x => x.StokCikisHareket)
                .AsQueryable();
        }
        public override StokHareket Get(int id)
        {
              return AllInclude().FirstOrDefault(x => x.StokHareketId== id);
        }
        public override StokHareket Get(Expression<Func<StokHareket, bool>> filter)
        {
            return AllInclude().FirstOrDefault(filter);
        }
        public override IQueryable<StokHareket> GetAll()
        {
            return AllInclude();
        }

        public override IQueryable<StokHareket> GetAll(Expression<Func<StokHareket, bool>> filter)
        {
            return AllInclude().Where(filter);
        }
    }
}