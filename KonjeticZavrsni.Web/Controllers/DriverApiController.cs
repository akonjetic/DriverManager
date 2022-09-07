using KonjeticZavrsni.DAL;
using KonjeticZavrsni.Model;
using Microsoft.AspNetCore.Mvc;

namespace KonjeticZavrsni.Web.Controllers
{
    [Route("/api/driver")]
    [ApiController]
    public class DriverApiController : Controller
    {
        private DriverManagerDbContext _dbContext;

        public DriverApiController(DriverManagerDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        public IActionResult Get()
        {
            var drivers = this._dbContext.Drivers
                .Select(p => new DriverDTO()
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    Country = new CountryDTO()
                    {
                        countryID = p.CountryId == null ? null : p.Country.Id,
                        Name = p.Country == null ? null : p.Country.Name
                    },
                    FavoriteTrack = new RaceTrackDTO()
                    {
                        trackID = p.RaceTrackId == null ? null : p.RaceTrack.Id,
                        Name = p.RaceTrack == null ? null : p.RaceTrack.Name
                    },
                    Team = new TeamDTO()
                    {
                        teamID = p.TeamId == null ? null : p.Team.Id,
                        Name = p.Team == null ? null : p.Team.Name,
                        Budget = p.Team.Budget
                    }

                }).AsQueryable();


            return Ok(drivers);
        }

        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var driver = this._dbContext.Drivers
                .Where(p => p.Id == id)
                .Select(p => new DriverDTO()
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    Country = new CountryDTO()
                    {
                        countryID = p.CountryId == null ? null : p.Country.Id,
                        Name = p.Country == null ? null : p.Country.Name
                    },
                    FavoriteTrack = new RaceTrackDTO()
                    {
                        trackID = p.RaceTrackId == null ? null : p.RaceTrack.Id,
                        Name = p.RaceTrack == null ? null : p.RaceTrack.Name
                    },
                    Team = new TeamDTO()
                    {
                        teamID = p.TeamId == null ? null : p.Team.Id,
                        Name = p.Team == null ? null : p.Team.Name,
                        Budget = p.Team.Budget
                    }

                }).FirstOrDefault();


            if (driver == null)
            {
                return NotFound();
            }

            return Ok(driver);
        }

        [Route("pretraga/{q}")]
        public IActionResult Get(string q)
        {
            var driver = this._dbContext.Drivers
                .Where(p => p.FirstName.Contains(q) || p.LastName.Contains(q))
                .Select(p => new DriverDTO()
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    Country = new CountryDTO()
                    {
                        countryID = p.CountryId == null ? null : p.Country.Id,
                        Name = p.Country == null ? null : p.Country.Name
                    },
                    FavoriteTrack = new RaceTrackDTO()
                    {
                        trackID = p.RaceTrackId == null ? null : p.RaceTrack.Id,
                        Name = p.RaceTrack == null ? null : p.RaceTrack.Name
                    },
                    Team = new TeamDTO()
                    {
                        teamID = p.TeamId == null ? null : p.Team.Id,
                        Name = p.Team == null ? null : p.Team.Name,
                        Budget = p.Team.Budget
                    }

                }).AsQueryable();


            if (driver == null)
            {
                return NotFound();
            }

            return Ok(driver);
        }

        [HttpPost]
        public IActionResult Post(DriverDTO driver)
        {
            string[] names = driver.FullName.Split(" ");

            if (string.IsNullOrWhiteSpace(names[0]))
            {
                return BadRequest();
            }

            var newCountry = this._dbContext.Countries.Where(t => t.Name == driver.Country.Name).FirstOrDefault();

            if(newCountry == null)
            {
                this._dbContext.Add(new Country()
                {
                    Name = driver.Country.Name
                });
            }
           

            var newTeam = this._dbContext.Teams.Where(t => t.Name == driver.Team.Name).FirstOrDefault();

            if(newTeam == null)
            {
                this._dbContext.Add(new Team()
                {
                    Name = driver.Team.Name,
                    Budget = driver.Team.Budget
                });
            }

            var newRaceTrack = this._dbContext.RaceTracks.Where(t => t.Name == driver.FavoriteTrack.Name).FirstOrDefault();
            
            if(newRaceTrack == null)
            {
                this._dbContext.Add(new RaceTrack()
                {
                    Name = driver.FavoriteTrack.Name
                });
            }
           

            this._dbContext.SaveChanges();

            var country = this._dbContext.Countries.Where(c => c.Name == driver.Country.Name).FirstOrDefault();
            var team = this._dbContext.Teams.Where(c => c.Name == driver.Team.Name).FirstOrDefault();
            var raceTrack = this._dbContext.RaceTracks.Where(c => c.Name == driver.FavoriteTrack.Name).FirstOrDefault();


            this._dbContext.Add(new Driver()
            {
                FirstName = names[0],
                LastName = names[1],
                Country = country,
                CountryId = country.Id,
                Team = team,
                TeamId = team.Id,
                RaceTrack = raceTrack,
                RaceTrackId = raceTrack.Id
            });

            this._dbContext.SaveChanges();

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put(int id, [FromBody] DriverDTO driver)
        {
            var driverDB = this._dbContext.Drivers.First(p => p.Id == id);
            string[] names = driver.FullName.Split(" ");
            var country = this._dbContext.Countries.Where(c => c.Name == driver.Country.Name).FirstOrDefault();
            var team = this._dbContext.Teams.Where(c => c.Name == driver.Team.Name).FirstOrDefault();
            var raceTrack = this._dbContext.RaceTracks.Where(c => c.Name == driver.FavoriteTrack.Name).FirstOrDefault();


            driverDB.FirstName = names[0];
            driverDB.LastName = names[1];
            driverDB.Country = country;
            driverDB.Team = team;
            driverDB.RaceTrack = raceTrack;

            this._dbContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var driver = this._dbContext.Drivers.First(p => p.Id == id);
            this._dbContext.Remove(driver);

            this._dbContext.SaveChanges();

            return Ok();
        }



    }

    public class DriverDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public CountryDTO Country { get; set; }

        public RaceTrackDTO FavoriteTrack { get; set; }

        public TeamDTO Team { get; set; }

    }

    public class CountryDTO
    {
        public int? countryID;
        public string Name { get; set; }

    }

    public class TeamDTO
    {
        public int? teamID;
        public string Name { get; set; }

        public int Budget { get; set; } 

    }

    public class RaceTrackDTO
    {
        public int? trackID;
        public string Name { get; set; }

    }
}

