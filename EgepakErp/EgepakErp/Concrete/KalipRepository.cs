using EgePakErp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace EgepakErp.Concrete
{
    public class KalipRepository : _GenericRepository<Kalip>
    {
        public override Kalip Get(int id)
        {
            return dbset
                .Include("UretimTeminSekli")
                .Include("KalipUrunRelation")
                .Include("KalipUrunRelation.Urun")
                .Include("KalipUrunRelation.Urun.UrunCinsi")
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
               .FirstOrDefault(x => x.KalipId == id && x.isAktive == true);
        }
        public override Kalip Get(Expression<Func<Kalip, bool>> filter)
        {
            return dbset
                .Include("UretimTeminSekli")
                .Include("KalipUrunRelation")
                .Include("KalipUrunRelation.Urun")
                .Include("KalipUrunRelation.Urun.UrunCinsi")
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
               .FirstOrDefault(filter);
        }
        public override IQueryable<Kalip> GetAll()
        {
            return dbset
                .Include("UretimTeminSekli")
                .Include("KalipUrunRelation")
                .Include("KalipUrunRelation.Urun")
                .Include("KalipUrunRelation.Urun.UrunCinsi")
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Where(x => x.isAktive == true)
               .AsQueryable();
        }

        public override IQueryable<Kalip> GetAll(Expression<Func<Kalip, bool>> filter)
        {
            return dbset
                .Include("UretimTeminSekli")
                .Include("KalipUrunRelation")
                .Include("KalipUrunRelation.Urun")
                .Include("KalipUrunRelation.Urun.UrunCinsi")
                .Include("KalipHammaddeRelation")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Where(filter)
               .AsQueryable();
        }
    }
}