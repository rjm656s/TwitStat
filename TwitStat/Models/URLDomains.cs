using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitStat.Models
{
    public class URLDomains
    {
        public long id { get; set; }
        public string Domain { get; set; }
        public int Count { get; set; }
    }
}