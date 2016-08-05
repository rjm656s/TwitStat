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
    public class URLDomainsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Accepts tweet text and iterates accross each URL found. If no URLDomains object exists for that domain, create one with 
        //a count value of one. Otherwise, retrieve existing object and increment the counter by one
        public bool[] ProcessURLs(List<IUrlEntity> urls, List<IMediaEntity> media)
        {
            bool hasUrls = false;
            bool hasPhotoUrl = false;
            foreach (IUrlEntity u in urls)
            {
                hasUrls = true;
                Uri myUri = new Uri(u.ExpandedURL);
                URLDomains domainCounter = db.URLDomains.SingleOrDefault(s => s.Domain == myUri.Host);
                if (u.ExpandedURL.Contains("instagram.com") || u.ExpandedURL.Contains("twimg.com") || u.ExpandedURL.Contains("pic.twiiter.comn")) { hasPhotoUrl = true; }
                if (domainCounter == null)
                {
                    URLDomains newCounter = new URLDomains();
                    newCounter.Domain = myUri.Host;
                    newCounter.Count = 1;
                    Create(newCounter);
                }
                else
                {
                    domainCounter.Count++;
                    Edit(domainCounter);
                }
                
            }
            foreach (IMediaEntity m in media)
            {
                if (m.ExpandedURL.Contains("instagram.com") || m.ExpandedURL.Contains("twimg.com") || m.ExpandedURL.Contains("pic.twiiter.comn")) { hasPhotoUrl = true; }
            }

            bool[] returnArray = new bool[2];
            returnArray[0] = hasUrls;
            returnArray[1] = hasPhotoUrl;
            return returnArray;
        }

        // GET: URLDomains
        public ActionResult Index()
        {
            var OrderedDomainList = db.URLDomains.OrderByDescending(w => w.Count);
            return View(OrderedDomainList.Take(20));
        }

        // GET: URLDomains/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: URLDomains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Domain,Count")] URLDomains uRLDomains)
        {
            if (ModelState.IsValid)
            {
                db.URLDomains.Add(uRLDomains);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(uRLDomains);
        }

        // GET: URLDomains/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            URLDomains uRLDomains = db.URLDomains.Find(id);
            if (uRLDomains == null)
            {
                return HttpNotFound();
            }
            return View(uRLDomains);
        }

        // POST: URLDomains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Domain,Count")] URLDomains uRLDomains)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uRLDomains).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(uRLDomains);
        }

        // GET: URLDomains/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            URLDomains uRLDomains = db.URLDomains.Find(id);
            if (uRLDomains == null)
            {
                return HttpNotFound();
            }
            return View(uRLDomains);
        }

        // POST: URLDomains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            URLDomains uRLDomains = db.URLDomains.Find(id);
            db.URLDomains.Remove(uRLDomains);
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
