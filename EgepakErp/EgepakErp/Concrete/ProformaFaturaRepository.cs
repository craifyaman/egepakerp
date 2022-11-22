using EgePakErp.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace EgePakErp.Concrete
{
    public class ProformaFaturaRepository : _GenericRepository<ProformaFatura>
    {
        public override IQueryable<ProformaFatura> AllInclude()
        {

            return dbset
               .Include(x => x.Cari)
               .Include(x => x.ProformaUrun)
               .Include(x => x.Doviz)
               .AsQueryable();
        }
        public override ProformaFatura Get(int id)
        {
            return AllInclude().FirstOrDefault(x => x.ProformaFaturaId == id);
        }
        public override ProformaFatura Get(Expression<Func<ProformaFatura, bool>> filter)
        {
            return AllInclude().FirstOrDefault(filter);
        }
        public override IQueryable<ProformaFatura> GetAll()
        {
            return AllInclude();
        }

        public override IQueryable<ProformaFatura> GetAll(Expression<Func<ProformaFatura, bool>> filter)
        {
            return AllInclude().Where(filter);
        }
    }
}