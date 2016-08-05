using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Tweetinvi.Models.Entities;
using TwitStat.Models;

namespace TwitStat.Controllers
{
    public class HashtagsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Accepts tweet text and iterates accross each hashtag found. If no hashtag object exists for that hashtag, create one with 
        //a count value of one. Otherwise, retrieve existing object and increment the counter by one
        public bool IncrementCounter(List<IHashtagEntity> hashtags)
        {
            bool hasHashtags = false;
            foreach (IHashtagEntity m in hashtags)
            {
                hasHashtags = true;
                Hashtags hashtagCounter = db.Hashtags.SingleOrDefault(s => s.HashtagText == m.Text);
                if (hashtagCounter == null)
                {
                    Hashtags newCounter = new Hashtags();
                    newCounter.HashtagText = m.Text;
                    newCounter.Count = 1;
                    Create(newCounter);
                }
                else
                {
                    hashtagCounter.Count++;
                    Edit(hashtagCounter);
                }
            }
            return hasHashtags;

            
        }
        
        // GET: Hashtags
        public ActionResult Index()
        {
            var OrderedHashtagList = db.Hashtags.OrderByDescending(w => w.Count);
            return View(OrderedHashtagList.Take(20));
        }

        // GET: Hashtags/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hashtags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,HashtagText,Count")] Hashtags hashtags)
        {
            if (ModelState.IsValid)
            {
                db.Hashtags.Add(hashtags);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hashtags);
        }

        // GET: Hashtags/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hashtags hashtags = db.Hashtags.Find(id);
            if (hashtags == null)
            {
                return HttpNotFound();
            }
            return View(hashtags);
        }

        // POST: Hashtags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,HashtagText,Count")] Hashtags hashtags)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hashtags).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hashtags);
        }

        // GET: Hashtags/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hashtags hashtags = db.Hashtags.Find(id);
            if (hashtags == null)
            {
                return HttpNotFound();
            }
            return View(hashtags);
        }

        // POST: Hashtags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hashtags hashtags = db.Hashtags.Find(id);
            db.Hashtags.Remove(hashtags);
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
