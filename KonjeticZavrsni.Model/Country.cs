using System.ComponentModel.DataAnnotations;

namespace KonjeticZavrsni.Model
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";


        public virtual ICollection<Driver>? Drivers { get; set; }
    }
}