using EmojiSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitStat.Services
{
    public class EmojiHandler
    {
        //Iterates the (very helpful) EmojiSharp dictionary class, and creates a dictionary appropriate for matching unicode to emojis
        public Dictionary<string, string>  GetEmojiDictionary()
        {
            Dictionary<string, string> EmojiDict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, Emoji> entry in Emoji.All)
            {
                EmojiDict.Add(entry.Value.Unified, entry.Key);
            }
            return EmojiDict;

        }
    }
}