#TwitStat
TwitStat is a web application written using the ASP.NET MVC Framework in Visual Studio 2015. It comsumes the Twitter Sample Stream using the Twitter Streaming API, and collects various statistics about the stream.

#Features
TwitStat reports the following statistics about the stream it consumes:
* Total count of tweets processed.
* Average tweets per second, minute, and hour.
* Average lenghth (in characters) of tweets processed.
* Percentage of tweets that contain emojis, and list of the top emojis seen.
* Percentage of tweets that contain hashtags, and list of the top hashtags seen.
* Percentage of tweets that contain URLs, and list of the top domains seen.
* Percentage of tweets that contain a URL for a photo site.

#Setup
Install Visual Studio 2015, create a new project, and clone this repository. You must supply values for the consumerSecret and accessTokenSecret strings in StreamConnection.cs.

#Libraries
TwitStat uses the [Tweetinvi](https://github.com/linvi/tweetinvi) libarary to facilitate its connection with Twitter's APIs, and the [EmojiSharp](https://github.com/jmazouri/EmojiSharp) library to identify Emojis (EmojiSharp uses a template to consume emoji.json from the [emoji-data](https://github.com/iamcal/emoji-data) project).

