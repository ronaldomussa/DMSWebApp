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
    public class DisplayController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DisplayController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET api/<DisplayController>/5
        [HttpGet("{token}")]
        public JsonResult Get(string token)
        {
            string query_displays = string.Format("SELECT * FROM display WHERE (is_active = true) AND (token = '{0}')", token);
            string sqlDataSource = _configuration.GetConnectionString("DmsAppCon");

            MySqlConnection mycon = new MySqlConnection(sqlDataSource);
            MySqlDataReader myReader;
            DataTable displasTable = new DataTable();

            if (string.IsNullOrEmpty(token))
                return new JsonResult(null);

            mycon.Open();

            using (MySqlCommand myCommand = new MySqlCommand(query_displays, mycon))
            {
                myReader = myCommand.ExecuteReader();
                displasTable.Load(myReader);

                if (displasTable.Rows.Count == 0)
                    return new JsonResult(null);

                myReader.Close();
            }

            mycon.Close();

            return new JsonResult(displasTable);
        }

        
    }
}
