using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EgePakErp.Models.Audit
{
    public interface IAuditLogEntity
    {
        // Override this method to provide a description of the entity for audit purposes
        string JsonString();
    }
}
 