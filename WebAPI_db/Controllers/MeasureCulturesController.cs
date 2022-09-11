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
    public class MeasureCulturesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public MeasureCulturesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                           select mcl_nAutoinc, mcl_sCulture, mcl_sDefaultMeasureType, mcl_sDefaultMeasureUnit
                           from 
                           dbo.MeasureCultures
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
        public JsonResult Post(MeasureCultures mclt)
        {
            string query = @"
                           insert into dbo.MeasureCultures
                           (mcl_sCulture, mcl_sDefaultMeasureType, mcl_sDefaultMeasureUnit)
                           values (@mcl_sCulture, @mcl_sDefaultMeasureType, @mcl_sDefaultMeasureUnit)
                      ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@mcl_sCulture", mclt.mcl_sCulture);
                    myCommand.Parameters.AddWithValue("@mcl_sDefaultMeasureType", mclt.mcl_sDefaultMeasureType);
                    myCommand.Parameters.AddWithValue("@mcl_sDefaultMeasureUnit", mclt.mcl_sDefaultMeasureUnit);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }


        [HttpPut]
        public JsonResult Put(MeasureCultures mclt)
        {
            string query = @"
                           update dbo.MeasureCultures
                           set mcl_sCulture=@mcl_sCulture, mcl_sDefaultMeasureType=@mcl_sDefaultMeasureType, mcl_sDefaultMeasureUnit=@mcl_sDefaultMeasureUnit
                           where mcl_nAutoinc=@mcl_nAutoinc
                           ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@mcl_nAutoinc", mclt.mcl_nAutoinc);
                    myCommand.Parameters.AddWithValue("@mcl_sCulture", mclt.mcl_sCulture);
                    myCommand.Parameters.AddWithValue("@mcl_sDefaultMeasureType", mclt.mcl_sDefaultMeasureType);
                    myCommand.Parameters.AddWithValue("@mcl_sDefaultMeasureUnit", mclt.mcl_sDefaultMeasureUnit);
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
                           delete from dbo.MeasureCultures
                           where mcl_nAutoinc=@mcl_nAutoinc
                      ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@mcl_nAutoinc", id);
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





