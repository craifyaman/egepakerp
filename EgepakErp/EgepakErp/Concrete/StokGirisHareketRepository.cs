using EgePakErp.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace EgePakErp.Concrete
{
    public class StokGirisHareketRepository : _GenericRepository<StokGirisHareket>
    {
        public override IQueryable<StokGirisHareket> AllInclude()
        {
            return dbset
                .Include(x => x.StokHareket)
                .AsQueryable();
        }
        public override StokGirisHareket Get(int id)
        {
            return AllInclude().FirstOrDefault(x => x.StokHareketId == id);
        }
        public override StokGirisHareket Get(Expression<Func<StokGirisHareket, bool>> filter)
        {
            return AllInclude().FirstOrDefault(filter);
        }
        public override IQueryable<StokGirisHareket> GetAll()
        {
            return AllInclude();
        }

        public override IQueryable<StokGirisHareket> GetAll(Expression<Func<StokGirisHareket, bool>> filter)
        {
            return AllInclude().Where(filter);
        }
    }
}