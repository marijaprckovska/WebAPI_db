using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_db.Models
{
    public class MeasureCultures
    {
        public int mcl_nAutoinc { get; set; }
        public string mcl_sCulture { get; set; }
        public string mcl_sDefaultMeasureType { get; set; }
        public string mcl_sDefaultMeasureUnit { get; set; }
    }
}
