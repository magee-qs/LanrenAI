using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Infrastructure.Middleware
{
    public class LogMesasge
    {
        public string ip { get; set; }

        public string host { get; set; }

        public string url { get; set; }

        public string method { get; set; }

        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }

        public long costTime { get; set; }

        public int status { get; set; }

        public string parameter { get; set; }

      

        public string browser { get; set; }

        public string webId { get; set; }

        public string userId { get; set; }

        public string userName { get; set; }

        public string userAccount { get; set; }


        public string request { get; set; }

        public string response { get; set; }

        public string message { get; set; }

        public string trace { get; set; }

        public string source { get; set; }

        public string controller { get; set; }

        public int result { get; set; }




    }
}
