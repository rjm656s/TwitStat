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
    public class EmojisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Iterate through chrafters in the tweet, looking for charachters that match the emoji dictionary
        public bool ProcessEmojis(Dictionary<string, string> emojiDict, string tweetText)
        {
            bool hasEmojis = false;
            char[] tweetChars = tweetText.ToCharArray();
            
            for (int i=0; i<tweetText.Length; i++)
            {
                int unicode;

                //Look for surrogate pairs and convert for evaluation
                if (char.IsHighSurrogate(tweetText, i))
                {
                    unicode = char.ConvertToUtf32(tweetText, i);
                }
                else
                {
                    unicode = Convert.ToInt32(tweetChars[i]);
                }
                string hex = String.Format("{0:X}", unicode);

                if (emojiDict.ContainsKey(hex))
                {
                    hasEmojis = true;
                    string emojiName = emojiDict[hex];
                    Emojis emojiCounter = db.Emojis.SingleOrDefault(s => s.EmojiName == emojiName);
                    if (emojiCounter == null)
                    {
                        Emojis newCounter = new Emojis();
                        newCounter.EmojiName = emojiName;
                        newCounter.EmojiUnicode = hex;
                        newCounter.Count = 1;
                        Create(newCounter);
                    }
                    else
                    {
                        emojiCounter.Count++;
                        Edit(emojiCounter);
                    }

                }
            }


            return hasEmojis;
        }
        
        // GET: Emojis
        public ActionResult Index()
        {
            var OrderedEmojiList = db.Emojis.OrderByDescending(w => w.Count);
            return View(OrderedEmojiList.Take(20));
        }

        // GET: Emojis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Emojis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmojiName,EmojiUnicode,Count")] Emojis emojis)
        {
            if (ModelState.IsValid)
            {
                db.Emojis.Add(emojis);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emojis);
        }

        // GET: Emojis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emojis emojis = db.Emojis.Find(id);
            if (emojis == null)
            {
                return HttpNotFound();
            }
            return View(emojis);
        }

        // POST: Emojis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmojiName,EmojiUnicode,Count")] Emojis emojis)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emojis).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emojis);
        }

        // GET: Emojis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emojis emojis = db.Emojis.Find(id);
            if (emojis == null)
            {
                return HttpNotFound();
            }
            return View(emojis);
        }

        // POST: Emojis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Emojis emojis = db.Emojis.Find(id);
            db.Emojis.Remove(emojis);
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
