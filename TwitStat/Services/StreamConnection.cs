using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tweetinvi;
using Tweetinvi.Models;
using TwitStat.Models;
using TwitStat.Controllers;
using Tweetinvi.Models.Entities;
using TwitStat.Services;
using System.Threading.Tasks;

namespace TwitStat
{
    public class StreamConnection
    {
        //Initializes the twitter stream using the Tweetinvi library. Consumes events and passes releven data to the processor method.
        //If the stream terminates, waits one second and attempts reconnect.
        public StreamConnection(Dictionary<string, string> emojiDict)
        {

            string consumerKey = "oIX2HCrtjl9SiT06gCciOQTfk";
            string consumerSecret = "";
            string accessToken = "760532886084653056-c4IVxOvwx0NuelgfWl4C7qhocVMI52L";
            string accessTokenSecret = "";
            Auth.ApplicationCredentials = new TwitterCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);

            while (true)
            {
                var stream = Stream.CreateSampleStream();
                
                
                stream.TweetReceived += (sender, args) =>
                {
                    ProcessTweetAsync(args.Tweet.Text, args.Tweet.CreatedAt, args.Tweet.Hashtags, args.Tweet.Urls, args.Tweet.Media, emojiDict);
                    
                };
                stream.StartStream();
                System.Threading.Thread.Sleep(1000);
            }
           
        }
        //Accept data from tweets. Call stat gathering handler methods.
        public static async Task<int> ProcessTweetAsync(String tweetText, DateTime createdAt, List<IHashtagEntity> hashtags, List<IUrlEntity> urls, List<IMediaEntity> media, Dictionary<string, string> emojiDict)
        {
            var hashtagController = new HashtagsController();
            var urlDomainController = new URLDomainsController();
            var twitSecondsController = new TwitSecondsController();
            var emojisController = new EmojisController();
            bool hasHashtags = false;
            bool hasUrls = false;
            bool hasEmojis = false;
            bool hasPhotoUrls = false;
            bool[] urlReturnArray = new bool[2];
            int tweetLength = tweetText.Length;

            hasHashtags = hashtagController.IncrementCounter(hashtags);

            urlReturnArray = urlDomainController.ProcessURLs(urls, media);
            hasUrls = urlReturnArray[0];
            hasPhotoUrls = urlReturnArray[1];
            
            hasEmojis = emojisController.ProcessEmojis(emojiDict, tweetText);
            twitSecondsController.IncrementCounter(createdAt, hasHashtags, hasUrls, hasEmojis, hasPhotoUrls, tweetLength);
            return 42;
        }
    }
}