using EgePakErp.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace EgePakErp.Concrete
{
    public class TeklifFormRepository : _GenericRepository<SiparisTeklifForm>
    {
        public override SiparisTeklifForm Get(int id)
        {
            return dbset
               .Include(x=>x.Cari)
               .Include(x=>x.Personel)
               .Include(x=>x.SiparisTeklifFormUrun)
               .FirstOrDefault(x=>x.SiparisTeklifFormId == id);
        }
        public override SiparisTeklifForm Get(Expression<Func<SiparisTeklifForm, bool>> filter)
        {
            return dbset
               .Include(x => x.Cari)
               .Include(x => x.Personel)
               .Include(x => x.SiparisTeklifFormUrun)
               .FirstOrDefault(filter);
        }
        public override IQueryable<SiparisTeklifForm> GetAll()
        {
            return dbset
               .Include(x => x.Cari)
               .Include(x => x.Personel)
               .Include(x => x.SiparisTeklifFormUrun)
               .AsQueryable();
        }

        public override IQueryable<SiparisTeklifForm> GetAll(Expression<Func<SiparisTeklifForm, bool>> filter)
        {
            return dbset
               .Include(x => x.Cari)
               .Include(x => x.Personel)
               .Include(x => x.SiparisTeklifFormUrun)
               .Where(filter)
               .AsQueryable();
        }
    }
}