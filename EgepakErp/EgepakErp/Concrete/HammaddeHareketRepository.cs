using EgePakErp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace EgePakErp.Concrete
{
    public class HammaddeHareketRepository : _GenericRepository<HammaddeHareket>
    {
        public override HammaddeHareket Get(int id)
        {
            return dbset
                .Include("Doviz")
                .Include("HammaddeCinsi")
                .Include("TableHammaddeBirim")
               .FirstOrDefault(x => x.HammaddeCinsiId== id);
        }
        public override HammaddeHareket Get(Expression<Func<HammaddeHareket, bool>> filter)
        {
            return dbset
                .Include("Doviz")
                .Include("HammaddeCinsi")
                .Include("TableHammaddeBirim")
                .FirstOrDefault(filter);
        }
        public override IQueryable<HammaddeHareket> GetAll()
        {
            return dbset
                .Include("Doviz")
                .Include("HammaddeCinsi")
                .Include("TableHammaddeBirim")
               .AsQueryable();
        }

        public override IQueryable<HammaddeHareket> GetAll(Expression<Func<HammaddeHareket, bool>> filter)
        {
            return dbset
                .Include("Doviz")
                .Include("HammaddeCinsi")
                .Include("TableHammaddeBirim")
                .Where(filter)
               .AsQueryable();
        }
    }
}