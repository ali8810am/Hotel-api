using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models
{
    public class LoginDto
    {

        [Required]
        [StringLength(maximumLength: 15, MinimumLength = 5)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
     
    }
    public class UserDto:LoginDto
    {
        [Required]
        [StringLength(maximumLength:50,MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public ICollection<string>? Roles { get; set; }
    }
}
