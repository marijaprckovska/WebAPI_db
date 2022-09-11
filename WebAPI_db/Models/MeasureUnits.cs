using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_db.Models
{
    public class MeasureUnits
    {
        public int mun_nAutoinc { get; set; }
        public string mun_sCode { get; set; }
        public string mun_sSymbol { get; set; }
        public string mun_sMeasureType { get; set; }
    }
}
