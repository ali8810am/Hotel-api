using AutoMapper;
using HotelListing.Constants;
using HotelListing.Data;
using HotelListing.Models;
using HotelListing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly SignInManager<ApiUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthManager _authManager;

        public AccountController(UserManager<ApiUser> userManager, SignInManager<ApiUser> signInManager, IMapper mapper, ILogger<AccountController> logger, IAuthManager authManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            _logger.LogInformation($"login attemped for {loginDto.UserName}");
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var result =
                    await _authManager.ValidateUser(loginDto);
                if (!result)
                    return Unauthorized(loginDto);

                return Accepted(new{Token=await _authManager.CreateToken()});

            } 
            catch (Exception e)
            {
                _logger.LogInformation($"registration attemped for {loginDto.UserName}");

                Console.WriteLine(e);
                throw;
            }
            
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            _logger.LogInformation($"registration attemped for {userDto.FirstName}  {userDto.LastName}");
            if (userDto.Roles==null)
                userDto.Roles.Add(UserRoles.Admin);
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var user = _mapper.Map<ApiUser>(userDto);
                var result = await _userManager.CreateAsync(user,userDto.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code,error.Description);
                    }
                    return BadRequest(ModelState);
                }

                await _userManager.AddToRolesAsync(user, userDto.Roles);
                return Accepted();

            }
            catch (Exception e)
            {
                _logger.LogInformation($"registration attemped for {userDto.FirstName}  {userDto.LastName}");
                throw;
            }

        }

    }
}
