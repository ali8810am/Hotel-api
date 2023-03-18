using AutoMapper;
using HotelListing.Constants;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCity(int id)
        {
            try
            {
                var city = await _unitOfWork.Cities.Get(c=>c.Id==id,new List<string>{ "Hotels" });
                var result = _mapper.Map<CityDto>(city);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(GetCity)}");
                return StatusCode(500, "Internal server error please try again later");
            }
        }


        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityDto cityDto)
        {
            _logger.LogInformation($"registration attemped for {cityDto.Name}");
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var city = _mapper.Map<City>(cityDto);
                await _unitOfWork.Cities.Add(city);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetHotel", new { id = city.Id }, city);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(CreateCity)}");
                return StatusCode(500, "Internal server error please try again later");
            }
        }

        //[Authorize(Roles = UserRoles.Admin)]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCity(int id, [FromBody] UpdateCityDto cityDto)
        {
            _logger.LogInformation($"registration attemped for {cityDto.Name}");
            if (!ModelState.IsValid || cityDto.Id != id)
                return BadRequest();
            try
            {
                var city = await _unitOfWork.Cities.Get(h => h.Id == cityDto.Id);
                if (city == null)
                {
                    _logger.LogInformation($" {cityDto.Name} not found");
                    return NotFound();
                }
                _mapper.Map(cityDto, city);
                _unitOfWork.Cities.Update(city);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(CreateCity)}");
                return StatusCode(500, "Internal server error please try again later");
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCity(int id)
        {
            await _unitOfWork.Cities.Delete(id);
            await _unitOfWork.Save();
            return NoContent();
        }

    }
}
