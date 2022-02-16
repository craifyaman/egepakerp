using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EgePakErp.Models.Audit
{
    [Table("AuditLog")]
    public class AuditLog
    {
        [Key]
        public Guid AuditLogID { get; set; }

        [Required]
        public int PersonelId { get; set; }
        public Personel Personel { get; set; }

        [Required]
        public DateTime EventDateUTC { get; set; }

        [Required]
        public int EventType { get; set; }

        [Required]
        [MaxLength(100)]
        public string TableName { get; set; }

        [Required]
        public int RecordID { get; set; }

        [Required]
        [MaxLength(100)]
        public string ColumnName { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
        public string OriginalJson { get; set; }
        public string NewJson { get; set; }
    }
}