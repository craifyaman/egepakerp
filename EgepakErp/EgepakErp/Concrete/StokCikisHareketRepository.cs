using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace EgePakErp.Concrete
{
    public class StokCikisHareketRepository : _GenericRepository<StokCikisHareket>
    {
        public override IQueryable<StokCikisHareket> AllInclude()
        {
            return dbset
                .Include(x => x.StokHareket)
                .Include(x => x.Cari)
                .AsQueryable();
        }
        public override StokCikisHareket Get(int id)
        {
            return AllInclude().FirstOrDefault(x => x.StokHareketId == id);
        }
        public override StokCikisHareket Get(Expression<Func<StokCikisHareket, bool>> filter)
        {
            return AllInclude().FirstOrDefault(filter);
        }
        public override IQueryable<StokCikisHareket> GetAll()
        {
            return AllInclude();
        }

        public override IQueryable<StokCikisHareket> GetAll(Expression<Func<StokCikisHareket, bool>> filter)
        {
            return AllInclude().Where(filter);
        }
    }
}