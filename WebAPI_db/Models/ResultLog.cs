using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_db.Models
{
    public class ResultLog
    {
        public int rlg_nincremental { get; set; }
        public DateTime rlg_dtConversion { get; set; }
        public string rlg_sFrom { get; set; }
        public string rlg_sTo { get; set; }
    }
}
