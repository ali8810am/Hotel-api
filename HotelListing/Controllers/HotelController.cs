using AutoMapper;
using HotelListing.Constants;
using HotelListing.Data;
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
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotels = await _unitOfWork.Hotels.GetAll();
                var result = _mapper.Map<IList<HotelDto>>(hotels);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(GetHotels)}");
                return StatusCode(500, "Internal server error please try again later");
            }
        }


        [HttpGet("{id:int}",Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(c => c.Id == id);
                var result = _mapper.Map<HotelDto>(hotel);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(GetHotel)}");
                return StatusCode(500, "Internal server error please try again later");
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody]CreateHotelDto hotelDto)
        {
            _logger.LogInformation($"registration attemped for {hotelDto.Name}");
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDto);
                await _unitOfWork.Hotels.Add(hotel);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetHotel", new { id = hotel.Id },hotel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(GetHotel)}");
                return StatusCode(500, "Internal server error please try again later");
            }
        }


        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id,[FromBody]UpdateHotelDto hotelDto)
        {
            _logger.LogInformation($"registration attemped for {hotelDto.Name}");
            if (!ModelState.IsValid||hotelDto.Id!=id)
                return BadRequest();
            try
            {
                var hotel =await _unitOfWork.Hotels.Get(h => h.Id == hotelDto.Id);
                if (hotel==null)
                {
                    _logger.LogInformation($" {hotelDto.Name} not found");
                    return NotFound();
                }
                _mapper.Map(hotelDto, hotel);
                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete]
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
