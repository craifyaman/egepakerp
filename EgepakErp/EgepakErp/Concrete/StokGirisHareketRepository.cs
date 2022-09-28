using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace EgePakErp.Concrete
{
    public class StokGirisHareketRepository : _GenericRepository<StokGirisHareket>
    {
      
        public override StokGirisHareket Get(int id)
        {
            return dbset
                .Include(x => x.StokHareket)
                .FirstOrDefault(x => x.StokGirisHareketId== id);
        }
        public override StokGirisHareket Get(Expression<Func<StokGirisHareket, bool>> filter)
        {
            return dbset
                .Include(x => x.StokHareket)
                .FirstOrDefault(filter);
        }
        public override IQueryable<StokGirisHareket> GetAll()
        {
            return dbset
                .Include(x => x.StokHareket)
                .AsQueryable();
        }

        public override IQueryable<StokGirisHareket> GetAll(Expression<Func<StokGirisHareket, bool>> filter)
        {
            return dbset
                .Include(x => x.StokHareket)
                .Where(filter).AsQueryable();
        }
    }
}