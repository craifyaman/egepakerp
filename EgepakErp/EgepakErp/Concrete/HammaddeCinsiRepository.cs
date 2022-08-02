using EgePakErp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace EgepakErp.Concrete
{
    public class HammaddeCinsiRepository : _GenericRepository<HammaddeCinsi>
    {
        public override HammaddeCinsi Get(int id)
        {
            return dbset
                .Include("Kalip")
                .Include("TableHammaddeBirim")
                .Include("HammaddeFire")
               .FirstOrDefault(x => x.HammaddeCinsiId== id);
        }
        public override HammaddeCinsi Get(Expression<Func<HammaddeCinsi, bool>> filter)
        {
            return dbset
                .Include("Kalip")
                .Include("TableHammaddeBirim")
                .Include("HammaddeFire")
                .FirstOrDefault(filter);
        }
        public override IQueryable<HammaddeCinsi> GetAll()
        {
            return dbset
                .Include("Kalip")
                .Include("TableHammaddeBirim")
                .Include("HammaddeFire")
               .AsQueryable();
        }

        public override IQueryable<HammaddeCinsi> GetAll(Expression<Func<HammaddeCinsi, bool>> filter)
        {
            return dbset
                .Include("Kalip")
                .Include("TableHammaddeBirim")
                .Include("HammaddeFire")
                .Where(filter)
               .AsQueryable();
        }
    }
}