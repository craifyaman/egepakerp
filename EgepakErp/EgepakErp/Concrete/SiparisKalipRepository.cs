﻿using EgePakErp.Models;
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
               .Include(x => x.TozBoyaKod)
               //.Include(x => x.Yaldiz)
               .Include(x => x.UretimEmir)
               //.Include(x => x.StokHareket)
               .Include(x => x.MetalizeKod)
               .Include(x => x.SpreyBoyaKod)
               .FirstOrDefault(x=>x.SiparisKalipId == id);
        }
        public override SiparisKalip Get(Expression<Func<SiparisKalip, bool>> filter)
        {
            return dbset
               .Include(x => x.Siparis)
               .Include(x => x.TozBoyaKod)
              // .Include(x => x.Yaldiz)
               .Include(x => x.UretimEmir)
               //.Include(x => x.StokHareket)
               .Include(x => x.MetalizeKod)
               .Include(x => x.SpreyBoyaKod)
               .FirstOrDefault(filter);
        }
        public override IQueryable<SiparisKalip> GetAll()
        {
            return dbset
               .Include(x => x.Siparis)
               .Include(x => x.TozBoyaKod)
              // .Include(x => x.Yaldiz)
               .Include(x => x.UretimEmir)
               //.Include(x => x.StokHareket)
               .Include(x => x.MetalizeKod)
               .Include(x => x.SpreyBoyaKod)
               .AsQueryable();
        }

        public override IQueryable<SiparisKalip> GetAll(Expression<Func<SiparisKalip, bool>> filter)
        {
            return dbset
               .Include(x => x.Siparis)
               .Include(x => x.TozBoyaKod)
              // .Include(x => x.Yaldiz)
               .Include(x => x.UretimEmir)
               //.Include(x => x.StokHareket)
               .Include(x => x.MetalizeKod)
               .Include(x => x.SpreyBoyaKod)
               .Where(filter)
               .AsQueryable();
        }
    }
}