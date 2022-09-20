using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace EgePakErp.Concrete
{
    public class StokCikisHareketRepository : _GenericRepository<StokCikisHareket>
    {
      
        public override StokCikisHareket Get(int id)
        {
            return dbset
                .Include(x => x.StokHareket)
                .Include(x => x.Cari)
                .FirstOrDefault(x => x.StokCikisHareketId== id);
        }
        public override StokCikisHareket Get(Expression<Func<StokCikisHareket, bool>> filter)
        {
            return dbset
                .Include(x => x.StokHareket)
                .Include(x => x.Cari)
                .FirstOrDefault(filter);
        }
        public override IQueryable<StokCikisHareket> GetAll()
        {
            return dbset
                .Include(x => x.StokHareket)
                .Include(x => x.Cari)
                .AsQueryable();
        }

        public override IQueryable<StokCikisHareket> GetAll(Expression<Func<StokCikisHareket, bool>> filter)
        {
            return dbset
                .Include(x => x.StokHareket)
                .Include(x => x.Cari)
                .Where(filter).AsQueryable();
        }
    }
}