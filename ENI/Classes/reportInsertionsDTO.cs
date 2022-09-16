using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENI.Classes
{
    public class reportInsertionsDTO
    {
        public int id { get; set; }
        public Nullable<int> media_id { get; set; }
        public string media_name { get; set; }
        public Nullable<int> display_id { get; set; }
        public string display_token { get; set; }
        public string display_name { get; set; }
        public string display_location { get; set; }
        public string display_orientation { get; set; }
        public string display_size { get; set; }
        public List<report_insertions> insertions { get; set; }
        public Nullable<System.DateTime> first_created_date { get; set; }
        public Nullable<System.DateTime> last_update_date { get; set; }
    }

    public class reportInsertionItemDTO
    {
        public Nullable<int> insertions_counted { get; set; }
        public Nullable<System.DateTime> period_start_date { get; set; }
        public Nullable<System.DateTime> period_end_date { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
    }
}