using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using RandomMovieGenerator.Models;

namespace RandomMovieGenerator.Controllers
{
    
    public class HomeController : Controller
    {
        public ApplicationDbContext _context;
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        public static class WatchlistStorage
        {
            public static List<Movie> Movies { get; set; } = new List<Movie>();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string genre)
        {
            if(string.IsNullOrEmpty(genre))
            {
                ViewBag.Message = "Nema validen film";
            }
            var films = _context.Movies.Where(f=>f.Genre == genre).ToList();
            if (!films.Any())
            {
                return HttpNotFound($"No movies found in that genre");
            }
            var random = new Random();
            
            var randomFilm = films[random.Next(films.Count)];
            return View("PrikazhiFilm", randomFilm);
        }
        [Authorize]
        public ActionResult AddToWatchlist()
        {
            var userId = User.Identity.GetUserId();

            var movies = _context.Watchlists
                           .Where(w => w.UserId == userId)
                           .Select(w => w.Movie)
                           .ToList();

            return View(movies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToWatchlist(Movie movie)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["DeferredMovie"] = JsonConvert.SerializeObject(movie);
                TempData["DeferredAction"] = "watchlist";
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("PostLoginHandler", "Home") });
            }

            var userId = User.Identity.GetUserId();
            var exists = _context.Watchlists.Any(w => w.UserId == userId && w.MovieId == movie.Id);
            if (!exists)
            {
                _context.Watchlists.Add(new Watchlist { UserId = userId, MovieId = movie.Id });
                _context.SaveChanges();
            }

            return RedirectToAction("AddToWatchlist");
        }

        [Authorize]
        public ActionResult Watched()
        {
            var userId = User.Identity.GetUserId();

            var watchedMovies = _context.Watcheds
                .Where(w => w.UserId == userId)
                .Select(w => new WatchedMovieViewModel
                {
                    Id = w.Movie.Id,
                    Name = w.Movie.Name,
                    Genre = w.Movie.Genre,
                    ImageUrl = w.Movie.ImageUrl,
                    ReleaseYear = w.Movie.ReleaseYear,
                    Duration = w.Movie.Duration,
                    Director = w.Movie.Director,
                    Rating = w.Movie.Rating, 
                    YourRating = w.Rating    
                })
                .ToList();

            return View(watchedMovies);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Watched(Movie movie)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["DeferredMovie"] = JsonConvert.SerializeObject(movie);
                TempData["DeferredAction"] = "watched";
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("PostLoginHandler", "Home") });
            }

            var userId = User.Identity.GetUserId();
            var exists = _context.Watcheds.Any(w => w.UserId == userId && w.MovieId == movie.Id);
            if (!exists)
            {
                _context.Watcheds.Add(new Watched { UserId = userId, MovieId = movie.Id });
                _context.SaveChanges();
            }

            return RedirectToAction("Watched");
        }

        [HttpPost]
        [Authorize]
        public ActionResult SubmitRating(int movieId, int rating)
        {
            var userId = User.Identity.GetUserId();

            var watchedEntry = _context.Watcheds.FirstOrDefault(w => w.UserId == userId && w.MovieId == movieId);
            if (watchedEntry != null)
            {
                watchedEntry.Rating = rating;
                _context.SaveChanges();
            }

            return RedirectToAction("Watched");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]

        public ActionResult RemoveFromWatchlist(int movieId)
        {
            var userId = User.Identity.GetUserId();
            var item = _context.Watchlists.SingleOrDefault(w => w.UserId == userId && w.MovieId == movieId);

            if (item != null)
            {
                _context.Watchlists.Remove(item);
                _context.SaveChanges();
            }

            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromWatched(int movieId)
        {
            var userId = User.Identity.GetUserId();
            var item = _context.Watcheds.SingleOrDefault(w=>w.UserId == userId && w.MovieId == movieId);

            if(item != null)
            {
                _context.Watcheds.Remove(item);
                _context.SaveChanges();
            }
            return new HttpStatusCodeResult(200);
        }
        


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult MoveToWatched(int movieId)
        {
            var userId = User.Identity.GetUserId();

            var exists = _context.Watcheds.Any(w => w.UserId == userId && w.MovieId == movieId);
            if (!exists)
            {
                _context.Watcheds.Add(new Watched
                {
                    UserId = userId,
                    MovieId = movieId,
                    Rating = null 
                });
            }

            var watchlistItem = _context.Watchlists.SingleOrDefault(w => w.UserId == userId && w.MovieId == movieId);
            if (watchlistItem != null)
                _context.Watchlists.Remove(watchlistItem);

            _context.SaveChanges();
            return new HttpStatusCodeResult(200);
        }

        [Authorize]
        public ActionResult PostLoginHandler()
        {
            if (TempData["DeferredMovie"] != null && TempData["DeferredAction"] != null)
            {
                var movie = JsonConvert.DeserializeObject<Movie>((string)TempData["DeferredMovie"]);
                var userId = User.Identity.GetUserId();
                var action = TempData["DeferredAction"].ToString();

                if (action == "watchlist")
                {
                    var exists = _context.Watchlists.Any(w => w.UserId == userId && w.MovieId == movie.Id);
                    if (!exists)
                    {
                        _context.Watchlists.Add(new Watchlist { UserId = userId, MovieId = movie.Id });
                    }
                    _context.SaveChanges();
                    return RedirectToAction("AddToWatchlist");
                }
                else if (action == "watched")
                {
                    var exists = _context.Watcheds.Any(w => w.UserId == userId && w.MovieId == movie.Id);
                    if (!exists)
                    {
                        _context.Watcheds.Add(new Watched { UserId = userId, MovieId = movie.Id });
                    }
                    _context.SaveChanges();
                    return RedirectToAction("Watched");
                }
            }

            return RedirectToAction("Index");
        }


    }
}