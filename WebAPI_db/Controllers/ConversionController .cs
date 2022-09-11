using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebAPI_db.Models;
using Newtonsoft.Json;
using System.Dynamic;
using NCalc;
using System.Globalization;


namespace WebAPI_db.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Dictionary<string, string> _measuresDefaultByType = new Dictionary<string, string>();
        private Dictionary<string, string> _measuresConversion = new Dictionary<string, string>();
        private Dictionary<string, Dictionary<string, string>> _measuresDefaultByCulture = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, string> _dateformatDefaultByCulture = new Dictionary<string, string>();

        public ConversionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult ExecuteConversion([FromBody] object jsonobj)
        {
            string query = @"
                           select mcn_nAutoinc, mcn_sMeasureUnitCode, mcn_sFormula
                           from 
                           dbo.MeasureConversions
                      ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
                for(int i = 0; i < table.Rows.Count; i++)
                {
                    _measuresConversion.Add(table.Rows[i].Field<string>("mcn_sMeasureUnitCode"),table.Rows[i].Field<string>("mcn_sFormula"));
                }
            }

            query = @"
                           select mdt_sTypeCode, mdt_sDefaultMeasureUnit
                           from 
                           dbo.MeasureDefaultTypes
                      ";
            table = new DataTable();
             sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    _measuresDefaultByType.Add(table.Rows[i].Field<string>("mdt_sTypeCode"), table.Rows[i].Field<string>("mdt_sDefaultMeasureUnit"));
                }
            }


            for (int j = 0; j < _measuresDefaultByType.Count; j++)
            {
                string item = _measuresDefaultByType.ElementAt(j).Key;
                Dictionary<string, string> temp = new Dictionary<string, string>();
                query = @"
                           select mcl_sCulture, mcl_sDefaultMeasureUnit
                           from dbo.MeasureCultures
                            where mcl_sDefaultMeasureType = '" + item+"'";

                table = new DataTable();
                sqlDataSource = _configuration.GetConnectionString("DBAppCon");
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))

                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        temp.Add(table.Rows[i].Field<string>("mcl_sCulture"), table.Rows[i].Field<string>("mcl_sDefaultMeasureUnit"));
                    }
                }
                _measuresDefaultByCulture.Add(item, temp);
            }

                dynamic json = JsonConvert.DeserializeObject(jsonobj.ToString());

            // dynamic json = JsonConvert.DeserializeObject(jsonobj.ToString());
            string _originCulture = "";
            string _destinationCulture = "";
            dynamic _jsonvaluesarray = null;

            _originCulture = json["originculture"];
            _destinationCulture = json["destculture"];
            _jsonvaluesarray = json["values"];

            (int res, string resultmessage) result = this._ExecuteConversion(_originCulture, _destinationCulture, _jsonvaluesarray);
            if (result.res > -1)
            {
                return Ok(result.resultmessage);
            }
            else
            {
                return StatusCode(500, "error");
            }

        }


        public (int res, string resultmessage) _ExecuteConversion(string originCulture, string destinationCulture, dynamic jsonvaluesarray)
        {
            (int res, string resultmessage) res = (-1, "");

            List<MeasureUtilityModel> measures = new List<MeasureUtilityModel>();
            List<CurrencyUtilityModel> currency = new List<CurrencyUtilityModel>();
            List<DateTimeUtilityModel> dates = new List<DateTimeUtilityModel>();

            List<MeasureUtilityModel> destmeasures = new List<MeasureUtilityModel>();
            List<CurrencyUtilityModel> destcurrency = new List<CurrencyUtilityModel>();
            List<DateTimeUtilityModel> destdates = new List<DateTimeUtilityModel>();

            ExpandoObject _obj = new ExpandoObject();
            for (int i = 0; i < jsonvaluesarray.Count; i++)
            {
                string _type = jsonvaluesarray[i]["type"];
                if (_type == "currency")
                {
                    string _index = jsonvaluesarray[i]["index"];
                    string _value = jsonvaluesarray[i]["value"];
                    string _measureunit = jsonvaluesarray[i]["measureunit"];
                    string _destmeasureunit = jsonvaluesarray[i]["destmeasureunit"];
                    currency.Add(new CurrencyUtilityModel(_index, _value, _type, _measureunit, _destmeasureunit));
                }
                else if (_type == "datetime" || _type == "date" || _type == "time")
                {
                    string _index = jsonvaluesarray[i]["index"];
                    string _value = jsonvaluesarray[i]["value"];
                    string _destformat = jsonvaluesarray[i]["destformat"];
                    dates.Add(new DateTimeUtilityModel(_index, _value, _type, _destformat));
                }
                else
                {
                    string _index = jsonvaluesarray[i]["index"];
                    string _value = jsonvaluesarray[i]["value"];
                    string _measureunit = jsonvaluesarray[i]["measureunit"];
                    string _destmeasureunit = jsonvaluesarray[i]["destmeasureunit"];
                    measures.Add(new MeasureUtilityModel(_index, _value, _type, _measureunit, _destmeasureunit));
                }
            }
            // destmeasures
            foreach (MeasureUtilityModel measure in measures)
            {
                string resultmeasure = null;
                string resultmeasureunit = null;
                // get the measure unit of reference and convert it to normal in case it is not normalized
                if (this._measuresDefaultByType.TryGetValue(measure.Type, out string defaultmeasureunit))
                {
                    // make conversion in case it is not the normalized
                    if (measure.Measureunit != defaultmeasureunit)
                    {
                        if (this._measuresConversion.TryGetValue(measure.Measureunit, out string conversionformula))
                        {
                            string expr = String.Format("{0} / ({1})", measure.Value, conversionformula.Replace("{{" + defaultmeasureunit + "}}", "1"));
                            Expression e = new Expression(expr);

                            // obtain the measure in meters
                            string tempresultmeasure = e.Evaluate().ToString().Replace(",", ".");
                            // resultmeasureunit = "m";
                            if (measure.Destmeasureunit != null)
                            {
                                if (this._measuresConversion.TryGetValue(measure.Destmeasureunit, out string destconversionformula))
                                {
                                    expr = destconversionformula.Replace("{{" + defaultmeasureunit + "}}", tempresultmeasure);
                                    e = new Expression(expr);

                                    // obtain the measure in destination measure
                                    resultmeasure = e.Evaluate().ToString().Replace(",", ".");
                                    resultmeasureunit = measure.Destmeasureunit;
                                }
                            }
                            // no cast destination is used so the conversion can be done using destination culture
                            else if (this._measuresDefaultByCulture.TryGetValue(measure.Type, out Dictionary<string, string> _d) && _d.TryGetValue(destinationCulture, out string _destmeasureunitbyculture))
                            {
                                if (this._measuresConversion.TryGetValue(_destmeasureunitbyculture, out string destconversionformula))
                                {
                                    expr = destconversionformula.Replace("{{" + defaultmeasureunit + "}}", tempresultmeasure);
                                    e = new Expression(expr);

                                    // obtain the measure in destination measure
                                    resultmeasure = e.Evaluate().ToString().Replace(",", ".");
                                    resultmeasureunit = _destmeasureunitbyculture;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (measure.Destmeasureunit != null)
                        {
                            // it is already in default measure unit, only conversion is necessary
                            if (this._measuresConversion.TryGetValue(measure.Measureunit, out string destconversionformula))
                            {
                                string expr = destconversionformula.Replace("{{" + defaultmeasureunit + "}}", measure.Value);
                                Expression e = new Expression(expr);

                                // obtain the measure in destination measure
                                resultmeasure = e.Evaluate().ToString().Replace(",", ".");
                                resultmeasureunit = measure.Destmeasureunit;
                            }
                        }
                        // no cast destination is used so the conversion can be done using destination culture
                        else if (this._measuresDefaultByCulture.TryGetValue(destinationCulture, out Dictionary<string, string> _d) && _d.TryGetValue((string)measure.Type, out string _destmeasureunitbyculture))
                        {
                            if (this._measuresConversion.TryGetValue(_destmeasureunitbyculture, out string destconversionformula))
                            {
                                string expr = destconversionformula.Replace("{{" + defaultmeasureunit + "}}", measure.Value);
                                Expression e = new Expression(expr);

                                // obtain the measure in destination measure
                                resultmeasure = e.Evaluate().ToString().Replace(",", ".");
                                resultmeasureunit = _destmeasureunitbyculture;
                            }
                        }

                    }
                }

                if (resultmeasure != null)
                {
                    destmeasures.Add(new MeasureUtilityModel(measure.Index, resultmeasure, measure.Type, resultmeasureunit));
                }
            }

            // destdates
            foreach(DateTimeUtilityModel dt in dates)
            {
                string resuldt = null;
                string resultdtformat = null;
                /*if (dt.Destformat == null)
                {
                    DateTime _dt = DateTime.Parse(dt.Value, CultureInfo.CreateSpecificCulture(originCulture), DateTimeStyles.AdjustToUniversal);
                }
                else if (this._dateformatDefaultByCulture.TryGetValue(destinationCulture, out string destformat))
                {
                    DateTime _dt = DateTime.Parse(dt.Value, CultureInfo.CreateSpecificCulture(originCulture), DateTimeStyles.AdjustToUniversal);
                }

                if (resuldt != null)
                {*/
                    destdates.Add(new DateTimeUtilityModel(dt.Index, dt.Value, dt.Type, dt.Destformat));
               // }
            }

            // destcurrency
            foreach (CurrencyUtilityModel curr in currency)
            {
                string resultcurrency = null;
                string resultcurrencyunit = null;
                // get the measure unit of reference and convert it to normal in case it is not normalized
                if (this._measuresDefaultByType.TryGetValue(curr.Type, out string defaultcurrencyunit))
                {
                    // make conversion in case it is not the normalized
                    if (curr.Measureunit != defaultcurrencyunit)
                    {
                        if (this._measuresConversion.TryGetValue(curr.Measureunit, out string conversionformula))
                        {
                            string expr = String.Format("{0} / ({1})", curr.Value, conversionformula.Replace("{{" + defaultcurrencyunit + "}}", "1"));
                            Expression e = new Expression(expr);

                            // obtain the measure in meters
                            string tempresultcurrency = e.Evaluate().ToString().Replace(",", ".");
                            // string tempresultcurrencyunit = "EUR";

                            if (curr.Destmeasureunit != null)
                            {
                                if (this._measuresConversion.TryGetValue(curr.Destmeasureunit, out string destconversionformula))
                                {
                                    expr = destconversionformula.Replace("{{" + defaultcurrencyunit + "}}", tempresultcurrency);
                                    e = new Expression(expr);

                                    // obtain the measure in destination measure
                                    resultcurrency = e.Evaluate().ToString().Replace(",", ".");
                                    resultcurrencyunit = curr.Destmeasureunit;
                                }
                            }
                            // no cast destination is used so the conversion can be done using destination culture
                            else if (this._measuresDefaultByCulture.TryGetValue("currency", out Dictionary<string, string> _d) && _d.TryGetValue(destinationCulture, out string _destcurrbyculture))
                            {
                                if (this._measuresConversion.TryGetValue(_destcurrbyculture, out string destconversionformula))
                                {
                                    expr = destconversionformula.Replace("{{" + defaultcurrencyunit + "}}", tempresultcurrency);
                                    e = new Expression(expr);

                                    // obtain the measure in destination measure
                                    resultcurrency = e.Evaluate().ToString().Replace(",", ".");
                                    resultcurrencyunit = _destcurrbyculture;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (curr.Destmeasureunit != null)
                        {
                            // it is already in default measure unit, only conversion is necessary
                            if (this._measuresConversion.TryGetValue(curr.Measureunit, out string destconversionformula))
                            {
                                string expr = destconversionformula.Replace("{{" + defaultcurrencyunit + "}}", curr.Value);
                                Expression e = new Expression(expr);

                                // obtain the measure in destination measure
                                resultcurrency = e.Evaluate().ToString().Replace(",", ".");
                                resultcurrencyunit = curr.Destmeasureunit;
                            }
                        }
                        // no cast destination is used so the conversion can be done using destination culture
                        else if (this._measuresDefaultByCulture.TryGetValue(destinationCulture, out Dictionary<string, string> _d) && _d.TryGetValue("currency", out string _destcurrbyculture))
                        {
                            if (this._measuresConversion.TryGetValue(_destcurrbyculture, out string destconversionformula))
                            {
                                string expr = destconversionformula.Replace("{{" + defaultcurrencyunit + "}}", curr.Value);
                                Expression e = new Expression(expr);

                                // obtain the measure in destination measure
                                resultcurrency = e.Evaluate().ToString().Replace(",", ".");
                                resultcurrencyunit = _destcurrbyculture;
                            }
                        }
                    }
                }

                if (resultcurrency != null)
                {
                    destcurrency.Add(new CurrencyUtilityModel(curr.Index, resultcurrency, curr.Type, resultcurrencyunit));
                }
            }


            _obj.TryAdd("measures", destmeasures);
            _obj.TryAdd("dates", destdates);
            _obj.TryAdd("currency", destcurrency);

            res.res = 0;
            res.resultmessage = JsonConvert.SerializeObject(_obj);

            return res;
        }
    }
}






