using HotelListing.Models;

namespace HotelListing.Services
{
    public interface IAuthManager
    {
        Task<string> CreateToken();
        Task<bool> ValidateUser(LoginDto user);
    }
}
