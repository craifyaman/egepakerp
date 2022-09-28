using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace EgePakErp.Concrete
{
    public class StokHareketRepository : _GenericRepository<StokHareket>
    {
        public override StokHareket Get(int id)
        {
            return dbset
                .Include(x=>x.StokHareketType)
                .Include(x=>x.Siparis)
                .Include(x => x.Siparis.Cari)
                .Include(x=>x.SiparisKalip)
                .Include(x=>x.StokCikisHareket)
               .FirstOrDefault(x => x.StokHareketId== id);
        }
        public override StokHareket Get(Expression<Func<StokHareket, bool>> filter)
        {
            return dbset
                .Include(x => x.StokHareketType)
                .Include(x => x.Siparis)
                .Include(x => x.Siparis.Cari)
                .Include(x => x.SiparisKalip)
                .Include(x => x.StokCikisHareket)
               .FirstOrDefault(filter);
        }
        public override IQueryable<StokHareket> GetAll()
        {
            return dbset
                .Include(x => x.StokHareketType)
                .Include(x => x.Siparis)
                .Include(x => x.Siparis.Cari)
                .Include(x => x.SiparisKalip)
                .Include(x => x.StokCikisHareket)
               .AsQueryable();
        }

        public override IQueryable<StokHareket> GetAll(Expression<Func<StokHareket, bool>> filter)
        {
            return dbset
                .Include(x => x.StokHareketType)
                .Include(x => x.Siparis)
                .Include(x => x.Siparis.Cari)
                .Include(x => x.SiparisKalip)
                .Include(x => x.StokCikisHareket)
                .Where(filter)
               .AsQueryable();
        }
    }
}