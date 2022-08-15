using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

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
                .Include("KalipHammaddeRelation.Kalip")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeHareket")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeFire")
                .Include("KalipHammaddeRelation.HammaddeCinsi.TableHammaddeBirim")
                .Where(x => x.isAktive == true)
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
                .Include("KalipHammaddeRelation.Kalip")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeHareket")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeFire")
                .Include("KalipHammaddeRelation.HammaddeCinsi.TableHammaddeBirim")
                .Where(x => x.isAktive == true)
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
                .Include("KalipHammaddeRelation.Kalip")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeHareket")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeFire")
                .Include("KalipHammaddeRelation.HammaddeCinsi.TableHammaddeBirim")
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
                .Include("KalipHammaddeRelation.Kalip")
                .Include("KalipHammaddeRelation.HammaddeCinsi")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeHareket")
                .Include("KalipHammaddeRelation.HammaddeCinsi.HammaddeFire")
                .Include("KalipHammaddeRelation.HammaddeCinsi.TableHammaddeBirim")
                .Where(x=>x.isAktive == true)
                .Where(filter)
               .AsQueryable();
        }
    }
}