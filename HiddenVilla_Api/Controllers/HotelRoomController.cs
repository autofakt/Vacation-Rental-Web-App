using Business.Repository.IRepository;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla_Api.Controllers
{
    [Route("api/[controller]")]
    public class HotelRoomController : Controller
    {
       
        private readonly IHotelRoomRepository _hotelRoomRepository;
        //using dependency injection to get access to hotelRoomRepo
        public HotelRoomController(IHotelRoomRepository hotelRoomRepository)
        {
            _hotelRoomRepository = hotelRoomRepository;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetHotelRooms(string checkInDate, string checkOutDate)
        {
            if (string.IsNullOrEmpty(checkInDate) || string.IsNullOrEmpty(checkOutDate))
            {
                return BadRequest(new ErrorModel()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "All parameters need to be supplied"
                });
            }
            
            //checks if check-in date is in not in MM/dd/yyyy format
            if(!DateTime.TryParseExact(checkInDate,"MM/dd/yyyy",CultureInfo.InvariantCulture,DateTimeStyles.None,out var dtCheckInDate))
            {
                return BadRequest(new ErrorModel()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid Check-in date format. Must be in MM/dd/yyyy"
                });
            }

            //checks if check-out date is in not in MM/dd/yyyy format
            if (!DateTime.TryParseExact(checkOutDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtCheckOutDate))
            {
                return BadRequest(new ErrorModel()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid Check-out date format. Must be in MM/dd/yyyy"
                });
            }

            //goes to the hotelRoomRepository in Business class library to get HotelRooms and then returns DTO versions.
            var allRooms = await _hotelRoomRepository.GetAllHotelRooms(checkInDate,checkOutDate);
            return Ok(allRooms);
        }

        //Called when user pressed booked button on rooms home page
        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetHotelRoom(int? roomId, string checkInDate, string checkOutDate)
        {
            if(roomId == null)
            {
                //made an errormodel class for error message.
                return BadRequest(new ErrorModel()
                {
                    Title = "No such Id",
                    ErrorMessage = "Invalid Room Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            if (string.IsNullOrEmpty(checkInDate) || string.IsNullOrEmpty(checkOutDate))
            {
                return BadRequest(new ErrorModel()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "All parameters need to be supplied"
                });
            }

            //error checks date format
            if (!DateTime.TryParseExact(checkInDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtCheckInDate))
            {
                return BadRequest(new ErrorModel()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid Check-in date format. Must be in MM/dd/yyyy"
                });
            }

            //error checks date format
            if (!DateTime.TryParseExact(checkOutDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtCheckOutDate))
            {
                return BadRequest(new ErrorModel()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid Check-out date format. Must be in MM/dd/yyyy"
                });
            }

            //makes call to hotelRoomRepository in Business class library
            var hotelRoom = await _hotelRoomRepository.GetHotelRoom(roomId.Value, checkInDate, checkOutDate);
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
