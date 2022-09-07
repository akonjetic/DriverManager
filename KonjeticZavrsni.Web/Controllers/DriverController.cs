using KonjeticZavrsni.DAL;
using KonjeticZavrsni.Model;
using KonjeticZavrsni.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KonjeticZavrsni.Web.Controllers
{
    public class DriverController : Controller
    {

        private DriverManagerDbContext _dbContext;

        public DriverController(DriverManagerDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var driversQuery = this._dbContext.Drivers.Include(p => p.Country).Include(p => p.RaceTrack).Include(p => p.Team).AsQueryable();

            var model = driversQuery.ToList();


            return View("Index", model);
        }

        public IActionResult Details(int? id = null)
        {
            var driver = this._dbContext.Drivers
                .Include(p => p.Country)
                .Include(p => p.RaceTrack)
                .Include(p => p.Team)
                .Where(p => p.Id == id)
                .FirstOrDefault();

            return View(driver);
        }

        public IActionResult Create()
        {
            this.FillDropdownValues();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Driver model)
        {
            if (ModelState.IsValid)
            {
                this._dbContext.Drivers.Add(model);
                this._dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                this.FillDropdownValues();
                return View(model);
            }
        }

        [ActionName(nameof(Edit))]
        public IActionResult Edit(int id)
        {
            var model = this._dbContext.Drivers.FirstOrDefault(d => d.Id == id);
            this.FillDropdownValues();
            return View(model);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            var driver = this._dbContext.Drivers.Single(d => d.Id == id);
            var ok = await this.TryUpdateModelAsync(driver);

            if (ok && this.ModelState.IsValid)
            {
                this._dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            this.FillDropdownValues();
            return View();
        }

        [HttpPost]
        public IActionResult IndexAjax(DriverFilterModel filter)
        {
            filter ??= new DriverFilterModel();
            var driverQuery = this._dbContext.Drivers.Include(d => d.Country).Include(d => d.RaceTrack).Include(d => d.Team).AsEnumerable();

            if (!string.IsNullOrWhiteSpace(filter.FullName))
                driverQuery = driverQuery.Where(p => p.FullName.ToLower().Contains(filter.FullName.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Country))
                driverQuery = driverQuery.Where(p => p.CountryId != null && p.Country.Name.ToLower().Contains(filter.Country.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Team))
                driverQuery = driverQuery.Where(p => p.TeamId != null && p.Team.Name.ToLower().Contains(filter.Team.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.FavoriteTrack))
                driverQuery = driverQuery.Where(p => p.RaceTrackId != null && p.RaceTrack.Name.ToLower().Contains(filter.FavoriteTrack.ToLower()));


            var model = driverQuery.ToList();
            return PartialView("_IndexTable", model);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var model = this._dbContext.Drivers.First(a => a.Id == id);
            this._dbContext.Drivers.Remove(model);

            this._dbContext.SaveChanges();

            var driverQuery = this._dbContext.Drivers.Include(d => d.Country).Include(d => d.RaceTrack).Include(d => d.Team).AsEnumerable();
            var driver = driverQuery.ToList();

            return Ok();
        }



        private void FillDropdownValues()
        {
            var selectItems = new List<SelectListItem>();

            //Polje je opcionalno
            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            selectItems.Add(listItem);

            foreach (var category in this._dbContext.Countries)
            {
                listItem = new SelectListItem(category.Name, category.Id.ToString());
                selectItems.Add(listItem);
            }

            ViewBag.PossibleCountries = selectItems;

            var selectItems2 = new List<SelectListItem>();

            var listItem2 = new SelectListItem();
            listItem2.Text = "- odaberite -";
            listItem2.Value = "";
            selectItems2.Clear();
            selectItems2.Add(listItem2);

            foreach (var category in this._dbContext.Teams)
            {
                listItem2 = new SelectListItem(category.Name, category.Id.ToString());
                selectItems2.Add(listItem2);
            }

            ViewBag.PossibleTeams = selectItems2;

            var selectItems3 = new List<SelectListItem>();

            var listItem3 = new SelectListItem();
            listItem3.Text = "- odaberite -";
            listItem3.Value = "";
            selectItems3.Clear();
            selectItems3.Add(listItem3);

            foreach (var category in this._dbContext.RaceTracks)
            {
                listItem3 = new SelectListItem(category.Name, category.Id.ToString());
                selectItems3.Add(listItem3);
            }

            ViewBag.PossibleRaceTracks = selectItems3;
        }

    }
}
