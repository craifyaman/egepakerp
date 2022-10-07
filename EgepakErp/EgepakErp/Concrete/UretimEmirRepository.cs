using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Collections.Generic;

namespace EgePakErp.Concrete
{
    public class UretimEmirRepository : _GenericRepository<UretimEmir>
    {
        private IQueryable<UretimEmir> AllIncludes()
        {
            return dbset
                .Include(x => x.Makine)
                .Include(x => x.SiparisKalip)
                .Include(x => x.SiparisKalip.Siparis)
                .Include(x => x.Siparis.Cari)
                .Include(x => x.UretimAksiyon)
                .Include(x => x.UretimEmirDurum)
                .AsQueryable();
        }
        public override UretimEmir Get(int id)
        {
            return AllIncludes().FirstOrDefault(x => x.UretimEmirId== id);
        }
        public override UretimEmir Get(Expression<Func<UretimEmir, bool>> filter)
        {
            return AllIncludes().FirstOrDefault(filter);
        }
        public override IQueryable<UretimEmir> GetAll()
        {
            return AllIncludes();
        }

        public override IQueryable<UretimEmir> GetAll(Expression<Func<UretimEmir, bool>> filter)
        {
            return AllIncludes().Where(filter);
        }
    }
}