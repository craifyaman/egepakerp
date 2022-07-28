using EgePakErp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace EgepakErp.Concrete
{
    public class UrunRepository : _GenericRepository<Urun>
    {
        public override Urun Get(int id)
        {
            return dbset.Where(x => x.UrunId == id && x.isAktif == true)
               .Include("UrunCinsi")
               .Include("KalipUrunRelation")
               .Include("KalipUrunRelation.Kalip")
               .FirstOrDefault();
        }
        public override Urun Get(Expression<Func<Urun, bool>> filter)
        {
            return dbset.Where(filter)
                .Where(x => x.isAktif == true)
               .Include("UrunCinsi")
               .Include("KalipUrunRelation")
               .Include("KalipUrunRelation.Kalip")
               .FirstOrDefault();
        }
        public override IQueryable<Urun> GetAll()
        {
            return dbset
                .Where(x => x.isAktif == true)
               .Include("UrunCinsi")
               .Include("KalipUrunRelation")
               .Include("KalipUrunRelation.Kalip")
               .AsQueryable();
        }

        public override IQueryable<Urun> GetAll(Expression<Func<Urun, bool>> filter)
        {
            return dbset.Where(filter)
                .Where(x => x.isAktif == true)
               .Include("UrunCinsi")
               .Include("KalipUrunRelation")
               .Include("KalipUrunRelation.Kalip")
               .AsQueryable();
        }
    }
}