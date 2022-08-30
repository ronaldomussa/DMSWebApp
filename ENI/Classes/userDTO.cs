using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENI.Classes
{
    public class userDTO
    {
        public int id { get; set; }
        public Nullable<int> user_role_id { get; set; }
        public string name { get; set; }
        public string user_picture_url { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string token { get; set; }
        public Nullable<System.DateTime> token_expiration_date { get; set; }
        public string created_by { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<System.DateTime> last_modified_date { get; set; }
        public Nullable<bool> is_active { get; set; }
        public Nullable<bool> is_super_user { get; set; }

    }
}