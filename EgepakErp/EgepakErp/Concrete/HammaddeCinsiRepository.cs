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
               .FirstOrDefault(x => x.HammaddeCinsiId== id);
        }
        public override HammaddeCinsi Get(Expression<Func<HammaddeCinsi, bool>> filter)
        {
            return dbset
                .Include("Kalip")
                .FirstOrDefault(filter);
        }
        public override IQueryable<HammaddeCinsi> GetAll()
        {
            return dbset
                .Include("Kalip")
               .AsQueryable();
        }

        public override IQueryable<HammaddeCinsi> GetAll(Expression<Func<HammaddeCinsi, bool>> filter)
        {
            return dbset
                .Include("Kalip")
                .Where(filter)
               .AsQueryable();
        }
    }
}