using Business.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla_Api.Controllers
{
    [Route("api/[controller]")]
    public class HotelRoomController : Controller
    {
        private readonly IHotelRoomRepository _hotelRoomRepository;
        public HotelRoomController(IHotelRoomRepository hotelRoomRepository)
        {
            _hotelRoomRepository = hotelRoomRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetHotelRooms()
        {
            var allRooms = await _hotelRoomRepository.GetAllHotelRooms();
            return Ok(allRooms);
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetHotelRoom(int? roomId)
        {
            if(roomId == null)
            {
                return BadRequest(new ErrorModel()
                {
                    Title = "No such Id",
                    ErrorMessage = "Invalid Room Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            var hotelRoom = await _hotelRoomRepository.GetHotelRoom(roomId.Value);
            if (hotelRoom == null)
            {
                return BadRequest(new ErrorModel()
                {
                    Title = "Does not exist",
                    ErrorMessage = "Room cannot be found",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(hotelRoom);
        }
         
    }
}
