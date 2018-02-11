using GameCatalog.Data.Context;
using GameCatalog.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameCatalog.Controllers
{
    public class HomeController : Controller
    {
        private GameCatalogDbContext db = new GameCatalogDbContext();
        public ActionResult Index()
        {
            IQueryable<Game> games = db.Games.AsQueryable();
            games = games.OrderBy(x => x.Rating.RatingValue);
            List<Game> list = games.ToList();
            if (list.Count != 0)
            {
                for (int i = 1; i <= list.Count; i++)
                {
                    switch (i)
                    {
                        case 1:
                            ViewBag.Link1 = list[i-1].TrailerLink;
                            break;
                        case 2:
                            ViewBag.Link2 = list[i-1].TrailerLink;
                            break;
                        case 3:
                            ViewBag.Link3 = list[i-1].TrailerLink;
                            break;
                        case 4:
                            ViewBag.Link4 = list[i-1].TrailerLink;
                            break;
                        case 5:
                            ViewBag.Link5 = list[i-1].TrailerLink;
                            break;
                        case 6:
                            ViewBag.Link6 = list[i-1].TrailerLink;
                            break;
                        case 7:
                            ViewBag.Link7 = list[i-1].TrailerLink;
                            break;
                        case 8:
                            ViewBag.Link8 = list[i-1].TrailerLink;
                            break;
                        case 9:
                            ViewBag.Link9 = list[i-1].TrailerLink;
                            break;

                    }
                }
                
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}