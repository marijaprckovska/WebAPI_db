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
    public class MeasureUnitsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public MeasureUnitsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                           select mun_nAutoinc, mun_sCode, mun_sSymbol, mun_sMeasureType
                           from 
                           dbo.MeasureUnits
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
        public JsonResult Post(MeasureUnits mut)
        {
            string query = @"
                           insert into dbo.MeasureUnits
                           (mun_sCode, mun_sSymbol, mun_sMeasureType)
                           values (@mun_sCode, @mun_sSymbol, @mun_sMeasureType)
                      ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@mun_sCode", mut.mun_sCode);
                    myCommand.Parameters.AddWithValue("@mun_sSymbol", mut.mun_sSymbol);
                    myCommand.Parameters.AddWithValue("@mun_sMeasureType", mut.mun_sMeasureType);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }


        [HttpPut]
        public JsonResult Put(MeasureUnits mut)
        {
            string query = @"
                           update dbo.MeasureUnits
                           set mun_sCode=@mun_sCode, mun_sSymbol=@mun_sSymbol, mun_sMeasureType=@mun_sMeasureType
                           where mun_nAutoinc=@mun_nAutoinc
                           ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@mun_nAutoinc", mut.mun_nAutoinc);
                    myCommand.Parameters.AddWithValue("@mun_sCode", mut.mun_sCode);
                    myCommand.Parameters.AddWithValue("@mun_sSymbol", mut.mun_sSymbol);
                    myCommand.Parameters.AddWithValue("@mun_sMeasureType", mut.mun_sMeasureType);
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
                           delete from dbo.MeasureUnits
                           where mun_nAutoinc=@mun_nAutoinc
                      ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@mun_nAutoinc", id);
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








