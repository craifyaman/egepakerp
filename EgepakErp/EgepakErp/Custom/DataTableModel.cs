using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EgePakErp.Custom
{
    public class DataTableModel<T>
    {
        public DataTableMeta meta { get; set; }
        public List<T> data { get; set; }
    }
}