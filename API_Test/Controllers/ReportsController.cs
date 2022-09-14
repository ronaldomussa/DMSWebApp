using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ReportsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET api/<ReportsController>/5
        [HttpGet]
        public JsonResult Get()
        {
            string query =
                string.Format("SELECT * FROM report_insertions");

            string sqlDataSource = _configuration.GetConnectionString("DmsAppCon");

            MySqlConnection mycon = new MySqlConnection(sqlDataSource);
            MySqlDataReader myReader;
            DataTable table = new DataTable();

            mycon.Open();

            using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
            {
                myReader = myCommand.ExecuteReader();

                table.Load(myReader);

                myReader.Close();
            }

            mycon.Close();

            return new JsonResult(table);
        }

        // POST api/<ReportsController>
        [HttpPost]
        public bool Post(string display_token, int media_id, int insertions_counted, string period_start_date, string period_end_date)
        {

            DateTime startDate = DateTime.Parse(period_start_date);
            DateTime endDate = DateTime.Parse(period_end_date);

            string queryMedia = $"(SELECT * FROM media WHERE id = {media_id})";
            string queryDisplay = $"(SELECT * FROM display WHERE token = '{display_token}')";
            
            int display_id = 0;
            string display_name = string.Empty; 
            string display_location = string.Empty;
            string media_name = string.Empty;

            string sqlDataSource = _configuration.GetConnectionString("DmsAppCon");

            MySqlConnection mycon = new MySqlConnection(sqlDataSource);
            MySqlDataReader myReader;

            mycon.Open();

            MySqlCommand myCommand = new MySqlCommand(queryMedia, mycon);

            myReader = myCommand.ExecuteReader();

            if (myReader.HasRows)
            {
                DataTable table = new DataTable();
                table.Load(myReader);

                media_name = table.Rows[0]["name"].ToString();
            }
            else
                return false;

            myReader.Close();

            myCommand = new MySqlCommand(queryDisplay, mycon);
            myReader = myCommand.ExecuteReader();

            if (myReader.HasRows)
            {
                DataTable table = new DataTable();
                table.Load(myReader);

                display_id       = (int)table.Rows[0]["id"];
                display_name     = table.Rows[0]["name"].ToString();
                display_location = table.Rows[0]["location"].ToString();
            }
            else
                return false;

            myReader.Close();

            string query_insert_report =
                string.Format("INSERT INTO report_insertions " +
                "(display_id, display_token, display_name, display_location, media_id, media_name, insertions_counted, period_start_date, period_end_date, created_date)" +
                "VALUES({0},'{1}','{2}','{3}',{4},'{5}',{6},'{7}','{8}', NOW())", display_id, display_token,display_name, display_location, media_id, media_name, insertions_counted, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

            myCommand = new MySqlCommand(query_insert_report, mycon);
            myReader = myCommand.ExecuteReader();
            myReader.Close();
            mycon.Close();

            if (myReader.RecordsAffected > 0)
                return true;
            else
                return false;
            
            

        }

        
    }
}
