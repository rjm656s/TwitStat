using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwitStat.Models
{
    public class Hashtags
    {
        [Key]
        public int id { get; set; }
        public string HashtagText { get; set; }
        public int Count { get; set; }
    }
}