namespace WebAPI_db.Controllers
{
    internal class CurrencyUtilityModel
    {
 
        public CurrencyUtilityModel(string index, string resultcurrency, string type, string resultcurrencyunit)
        {
            this.Index = index;
            this.Resultcurrency = resultcurrency;
            this.Type = type;
            this.Resultcurrencyunit = resultcurrencyunit;
        }

        public CurrencyUtilityModel(string index, string value, string type, string measureunit, string destmeasureunit)
        {
            this.Index = index;
            this.Value = value;
            this.Type = type;
            this.Measureunit = measureunit;
            this.Destmeasureunit = destmeasureunit;
        }

        public string Destmeasureunit { get; internal set; }
        public string Measureunit { get; internal set; }
        public string Index { get; internal set; }
        public string Resultcurrency { get; private set; }
        public string Type { get; internal set; }
        public string Resultcurrencyunit { get; private set; }
        public string Value { get; internal set; }
    }
}