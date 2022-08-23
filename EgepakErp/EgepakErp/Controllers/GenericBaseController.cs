using EgePakErp.Concrete;

namespace EgePakErp.Controllers
{
    public class GenericBaseController<T> : BaseController where T : class
    {
        public _GenericRepository<T> BaseRepo { get; set; }
        public GenericBaseController()
        {
            BaseRepo = new _GenericRepository<T>();
        }
    }
}