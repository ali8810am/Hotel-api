﻿using AutoMapper;
using HotelListing.Constants;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Marvin.Cache.Headers;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public,MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCities([FromQuery] RequestParameters? parameters)
        {
            var cities = await _unitOfWork.Cities.GetAll(parameters);
                var result = _mapper.Map<IList<CityDto>>(cities);
                if (result == null)
                    return NotFound();
            return Ok(result);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCity(int id)
        {
           
                var city = await _unitOfWork.Cities.Get(c=>c.Id==id,new List<string>{ "Hotels" });
                var result = _mapper.Map<CityDto>(city);
                if (result==null)
                    return NotFound();
                return Ok(result);
           
        }


        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityDto cityDto)
        {
            _logger.LogInformation($"registration attemped for {cityDto.Name}");
            if (!ModelState.IsValid)
                return BadRequest();
           
                var city = _mapper.Map<City>(cityDto);
                await _unitOfWork.Cities.Add(city);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetHotel", new { id = city.Id }, city);
            
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCity(int id, [FromBody] UpdateCityDto cityDto)
        {
            _logger.LogInformation($"registration attemped for {cityDto.Name}");
            if (!ModelState.IsValid || cityDto.Id != id)
                return BadRequest();
            
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

        [HttpDelete("{id:int}")]
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
