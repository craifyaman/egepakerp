using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgePakErp.Models
{
    [Table("BoyaKodType")]
    public class BoyaKodType
    {
        [Key]
        public int BoyaKodTypeId { get; set; }
        public string Type { get; set; }
        public ICollection<BoyaKod> BoyaKod { get; set; }
    }


}