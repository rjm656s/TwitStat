using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwitStat.Models
{
    public class TwitSeconds
    {
        [Key]
        public long TwitSecondID { get; set; }
        public DateTime TwitSecondTime { get; set; }
        public int Count { get; set; }
        public int UrlCount { get; set; }
        public int PhotoCount { get; set; }
        public int HashtagCount { get; set; }
        public int EmojiCount { get; set; }
        public int Characters { get; set; }
    }
}