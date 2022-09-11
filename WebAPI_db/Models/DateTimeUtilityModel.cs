using System;

namespace WebAPI_db.Controllers
{
    internal class DateTimeUtilityModel
    {

        public DateTimeUtilityModel(string index, string value, string type, string destformat)
        {
            this.Index = index;
            this.Value = value;
            this.Type = type;
            this.Destformat = destformat;
        }

        public string Destformat { get; internal set; }
        public string Value { get; internal set; }
        public string Index { get; internal set; }
        public string Type { get; internal set; }
    }
}