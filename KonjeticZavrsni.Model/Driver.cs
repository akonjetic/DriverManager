using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonjeticZavrsni.Model
{
    public class Driver
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Neispravan unos imena.")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Neispravan unos prezimena.")]
        public string LastName { get; set; } = "";

        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; }

        public Team? Team { get; set; }

        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }

        public Country? Country { get; set; }   

        [ForeignKey(nameof(RaceTrack))]
        public int RaceTrackId { get; set; }    

        public RaceTrack? RaceTrack { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
