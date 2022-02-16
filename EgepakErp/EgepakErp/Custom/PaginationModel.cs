using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EgePakErp.Models
{
    public class PaginationModel
    {
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public long Count { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
    }
}