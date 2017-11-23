using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Movies.Models;

namespace Movies.Controllers
{
    public class MoviesController : Controller
    {
        private MoviesEntities db = new MoviesEntities();

        // GET: Movies
        public ActionResult Index()
        {
            return View(db.Movies.ToList().OrderBy(m => m.ReleaseDate));
        }

        // POST: Movies (Filter by Title)
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            string title = form["txtTitle"];

            return View(db.Movies.Where(t => t.Title.Contains(title)).OrderBy(m => m.ReleaseDate));
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,ReleaseDate,Director")] Movie movie)
        {
            // Validate the movie
            if (!MovieValidate(movie))
            {
                ModelState.AddModelError("Title", "There is another movie with the same title and director");
            }

            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,ReleaseDate,Director")] Movie movie)
        {
            // Validate the movie
            if (!MovieValidate(movie))
            {
                ModelState.AddModelError("Title", "There is another movie with the same title and director");
            }

            if (ModelState.IsValid)
            {              
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
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
        
        // Function to validate de title and director of the movie
        private bool MovieValidate(Movie movie)
        {
            // Count movies with the same title and director and different Id
            int movies = db.Movies.Where(t => t.Title == movie.Title)
                                  .Where(d => d.Director == movie.Director)
                                  .Where(i => i.Id != movie.Id)
                                  .Count();
            // Return true if there is no duplicate movie
            return movies == 0;
        }
    }
}
