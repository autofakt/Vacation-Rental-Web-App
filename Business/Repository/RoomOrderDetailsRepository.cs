using AutoMapper;
using Business.Repository.IRepository;
using Common;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class RoomOrderDetailsRepository : IRoomOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public RoomOrderDetailsRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<RoomOrderDetailsDTO> Create(RoomOrderDetailsDTO details)
        {
            try
            {
                details.CheckInDate = details.CheckInDate.Date;  //gets rid of the time portion since its not needed.
                details.CheckOutDate = details.CheckOutDate.Date;
                var roomOrder = _mapper.Map<RoomOrderDetailsDTO, RoomOrderDetails>(details);
                roomOrder.Status = SD.Status_Pending;
                var result = await _db.RoomOrderDetails.AddAsync(roomOrder);
                await _db.SaveChangesAsync();
                return _mapper.Map<RoomOrderDetails, RoomOrderDetailsDTO>(result.Entity);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task<IEnumerable<RoomOrderDetailsDTO>> GetAllOrderRoomDetails()
        {
            try
            {
                //eager loading -> foreign key related data is loaded from the database as part of the initial query 
                IEnumerable<RoomOrderDetailsDTO> roomOrders =
                    _mapper.Map<IEnumerable<RoomOrderDetails>, IEnumerable<RoomOrderDetailsDTO>>(_db.RoomOrderDetails.Include(u => u.HotelRoom));
                return roomOrders;
            }catch(Exception e)
            {
                return null;
            }
        }

        public async Task<RoomOrderDetailsDTO> GetOrderRoomDetail(int roomOrderId)
        {
            try
            {
                //eager loading -> queries the first roomOrder that matches roomOrderID and then eager loads the hotel room followed by the images.
                RoomOrderDetails roomOrder = 
                    await _db.RoomOrderDetails.Include(u => u.HotelRoom).ThenInclude(u=>u.HotelRoomImages).FirstOrDefaultAsync(u=>u.Id==roomOrderId);
                RoomOrderDetailsDTO roomOrderDTO = _mapper.Map<RoomOrderDetails, RoomOrderDetailsDTO>(roomOrder);

                //No total days in roomOrderDTO or Model or Db. Manually calculating and inputting.
                roomOrderDTO.HotelRoomDTO.TotalDays = roomOrderDTO.CheckOutDate.Subtract(roomOrderDTO.CheckInDate).Days;
                return roomOrderDTO;

            }catch(Exception e)
            {
                return null;
            }
        }

        public async Task<bool> IsRoomBooked(int RoomId, DateTime checkInDate, DateTime checkOutDate)
        {
            //confusing
            var status = false;
            var existingBooking = await _db.RoomOrderDetails.Where(x => x.RoomId == RoomId && x.IsPaymentSuccessful &&
            //check if checkin date that user wants does not fall in between any dates for room that is booked
            (checkInDate < x.CheckOutDate && checkInDate.Date > x.CheckInDate
            //check if checkout date that user wnats does not fall in between any dates for rom that is booked
            || checkOutDate.Date > x.CheckInDate.Date && checkInDate.Date < x.CheckInDate.Date
            )).FirstOrDefaultAsync();

            if(existingBooking != null)
            {
                status = true;
            }
            return status;

            //RoomOrderDetails roomOrder = await _db.RoomOrderDetails.FirstOrDefaultAsync(x => x.RoomId == RoomId);
            //if (roomOrder != null)
            //{
            //    var resultCheckIn = DateTime.Compare(checkInDate, roomOrder.CheckInDate);
            //    var resultCheckOut = DateTime.Compare(checkOutDate, roomOrder.CheckOutDate);
            //    //earlier than
            //    if (resultCheckIn < 0 && resultCheckOut < 0)
            //        return true;
            //    //later than
            //    else if (resultCheckIn > 0 && resultCheckOut > 0)
            //        return true;
            //    else
            //        return false;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public Task<RoomOrderDetailsDTO> MarkPaymentAsSuccessful(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateOrderStatus(int roomOrderId, string status)
        {
            throw new NotImplementedException();
        }
    }
}
