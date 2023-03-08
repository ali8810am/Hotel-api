using AutoMapper;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CityController(ILogger<CityController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            try
            {
                var cities = await _unitOfWork.Cities.GetAll();
                var result = _mapper.Map<IList<CityDto>>(cities);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"Something went wrong in {nameof(GetCities)}");
                return StatusCode(500, "Internal server error please try again later");
            }
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCity(int id)
        {
            try
            {
                var city = await _unitOfWork.Cities.Get(c=>c.Id==id);
                var result = _mapper.Map<CityDto>(city);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(GetCity)}");
                return StatusCode(500, "Internal server error please try again later");
            }
        }
    }
}
