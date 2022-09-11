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

namespace WebAPI_db.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasureDatetimeZonesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public MeasureDatetimeZonesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                           select mdc_nAutoinc, mdc_sZone, mdc_sRegion, mdc_sDefaultDateFormat, mdc_sUtcTimezone, mdc_sUtcTimezoneDST
                           from 
                           dbo.MeasureDatetimeZones
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
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(MeasureDatetimeZones mdct)
        {
            string query = @"
                           insert into dbo.MeasureDatetimeZones
                           (mdc_sZone, mdc_sRegion, mdc_sDefaultDateFormat, mdc_sUtcTimezone, mdc_sUtcTimezoneDST)
                           values (@mdc_sZone, @mdc_sRegion, @mdc_sDefaultDateFormat, @mdc_sUtcTimezone, @mdc_sUtcTimezoneDST)
                      ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@mdc_sZone", mdct.mdc_sZone);
                    myCommand.Parameters.AddWithValue("@mdc_sRegion", mdct.mdc_sRegion);
                    myCommand.Parameters.AddWithValue("@mdc_sDefaultDateFormat", mdct.mdc_sDefaultDateFormat);
                    myCommand.Parameters.AddWithValue("@mdc_sUtcTimezone", mdct.mdc_sUtcTimezone);
                    myCommand.Parameters.AddWithValue("@mdc_sUtcTimezoneDST", mdct.mdc_sUtcTimezoneDST);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }


        [HttpPut]
        public JsonResult Put(MeasureDatetimeZones mdct)
        {
            string query = @"
                           update dbo.MeasureDatetimeZones
                           mdc_sZone=@mdc_sZone, mdc_sRegion=@mdc_sRegion, mdc_sDefaultDateFormat=@mdc_sDefaultDateFormat, mdc_sUtcTimezone=@ mdc_sUtcTimezone, mdc_sUtcTimezoneDST=@mdc_sUtcTimezoneDST
                           where mdc_nAutoinc=@mdc_nAutoinc
                           ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@mdc_nAutoinc", mdct.mdc_nAutoinc);
                    myCommand.Parameters.AddWithValue("@mdc_sZone", mdct.mdc_sZone);
                    myCommand.Parameters.AddWithValue("@mdc_sRegion", mdct.mdc_sRegion);
                    myCommand.Parameters.AddWithValue("@mdc_sDefaultDateFormat", mdct.mdc_sDefaultDateFormat);
                    myCommand.Parameters.AddWithValue("@mdc_sUtcTimezone", mdct.mdc_sUtcTimezone);
                    myCommand.Parameters.AddWithValue("@mdc_sUtcTimezoneDST", mdct.mdc_sUtcTimezoneDST);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Updated Successfully");
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                           delete from dbo.MeasureDatetimeZones
                           where mdc_nAutoinc=@mdc_nAutoinc
                      ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@mdc_nAutoinc", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }
    }
}

