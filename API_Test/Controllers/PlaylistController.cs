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
    public class PlaylistController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PlaylistController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET api/<PlaylistController>/5
        [HttpGet("{token}")]
        public JsonResult Get(string token)
        {
            string query_displays = string.Format("SELECT * FROM display WHERE (is_active = true) AND (token = '{0}')", token);
            string query_medias = string.Format("SELECT * FROM media WHERE is_active = true AND (start_date <= NOW() AND end_date >= NOW())");
            string sqlDataSource = _configuration.GetConnectionString("DmsAppCon");
            
            MySqlConnection mycon = new MySqlConnection(sqlDataSource);
            MySqlDataReader myReader;
            DataTable mediasTable = new DataTable();
            DataTable displaysTable = new DataTable();
            List<Models.Media> mediasList = new List<Models.Media>();

            Models.ActiveMedias activeMedias = new Models.ActiveMedias();
            Models.DisplaySetup displaySetup = new Models.DisplaySetup();
            activeMedias.token = token;
            activeMedias.count = 0;
            activeMedias.timestamp = DateTime.Now;
            int display_id;

            if (string.IsNullOrEmpty(token))
                return new JsonResult(activeMedias);

            mycon.Open();

            using (MySqlCommand myCommand = new MySqlCommand(query_displays, mycon))
            {
                myReader = myCommand.ExecuteReader();
                displaysTable.Load(myReader);

                if (displaysTable.Rows.Count == 0)
                    return new JsonResult(activeMedias);
                else
                {
                    // GET ID
                    display_id = (int)displaysTable.Rows[0]["id"];
                    activeMedias.display_name = displaysTable.Rows[0]["name"].ToString();

                    // GET SETUP INFO
                    int playlist_update_interval;
                    int.TryParse(displaysTable.Rows[0]["playlist_update_interval"].ToString(), out playlist_update_interval);
                    displaySetup.playlist_update_interval = playlist_update_interval;

                    int report_update_interval;
                    int.TryParse(displaysTable.Rows[0]["report_update_interval"].ToString(), out report_update_interval);
                    displaySetup.report_update_interval = report_update_interval;

                    displaySetup.timetable = displaysTable.Rows[0]["timetable"].ToString();

                }

                myReader.Close();
            }

            using (MySqlCommand myCommand = new MySqlCommand(query_medias, mycon))
            {
                myReader = myCommand.ExecuteReader();
                mediasTable.Load(myReader);

                for (int i = 0; i < mediasTable.Rows.Count; i++)
                {

                    var item = mediasTable.Rows[i];
                    bool expose_at_all = Convert.ToBoolean(item["expose_at_all"]);
                    bool is_valid = false;

                    if (expose_at_all)
                        is_valid = true;
                    else
                    {
                        string expose_in_str = (string)item["expose_in"];
                        string[] expose_in_array = expose_in_str.Split(',');
                        ArrayList expose_in_list = new ArrayList();
                        expose_in_list.AddRange(expose_in_array);

                        if (expose_in_list.Contains(display_id.ToString()))
                            is_valid = true;
                    }

                    if (is_valid)
                    {

                        mediasList.Add(new Models.Media
                        {

                            id = (int)item["id"],
                            name = (string)item["name"],
                            media_url = (string)item["media_url"],
                            media_type = (string)item["media_type"],
                            media_type_id = (Convert.IsDBNull(item["media_type_id"]) ? null : (int?)item["media_type_id"]),
                            expose_timing = (Convert.IsDBNull(item["expose_timing"]) ? null : (int?)item["expose_timing"]),
                            expose_in = (string)item["expose_in"],
                            start_date = (Convert.IsDBNull(item["start_date"]) ? null : (DateTime?)item["start_date"]),
                            end_date = (Convert.IsDBNull(item["end_date"]) ? null : (DateTime?)item["end_date"]),
                            expose_at_all = (Convert.ToBoolean(item["expose_at_all"])),
                            is_active = (Convert.ToBoolean(item["expose_at_all"])),
                            last_modified_date = (Convert.IsDBNull(item["last_modified_date"]) ? null : (DateTime?)item["last_modified_date"]),
                            insertions_limit = (Convert.IsDBNull(item["insertions_limit"]) ? null : (int?)item["insertions_limit"]),
                            created_by = (string)item["created_by"],
                            created_date = (Convert.IsDBNull(item["start_date"]) ? null : (DateTime?)item["start_date"]),

                        });
                    }

                }

                activeMedias.display_id = display_id;
                activeMedias.count = mediasList.Count;
                activeMedias.medias = mediasList;
                activeMedias.display_setup = displaySetup;

                myReader.Close();

            }

            mycon.Close();

            return new JsonResult(activeMedias);
        }

       
    }
}
