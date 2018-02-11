using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GameCatalog.Data.Context;
using GameCatalog.Data.Entities;
using PagedList;
using System.Collections.Generic;

namespace GameCatalog.Controllers

{
    public class Game_Hellp
    {
        public int? page { get; set; }
        public string sortOrder { get; set; }
        public string search { get; set; }
        public string searchValue { get; set; }


    }

    public class GamesController : Controller
    {
        private GameCatalogDbContext db = new GameCatalogDbContext();
        
        private static string lastSearch;
        private  IQueryable<Game> Search(string search, string searchValue, IQueryable<Game> games)
        {
          
            if (!String.IsNullOrEmpty(search))
            {
                switch (searchValue)
                {
                    case "Genre":
                        lastSearch = "Genre";
                       return games.Where(x => x.Genre.Name.Contains(search));
                       
                    case "Rating":
                        lastSearch = "Rating";
                        return games.Where(x => x.Rating.RatingValue.Contains(search));
                        
                    case "Release Year":
                        lastSearch = "Release Year";
                        return games.Where(x => x.ReleaseYear.Contains(search));
                        
                    case "Name":
                        lastSearch = "Name";
                        return games.Where(x => x.Name.Contains(search));
                        


                }
            }
            return games.AsQueryable();

        }
        private void SearchDropDownList(string searchValue)
        {
            List<string> valueList = new List<string>() { "Name", "Rating", "Genre", "Release Year" };
            ViewBag.SearchValue = new SelectList(valueList, "", "", searchValue);
        }
        private IQueryable<Game> Sort(string sortOrder, string search, IQueryable<Game> games)
        {

            ViewBag.CurrentSortParm = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.GenreNameSortParm = sortOrder == "genreName_asc" ? "genreName_desc" : "genreName_asc";
            ViewBag.RatingSortParm = sortOrder == "rating_asc" ? "rating_desc" : "rating_asc";
            switch (sortOrder)
            {
                case "name_desc":
                    games = Search(search, lastSearch, games);
                    games = games.OrderByDescending(x => x.Name);
                    break;
                case "genreName_asc":
                    games = Search(search, lastSearch, games);
                    games = games.OrderBy(x => x.Genre.Name);
                    break;
                case "genreName_desc":
                    games = Search(search, lastSearch, games);
                    games = games.OrderByDescending(x => x.Genre.Name);
                    break;
                case "rating_asc":
                    games = Search(search, lastSearch, games);
                    games = games.OrderBy(x => x.Rating.RatingValue);
                    break;
                case "rating_desc":
                    games = Search(search, lastSearch, games);
                    games = games.OrderByDescending(x => x.Rating.RatingValue);
                    break;
                default:
                    games = Search(search, lastSearch, games);
                    games = games.OrderBy(x => x.Name);
                    break;
            }
            return games;
        }
        // GET: Games
        public ActionResult Index([Bind(Include = "page,sortOrder,search,searchValue")] Game_Hellp gameHellp)
        {
            IQueryable<Game> games = db.Games.AsQueryable();
            int pageNumber = gameHellp.page ?? 1;
            int pageSize = 5;
            SearchDropDownList(gameHellp.searchValue);
            ViewBag.Search = gameHellp.search;
            games = Search(gameHellp.search, gameHellp.searchValue, games);
            games = Sort(gameHellp.sortOrder, gameHellp.search, games);
            return View(games.ToPagedList(pageNumber, pageSize));
        }



        
        //public ActionResult Index(int? page,string sortOrder ,string search,string searchValue)
        //{

        //    int pageNumber = page ?? 1;
        //    int pageSize = 5;

        //    List<string> valueList = new List<string>(){"Name","Rating","Genre","Release Year" };
        //    ViewBag.SearchValue = new SelectList(valueList,"", "", searchValue);
        //    IQueryable<Game> games = db.Games.AsQueryable();

        //    ViewBag.Search = search;
        //   games=Search(search, searchValue, games);



        //    ViewBag.CurrentSortParm = sortOrder;
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewBag.GenreNameSortParm = sortOrder == "genreName_asc" ? "genreName_desc" : "genreName_asc";
        //    ViewBag.RatingSortParm = sortOrder == "rating_asc" ? "rating_desc" : "rating_asc";
        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            games = Search(search, lastSearch, games);
        //            games = games.OrderByDescending(x => x.Name);
        //            break;
        //        case "genreName_asc":
        //            games = Search(search, lastSearch, games);
        //            games = games.OrderBy(x => x.Genre.Name);
        //            break;
        //        case "genreName_desc":
        //            games = Search(search, lastSearch, games);
        //            games = games.OrderByDescending(x => x.Genre.Name);
        //            break;
        //        case "rating_asc":
        //            games = Search(search, lastSearch, games);
        //            games = games.OrderBy(x => x.Rating.RatingValue);
        //            break;
        //        case "rating_desc":
        //            games = Search(search, lastSearch, games);
        //            games = games.OrderByDescending(x => x.Rating.RatingValue);
        //            break;
        //        default:
        //            games = Search(search, lastSearch, games);
        //            games = games.OrderBy(x => x.Name);
        //            break;
        //    }
        //    return View(games.ToPagedList(pageNumber,pageSize));
        //}

        // GET: Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: Games/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name");
            ViewBag.RatingId = new SelectList(db.Ratings, "Id", "RatingValue");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name,ReleaseYear,GenreId,RatingId,TrailerLink")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Games.Add(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", game.GenreId);
            ViewBag.RatingId = new SelectList(db.Ratings, "Id", "RatingValue", game.RatingId);
            return View(game);
        }

        // GET: Games/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", game.GenreId);
            ViewBag.RatingId = new SelectList(db.Ratings, "Id", "RatingValue", game.RatingId);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,ReleaseYear,GenreId,RatingId,TrailerLink")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", game.GenreId);
            ViewBag.RatingId = new SelectList(db.Ratings, "Id", "RatingValue", game.RatingId);
            return View(game);
        }

        // GET: Games/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
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
