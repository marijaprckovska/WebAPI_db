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
    public class MeasureDefaultTypesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public MeasureDefaultTypesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                           select mdt_nAutoinc, mdt_sTypeCode, mdt_sDefaultMeasureUnit
                           from 
                           dbo.MeasureDefaultTypes
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
        public JsonResult Post(MeasureDefaultTypes mdtt)
        {
            string query = @"
                           insert into dbo.MeasureDefaultTypes
                           (mdt_sTypeCode, mdt_sDefaultMeasureUnit)
                           values (@mdt_sTypeCode, @mdt_sDefaultMeasureUnit)
                      ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@mdt_sTypeCode", mdtt.mdt_sTypeCode);
                    myCommand.Parameters.AddWithValue("@mdt_sDefaultMeasureUnit", mdtt.mdt_sDefaultMeasureUnit);
                    
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }


        [HttpPut]
        public JsonResult Put(MeasureDefaultTypes mdtt)
        {
            string query = @"
                           update dbo.MeasureDefaultTypes
                           set mdt_sTypeCode=@mdt_sTypeCode, mdt_sDefaultMeasureUnit=@mdt_sDefaultMeasureUnit
                           where mdt_nAutoinc=@mdt_nAutoinc
                           ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@mdt_nAutoinc", mdtt.mdt_nAutoinc);
                    myCommand.Parameters.AddWithValue("@mdt_sTypeCode", mdtt.mdt_sTypeCode);
                    myCommand.Parameters.AddWithValue("@mdt_sDefaultMeasureUnit", mdtt.mdt_sDefaultMeasureUnit);
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
                           delete from dbo.MeasureDefaultTypes
                           where mdt_nAutoinc=@mdt_nAutoinc
                      ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@mdt_nAutoinc", id);
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






