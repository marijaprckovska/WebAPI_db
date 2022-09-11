using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_db.Models
{
    public class MeasureDatetimeZones
    {
        public int mdc_nAutoinc { get; set; }
        public string mdc_sZone { get; set; }
        public string mdc_sRegion { get; set; }
        public string mdc_sDefaultDateFormat { get; set; }
        public string mdc_sUtcTimezone { get; set; }
        public string mdc_sUtcTimezoneDST { get; set; }
    }
}
