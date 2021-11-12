using Business.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla_Api.Controllers
{
    [Route("api/[controller]")]
    public class HotelAmenityController : Controller
    {
        private readonly IHotelAmenityRepository _hotelAmenityRepository;
        public HotelAmenityController(IHotelAmenityRepository hotelAmenityRepository)
        {
            _hotelAmenityRepository = hotelAmenityRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHotelAmenities()
        {
            
            var allAmenities = await _hotelAmenityRepository.GetAllHotelAmenities();
            return Ok(allAmenities);
        }
    }
}
