using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENI.Classes
{
    public class userSessionDTO
    {
        public int id { get; set; }
        public Nullable<int> user_role_id { get; set; }
        public string name { get; set; }
        public string user_picture_url { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public bool is_super_user { get; set; }

    }
}