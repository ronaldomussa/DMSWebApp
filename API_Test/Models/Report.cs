using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_DMS.Models
{
    public class Report
    {
        public int id { get; set; }
        public Nullable<int> media_id { get; set; }
        public string media_name { get; set; }
        public Nullable<int> display_id { get; set; }
        public string display_name { get; set; }
        public string display_token { get; set; }
        public string display_location { get; set; }
        public string display_orientation { get; set; }
        public string display_size { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
    }
}
