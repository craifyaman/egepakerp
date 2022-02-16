using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EgePakErp.Custom
{
    public class YetkiAttribute : Attribute,IActionFilter   
    {
        public YetkiAttribute(string Description,string Group)
        {
            this.Description = Description;
            this.Group = Group;
        }

        public string Description { get; set; }
        public string Group { get; set; }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
}