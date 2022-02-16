using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EgePakErp.Custom
{
    public class Response
    {
        public bool Success { get; set; }
        public string Description { get; set; }
        public Exception ex { get; set; }
    }

    public class Response<T> : Response
    {
        public T Data;
    }
}