using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Test.Models
{
    public class Media
    {
        public int id { get; set; }
        public string name { get; set; }
        public Nullable<int> media_type_id { get; set; }
        public string media_type { get; set; }
        public Nullable<int> expose_timing { get; set; }
        public string media_url { get; set; }
        public Nullable<System.DateTime> start_date { get; set; }
        public Nullable<System.DateTime> end_date { get; set; }
        public Nullable<int> insertions_limit { get; set; }
        public Nullable<bool> expose_at_all { get; set; }
        public string expose_in_groups { get; set; }
        public string expose_in { get; set; }
        public bool is_active { get; set; }
        public string created_by { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<System.DateTime> last_modified_date { get; set; }
    }
}
