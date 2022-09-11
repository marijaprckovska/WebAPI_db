namespace WebAPI_db.Controllers
{
    internal class MeasureUtilityModel
    {


        public MeasureUtilityModel(string index, string resultmeasure, string type, string resultmeasureunit)
        {
            this.Index = index;
            this.Resultmeasure = resultmeasure;
            this.Type = type;
            this.Resultmeasureunit = resultmeasureunit;
        }

        public MeasureUtilityModel(string index, string value, string type, string measureunit, string destmeasureunit)
        {
            this.Index = index;
            this.Value = value;
            this.Type = type;
            this.Measureunit = measureunit;
            this.Destmeasureunit = destmeasureunit;
        }

        public string Measureunit { get; internal set; }
        public string Index { get; internal set; }
        public string Resultmeasure { get; private set; }
        public string Type { get; internal set; }
        public string Resultmeasureunit { get; private set; }
        public string Value { get; internal set; }
        public string Destmeasureunit { get; internal set; }
    }
}