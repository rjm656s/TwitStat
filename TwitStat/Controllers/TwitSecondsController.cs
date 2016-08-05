using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TwitStat.Models;

namespace TwitStat.Controllers
{
    public class TwitSecondsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Accepts a DateTime and bools containg iformation needed for stat gathering. If no TwitSecond object exists for that second, 
        //create one with a count value of one. Otherwise, retrieve existing object and increment the counter by one. Increment stat counters by one if true.
        public void IncrementCounter(DateTime time, bool hasHashtags, bool hasUrls, bool hasEmojis, bool hasPhotoUrls, int tweetLength)
        {
            TwitSeconds timeCounter = db.TwitSeconds.SingleOrDefault(s => s.TwitSecondTime == time);
            if (timeCounter == null)
            {
                TwitSeconds newCounter = new TwitSeconds();
                newCounter.TwitSecondTime = time;
                newCounter.Count = 1;
                if (hasHashtags) { newCounter.HashtagCount = 1; }
                if (hasUrls) { newCounter.UrlCount = 1; }
                if (hasEmojis) { newCounter.EmojiCount = 1; }
                if (hasPhotoUrls) { newCounter.PhotoCount = 1; }
                newCounter.Characters = tweetLength;
                Create(newCounter);
            }
            else
            {
                timeCounter.Count++;
                if (hasHashtags) { timeCounter.HashtagCount++; }
                if (hasUrls) { timeCounter.UrlCount++; }
                if (hasEmojis) { timeCounter.EmojiCount++; }
                if (hasPhotoUrls) { timeCounter.PhotoCount++; }
                timeCounter.Characters += tweetLength;
                Edit(timeCounter);
            }

        }
        
        // Pass totals and percentages to the index view
        public ActionResult Index()
        {
            int CountAverage = (int)db.TwitSeconds.Select(x => x.Count).Average();
            ViewBag.CountAverage = CountAverage;

            int CountTotal = db.TwitSeconds.Select(x => x.Count).Sum();
            ViewBag.CountTotal = CountTotal;

            double URLCount = db.TwitSeconds.Select(x => x.UrlCount).Sum();
            double URLPercent = (URLCount / CountTotal) * 100;
            ViewBag.URLPercent = Math.Round(URLPercent, 2);

            double HashtagCount = db.TwitSeconds.Select(x => x.HashtagCount).Sum();
            double HashtagPercent = (HashtagCount / CountTotal) * 100;
            ViewBag.HashtagPercent = Math.Round(HashtagPercent, 2);

            double PhotoURLCount = db.TwitSeconds.Select(x => x.PhotoCount).Sum();
            double PhotoURLPercent = (PhotoURLCount / CountTotal) * 100;
            ViewBag.PhotoURLPercent = Math.Round(PhotoURLPercent, 2);

            double EmojiCount = db.TwitSeconds.Select(x => x.EmojiCount).Sum();
            double EmojiPercent = (EmojiCount / CountTotal) * 100;
            ViewBag.EmojiPercent = Math.Round(EmojiPercent, 2);

            int TweetLength = db.TwitSeconds.Select(x => x.Characters).Sum();
            double LengthAverage = TweetLength / CountTotal;
            ViewBag.LengthAverage = Math.Round(LengthAverage, 0);

            return View(db.TwitSeconds.ToList());
        }

        // GET: TwitSeconds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TwitSeconds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TwitSecondID,TwitSecondTime,Count")] TwitSeconds twitSeconds)
        {
            if (ModelState.IsValid)
            {
                db.TwitSeconds.Add(twitSeconds);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(twitSeconds);
        }

        // GET: TwitSeconds/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TwitSeconds twitSeconds = db.TwitSeconds.Find(id);
            if (twitSeconds == null)
            {
                return HttpNotFound();
            }
            return View(twitSeconds);
        }

        // POST: TwitSeconds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TwitSecondID,TwitSecondTime,Count")] TwitSeconds twitSeconds)
        {
            if (ModelState.IsValid)
            {
                db.Entry(twitSeconds).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(twitSeconds);
        }

        // GET: TwitSeconds/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TwitSeconds twitSeconds = db.TwitSeconds.Find(id);
            if (twitSeconds == null)
            {
                return HttpNotFound();
            }
            return View(twitSeconds);
        }

        // POST: TwitSeconds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TwitSeconds twitSeconds = db.TwitSeconds.Find(id);
            db.TwitSeconds.Remove(twitSeconds);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
