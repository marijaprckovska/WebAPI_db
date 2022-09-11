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
    public class ResultLogController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ResultLogController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                           select rlg_nincremental, rlg_dtConversion, rlg_sFrom, rlg_sTo
                           from 
                           dbo.ResultLog
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
        public JsonResult Post(ResultLog rlt)
        {
            string query = @"
                           insert into dbo.ResultLog
                           (rlg_dtConversion, rlg_sFrom, rlg_sTo)
                           values (@rlg_dtConversion, @rlg_sFrom, @rlg_sTo)
                      ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@rlg_dtConversion", rlt.rlg_dtConversion);
                    myCommand.Parameters.AddWithValue("@rlg_sFrom", rlt.rlg_sFrom);
                    myCommand.Parameters.AddWithValue("@rlg_sTo", rlt.rlg_sTo);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }


        [HttpPut]
        public JsonResult Put(ResultLog rlt)
        {
            string query = @"
                           update dbo.ResultLog
                           set rlg_dtConversion=@rlg_dtConversion, rlg_sFrom=@rlg_sFrom, rlg_sTo=@rlg_sTo
                           where rlg_nincremental=@rlg_nincremental
                           ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@rlg_nincremental", rlt.rlg_nincremental);
                    myCommand.Parameters.AddWithValue("@rlg_dtConversion", rlt.rlg_dtConversion);
                    myCommand.Parameters.AddWithValue("@rlg_sFrom", rlt.rlg_sFrom);
                    myCommand.Parameters.AddWithValue("@rlg_sTo", rlt.rlg_sTo);
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
                           delete from dbo.ResultLog
                           where rlg_nincremental=@rlg_nincremental
                      ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))

            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@rlg_nincremental", id);
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









