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

        

        public async Task<RoomOrderDetailsDTO> MarkPaymentAsSuccessful(int id)
        {
            var data = await _db.RoomOrderDetails.FindAsync(id);
            if (data == null)
            {
                return null;
            }
            if (!data.IsPaymentSuccessful) //if not marked as successful then...
            {
                data.IsPaymentSuccessful = true;
                data.Status = SD.Status_Booked;
                var markPaymentSuccessful = _db.RoomOrderDetails.Update(data);
                await _db.SaveChangesAsync();
                return _mapper.Map<RoomOrderDetails, RoomOrderDetailsDTO>(markPaymentSuccessful.Entity);
            }
            return new RoomOrderDetailsDTO();
        }

        public Task<bool> UpdateOrderStatus(int roomOrderId, string status)
        {
            throw new NotImplementedException();
        }
    }
}
