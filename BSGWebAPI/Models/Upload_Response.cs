using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSGWebAPI.Models
{
    public class Upload_Response
    {
        public string error { get; set; }
        public int status { get; set; }
        public string result { get; set; }
        public string rCode { get; set; }
        public string message { get; set; }
        public string timestamp { get; set; }
    }
}
