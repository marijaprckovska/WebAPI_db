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
    public class MeasureConversionsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public MeasureConversionsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
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
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(MeasureConversions mct)
        {
            string query = @"
                           insert into dbo.MeasureConversions
                           (mcn_sMeasureUnitCode, mcn_sFormula)
                           values (@mcn_sMeasureUnitCode, @mcn_sFormula)
                      ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@mcn_sMeasureUnitCode", mct.mcn_sMeasureUnitCode);
                    myCommand.Parameters.AddWithValue("@mcn_sFormula", mct.mcn_sFormula);
                    
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }


        [HttpPut]
        public JsonResult Put(MeasureConversions mct)
        {
            string query = @"
                           update dbo.MeasureConversions
                           set mcn_sMeasureUnitCode=@mcn_sMeasureUnitCode, mcn_sFormula=@mcn_sFormula
                           where mcn_nAutoinc=@mcn_nAutoinc
                           ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@mcn_nAutoinc", mct.mcn_nAutoinc);
                    myCommand.Parameters.AddWithValue("@mcn_sMeasureUnitCode", mct.mcn_sMeasureUnitCode);
                    myCommand.Parameters.AddWithValue("@mcn_sFormula", mct.mcn_sFormula);
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
                           delete from dbo.MeasureConversions
                           where mcn_nAutoinc=@mcn_nAutoinc
                      ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@mcn_nAutoinc", id);
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





