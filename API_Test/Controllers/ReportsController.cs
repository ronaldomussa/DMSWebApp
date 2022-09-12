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
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ReportsController>
        [HttpPost]
        public void Post([FromBody] int display_id, int media_id, int insertions_counted, string start_date, string end_date)
        {
            string query_insert_report = 
                string.Format("INSERT INTO report (display_id, media_id, insertions_counted, start_date, end_date, created_date)" +
                "VALUES({0},{1},{2},'{4}','{5}', NOW());", display_id, media_id, insertions_counted, start_date, end_date);

            string sqlDataSource = _configuration.GetConnectionString("DmsAppCon");

            MySqlConnection mycon = new MySqlConnection(sqlDataSource);
            MySqlDataReader myReader;

            mycon.Open();

            using (MySqlCommand myCommand = new MySqlCommand(query_insert_report, mycon))
            {
                myReader = myCommand.ExecuteReader();
                myReader.Close();
            }

            mycon.Close();

        }

        // PUT api/<ReportsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReportsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
