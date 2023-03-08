using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models
{
    public class CityDto:CreateCityDto
    {
        public int Id { get; set; }
        public List<HotelDto>? Hotels { get; set; }
    }

    public class CreateCityDto
    {
        [Required]
        [Display(Name = "Name")]
        [StringLength(maximumLength: 100, ErrorMessage = "Too long for city name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Name")]
        [StringLength(maximumLength: 100, ErrorMessage = "Too long for state name")]
        public string State { get; set; }
    }
}
