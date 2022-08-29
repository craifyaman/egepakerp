using EgePakErp.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace EgePakErp.Concrete
{
    public class CariRepository : _GenericRepository<Cari>
    {
        public override Cari Get(int id)
        {
            return dbset
               .Include(x=>x.CariGrup)
               .Include(x=>x.Kisi)
               .Include(x=>x.Il)
               .Include(x=>x.Ilce)
               .Include(x=>x.BaglantiTipi)
               .FirstOrDefault(x=>x.CariId== id);
        }
        public override Cari Get(Expression<Func<Cari, bool>> filter)
        {
            return dbset
               .Include(x => x.CariGrup)
               .Include(x => x.Kisi)
               .Include(x => x.Il)
               .Include(x => x.Ilce)
               .Include(x => x.BaglantiTipi)
               .FirstOrDefault(filter);
        }
        public override IQueryable<Cari> GetAll()
        {
            return dbset
               .Include(x => x.CariGrup)
               .Include(x => x.Kisi)
               .Include(x => x.Il)
               .Include(x => x.Ilce)
               .Include(x => x.BaglantiTipi)
               .AsQueryable();
        }

        public override IQueryable<Cari> GetAll(Expression<Func<Cari, bool>> filter)
        {
            return dbset
               .Include(x => x.CariGrup)
               .Include(x => x.Kisi)
               .Include(x => x.Il)
               .Include(x => x.Ilce)
               .Include(x => x.BaglantiTipi)
               .Where(filter)
               .AsQueryable();
        }
    }
}