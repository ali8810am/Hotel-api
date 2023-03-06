using System.ComponentModel.DataAnnotations;
using HotelListing.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.Models
{
    public class HotelDto:CreateHotelDto
    {
        public CityDto City { get; set; }
        public int Id { get; set; }
    }

    public class CreateHotelDto
    {
        [Required]
        [StringLength(maximumLength:150,ErrorMessage = "Too long for hotel name")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 1000, ErrorMessage = "Too long for description")]
        public string? Description { get; set; }
        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "Too long for hotel address")]
        public string Address { get; set; }
        [Required]
        [Range(1,5)]
        public double Rating { get; set; }
        [Required]
        public int CityId { get; set; }
    }
}
