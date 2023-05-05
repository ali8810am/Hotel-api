using System.Text.RegularExpressions;
using AutoMapper;
using HotelListing.Constants;
using HotelListing.Data;
using HotelListing.Exceptions;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HotelController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HotelController(ILogger<HotelController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels([FromQuery] RequestParameters? parameters)
        {

            var hotels = await _unitOfWork.Hotels.GetAll(parameters);
            var result = _mapper.Map<IList<HotelDto>>(hotels);
            if (result == null)
                return NotFound();
            return Ok(result);

        }


        [HttpGet("{id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            var hotel = await _unitOfWork.Hotels.Get(c => c.Id == id);
            var result = _mapper.Map<HotelDto>(hotel);
            if (result == null)
                return NotFound();
            return Ok(result);

        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDto hotelDto)
        {
            _logger.LogInformation($"registration attemped for {hotelDto.Name}");
            if (!ModelState.IsValid)
                return BadRequest();
            if (Regex.IsMatch(hotelDto.Name, @"^\d"))
                throw new NameException("name starts with numbers", hotelDto.Name);

            var hotel = _mapper.Map<Hotel>(hotelDto);
            await _unitOfWork.Hotels.Add(hotel);
            await _unitOfWork.Save();
            return CreatedAtRoute("GetHotel", new { id = hotel.Id }, hotel);

        }


        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDto hotelDto)
        {
            _logger.LogInformation($"registration attemped for {hotelDto.Name}");
            if (!ModelState.IsValid || hotelDto.Id != id)
                return BadRequest();

            var hotel = await _unitOfWork.Hotels.Get(h => h.Id == hotelDto.Id);
            if (hotel == null)
            {
                _logger.LogInformation($" {hotelDto.Name} not found");
                return NotFound();
            }
            _mapper.Map(hotelDto, hotel);
            _unitOfWork.Hotels.Update(hotel);
            await _unitOfWork.Save();
            return NoContent();

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            await _unitOfWork.Hotels.Delete(id);
            await _unitOfWork.Save();
            return NoContent();
        }
    }

}
