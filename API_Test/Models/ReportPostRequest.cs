using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_DMS.Models
{
    public class ReportPostRequest
    {
        public string display_token { get; set; }
        public int media_id { get; set; }
        public int insertions_counted { get; set; }
        public string period_start_date { get; set; }
        public string period_end_date { get; set; }
    }
}
