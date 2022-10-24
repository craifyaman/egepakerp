using EgePakErp.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace EgePakErp.Concrete
{
    public class UretimEmirAksiyonRepository : _GenericRepository<UretimEmirAksiyon>
    {
        public override IQueryable<UretimEmirAksiyon> AllInclude()
        {

            return dbset
               .Include(x => x.UretimEmir)
               .Include(x => x.UretimEmir.SiparisKalip)
               .Include(x => x.UretimEmirAksiyonType)
               .Include(x => x.Kisi)
               .Include(x => x.UretimEmir.Siparis.Urun)
               .Include(x => x.UretimEmir.Siparis.Urun.UrunCinsi)
               .Include(x => x.Makine)
               .AsQueryable();
        }
        public override UretimEmirAksiyon Get(int id)
        {
            return AllInclude().FirstOrDefault(x => x.UretimEmirAksiyonId == id);
        }
        public override UretimEmirAksiyon Get(Expression<Func<UretimEmirAksiyon, bool>> filter)
        {
            return AllInclude().FirstOrDefault(filter);
        }
        public override IQueryable<UretimEmirAksiyon> GetAll()
        {
            return AllInclude();
        }

        public override IQueryable<UretimEmirAksiyon> GetAll(Expression<Func<UretimEmirAksiyon, bool>> filter)
        {
            return AllInclude().Where(filter);
        }
    }
}