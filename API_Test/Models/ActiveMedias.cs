using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Test.Models
{
    public class ActiveMedias { 
        public string token { get; set; }
        public int display_id { get; set; }
        public string display_name { get; set; }
        public DateTime timestamp { get; set; }
        public int count { get; set; }
        public List<Media> medias { get; set; }
        public DisplaySetup display_setup { get; set; }
    }

    public class DisplaySetup
    {
        public int playlist_update_interval { get; set; }
        public int report_update_interval { get; set; }
        public string timetable { get; set; }
    }
}
