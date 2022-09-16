using API_DMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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


        [HttpGet("{media_id}")]
        public JsonResult Get(string media_id)
        {
            string query = "SELECT a.id, a.report_id, b.media_id, b.display_id, b.media_name, b.display_name, a.insertions_counted, a.period_start_date, a.period_end_date FROM report_insertions AS a " +
                "INNER JOIN report AS b ON b.id = a.report_id " +
                $"WHERE b.media_id = '{media_id}'";

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

        [HttpGet]
        public JsonResult Get()
        {
            string query = "SELECT a.id, a.report_id, b.media_id, b.display_id, b.media_name, b.display_name, a.insertions_counted, a.period_start_date, a.period_end_date FROM report_insertions AS a " +
                "INNER JOIN report AS b ON b.id = a.report_id ";

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



        [HttpPost]
        public bool Post([FromBody] ReportPostRequest reportPostRequest)
        {

            int? report_id;
            DateTime startDate = DateTime.Parse(reportPostRequest.period_start_date);
            DateTime endDate = DateTime.Parse(reportPostRequest.period_end_date);

            string sqlDataSource = _configuration.GetConnectionString("DmsAppCon");

            MySqlConnection mycon = new MySqlConnection(sqlDataSource);
            mycon.Open();

            #region VERIFICA SE EXISTE REPORT
            
            string findReport = $"(SELECT * FROM report WHERE media_id = '{reportPostRequest.media_id}' AND display_token = '{reportPostRequest.display_token}')";

            using (MySqlCommand myCommand = new MySqlCommand(findReport, mycon)) {
                
                MySqlDataReader myReader = myCommand.ExecuteReader();
                
                if (myReader.HasRows)
                {
                    DataTable table = new DataTable();
                    table.Load(myReader);
                    report_id = (int)table.Rows[0]["id"];
                }
                else
                {
                    report_id = InsertNewReport(reportPostRequest.display_token, reportPostRequest.media_id);
                }

                myReader.Close();

            };

            #endregion

            if (report_id == null) 
            {
                mycon.Close();
                return false;
            }

            #region INSERE NOVO REPORT_INSERTION_ITEM

            CultureInfo ci = CultureInfo.InvariantCulture;

            string query_insert_report =
                "INSERT INTO report_insertions " +
                "(report_id, insertions_counted, period_start_date, period_end_date, created_date) " +
                $"VALUES('{report_id}','{reportPostRequest.insertions_counted}','{startDate.ToString("yyyy-MM-dd hh:mm:ss", ci)}','{endDate.ToString("yyyy-MM-dd hh:mm:ss", ci)}', NOW())";

            using (MySqlCommand myCommand = new MySqlCommand(query_insert_report, mycon))
            {
                MySqlDataReader myReader = myCommand.ExecuteReader();

                if (myReader.RecordsAffected > 0)
                {
                    myReader.Close();
                    mycon.Close();
                    return true;
                }
                else {
                    myReader.Close();
                    mycon.Close();
                    return false;
                }
                    
            }

            #endregion
            
        }

        private Nullable<int> InsertNewReport(string display_token, int media_id)
        {
            Nullable<int> report_id;
            Nullable<int> display_id;
            string media_name = string.Empty;
            string display_name = string.Empty;
            string display_location = string.Empty;
            string display_orientation = string.Empty;
            string display_size = string.Empty;
            string sqlDataSource = _configuration.GetConnectionString("DmsAppCon");

            MySqlConnection mycon = new MySqlConnection(sqlDataSource);
            mycon.Open();

            #region MEDIA

            string queryMedia = $"(SELECT * FROM media WHERE id = '{media_id}')";

            using (MySqlCommand myCommand = new MySqlCommand(queryMedia, mycon))
            {

                MySqlDataReader myReader = myCommand.ExecuteReader();
                
                if (myReader.HasRows)
                {
                    DataTable table = new DataTable();
                    table.Load(myReader);
                    media_name = table.Rows[0]["name"].ToString();
                }
                else
                {
                    myReader.Close();
                    mycon.Close();

                    return null;
                }

                myReader.Close();
            };

            #endregion

            #region DISPLAY

            string queryDisplay = $"(SELECT * FROM display WHERE token = '{display_token}')";

            using (MySqlCommand myCommand = new MySqlCommand(queryDisplay, mycon))
            {

                MySqlDataReader myReader = myCommand.ExecuteReader();

                if (myReader.HasRows)
                {
                    DataTable table = new DataTable();
                    table.Load(myReader);

                    display_id = (int)table.Rows[0]["id"];
                    display_name = table.Rows[0]["name"].ToString();
                    display_location = table.Rows[0]["location"].ToString();
                    display_orientation = table.Rows[0]["orientation"].ToString();
                    display_size = table.Rows[0]["display_size"].ToString();
                }
                else
                {
                    myReader.Close();
                    mycon.Close();
                    return null;
                }

                myReader.Close();
            };

            #endregion

            #region INSERT NEW ITEM

            string query_insert =
                "INSERT INTO report (display_id, display_token, display_name, display_location, display_orientation, display_size, media_id, media_name, created_date)" +
                $"VALUES('{display_id}','{display_token}','{display_name}','{display_location}','{display_orientation}','{display_size}','{media_id}','{media_name}', NOW())";

            using (MySqlCommand myCommand = new MySqlCommand(query_insert, mycon))
            {

                MySqlDataReader myReader = myCommand.ExecuteReader();

                if (myReader.RecordsAffected > 0)
                {                    
                    report_id = int.Parse(myCommand.LastInsertedId.ToString());
                }
                else
                {
                    myReader.Close();
                    mycon.Close();
                    return null;
                }

                myReader.Close();
            };

            #endregion
            
            mycon.Close();
            return report_id;

        }
    }
}