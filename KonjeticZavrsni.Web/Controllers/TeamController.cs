using KonjeticZavrsni.DAL;
using KonjeticZavrsni.Model;
using KonjeticZavrsni.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace KonjeticZavrsni.Web.Controllers
{
    public class TeamController : Controller
    {

        private DriverManagerDbContext _dbContext;

        public TeamController(DriverManagerDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var teamsQuery = this._dbContext.Teams.AsQueryable();

            var model = teamsQuery.ToList();


            return View("Index", model);
        }

        public IActionResult Details(int? id = null)
        {
            var team = this._dbContext.Teams
                .Where(p => p.Id == id)
                .FirstOrDefault();

            var drivers = this._dbContext.Drivers.Where(p => p.TeamId== team.Id).ToList();
            ViewBag.Drivers = drivers;

            /* var citiesQuery = this._dbContext.Cities.ToList();
            ViewBag.Cities = citiesQuery;*/

            return View(team);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Team model)
        {
            if (ModelState.IsValid)
            {
                this._dbContext.Teams.Add(model);
                this._dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }

        [ActionName(nameof(Edit))]
        public IActionResult Edit(int id)
        {
            var model = this._dbContext.Teams.FirstOrDefault(d => d.Id == id);
            return View(model);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            var team = this._dbContext.Teams.Single(d => d.Id == id);
            var ok = await this.TryUpdateModelAsync(team);

            if (ok && this.ModelState.IsValid)
            {
                this._dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }


        [HttpPost]
        public IActionResult IndexAjax(TeamFilterModel filter)
        {
            filter ??= new TeamFilterModel();
            var teamQuery = this._dbContext.Teams.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                teamQuery = teamQuery.Where(p => p.Name.ToLower().Contains(filter.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Low))
                teamQuery = teamQuery.Where(p => p.Budget >= int.Parse(filter.Low + "000000"));

            if (!string.IsNullOrWhiteSpace(filter.High))
                teamQuery = teamQuery.Where(p => p.Budget <= int.Parse(filter.High + "000000"));

            
            var model = teamQuery.ToList();
            return PartialView("_IndexTable", model);
        }


    }
}
