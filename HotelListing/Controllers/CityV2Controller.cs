using AutoMapper;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/city")]
    [ApiController]
    public class CityV2Controller : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CityV2Controller(ILogger<CityController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCities([FromQuery] RequestParameters? parameters)
        {
            var cities = await _unitOfWork.Cities.GetAll(parameters);
            var result = _mapper.Map<IList<CityDto>>(cities);
            return Ok(result);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCity(int id)
        {

            var city = await _unitOfWork.Cities.Get(c => c.Id == id, new List<string> { "Hotels" });
            var result = _mapper.Map<CityDto>(city);
            return Ok(result);

        }

    }
}

