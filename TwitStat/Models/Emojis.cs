using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitStat.Models
{
    public class Emojis
    {
        public int ID { get; set; }
        public string EmojiName { get; set; }
        public string EmojiUnicode { get; set; }
        public int Count { get; set; }
    }
}