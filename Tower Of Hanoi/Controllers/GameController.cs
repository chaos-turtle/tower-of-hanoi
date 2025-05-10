using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;
using Tower_Of_Hanoi.Models;
using PagedList;

namespace Tower_Of_Hanoi.Controllers
{
    public class GameController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SaveScore(int moves, int disks, bool isPerfect)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var score = new Score
            {
                UserId = User.Identity.GetUserId(),
                Moves = moves,
                Disks = disks,
                IsPerfect = isPerfect,
                DateAchieved = DateTime.Now
            };

            db.Scores.Add(score);
            db.SaveChanges();

            return RedirectToAction("Leaderboard");
        }

        public ActionResult Leaderboard(string search, int page = 1, int pageSize = 5, string sortBy = "Moves", bool sortDesc = false, int? disks = null, bool? isPerfect = null)
        {
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDesc = sortDesc;

            var scores = db.Scores.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                scores = scores.Where(s => s.User.UserName.Contains(search)
                           || s.Moves.ToString().Contains(search)
                           || s.Disks.ToString().Contains(search)
                           || s.DateAchieved.ToString().Contains(search));
            }

            if (disks.HasValue)
            {
                scores = scores.Where(s => s.Disks == disks);
            }

            if (isPerfect.HasValue)
            {
                scores = scores.Where(s => s.IsPerfect == isPerfect);
            }

            switch (sortBy)
            {
                case "Moves":
                    scores = sortDesc ? scores.OrderByDescending(s => s.Moves) : scores.OrderBy(s => s.Moves);
                    break;
                case "DateAchieved":
                    scores = sortDesc ? scores.OrderByDescending(s => s.DateAchieved) : scores.OrderBy(s => s.DateAchieved);
                    break;
                case "Disks":
                    scores = sortDesc ? scores.OrderByDescending(s => s.Disks) : scores.OrderBy(s => s.Disks);
                    break;
                default:
                    scores = scores.OrderBy(s => s.Moves);
                    break;
            }

            var pagedScores = scores.ToPagedList(page, pageSize);
            return View(pagedScores);
        }

        [HttpPost]
        public ActionResult DeleteScore(int id)
        {
            var score = db.Scores.Find(id);
            if (score != null && score.UserId == User.Identity.GetUserId())
            {
                db.Scores.Remove(score);
                db.SaveChanges();
            }
            return RedirectToAction("Leaderboard");
        }

        [HttpPost]
        public ActionResult ClearScores()
        {
            var userId = User.Identity.GetUserId();
            var scores = db.Scores.Where(s => s.UserId == userId);
            db.Scores.RemoveRange(scores);
            db.SaveChanges();
            return RedirectToAction("Leaderboard");
        }
    }

}