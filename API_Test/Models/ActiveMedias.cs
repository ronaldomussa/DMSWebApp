using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Test.Models
{
    public class ActiveMedias { 
        public string token { get; set; }
        public int display_id { get; set; }
        public DateTime timestamp { get; set; }
        public int count { get; set; }
        public List<Media> medias { get; set; }
    }
}
