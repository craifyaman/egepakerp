﻿using EgePakErp.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace EgePakErp.Concrete
{
    public class SiparisRepository : _GenericRepository<Siparis>
    {
        public override Siparis Get(int id)
        {
            return dbset
               .Include(x=>x.Cari)
               .Include(x=>x.Urun)
               .Include(x=>x.SiparisKalip)
               .Include(x => x.Urun.UrunCinsi)
               .FirstOrDefault(x=>x.SiparisId == id);
        }
        public override Siparis Get(Expression<Func<Siparis, bool>> filter)
        {
            return dbset
               .Include(x => x.Cari)
               .Include(x => x.Urun)
               .Include(x => x.SiparisKalip)
               .Include(x => x.Urun.UrunCinsi)
               .FirstOrDefault(filter);
        }
        public override IQueryable<Siparis> GetAll()
        {
            return dbset
               .Include(x => x.Cari)
               .Include(x => x.Urun)
               .Include(x => x.SiparisKalip)
               .Include(x => x.Urun.UrunCinsi)
               .AsQueryable();
        }

        public override IQueryable<Siparis> GetAll(Expression<Func<Siparis, bool>> filter)
        {
            return dbset
               .Include(x => x.Cari)
               .Include(x => x.Urun)
               .Include(x => x.SiparisKalip)
               .Include(x => x.Urun.UrunCinsi)
               .Where(filter)
               .AsQueryable();
        }
    }
}