using AutoMapper;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public class HotelRoomRepository : IHotelRoomRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public HotelRoomRepository(ApplicationDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<HotelRoomDTO> CreateHotelRoom(HotelRoomDTO hotelRoomDTO)
        {
            HotelRoom hotelRoom = _mapper.Map<HotelRoomDTO, HotelRoom>(hotelRoomDTO);
            hotelRoom.CreatedDate = DateTime.Now;
            hotelRoom.CreatedBy = "";
            var addedHotelRoom = await _db.HotelRooms.AddAsync(hotelRoom);
            await _db.SaveChangesAsync();
            return _mapper.Map<HotelRoom, HotelRoomDTO>(addedHotelRoom.Entity);
        }

        public async Task<IEnumerable<HotelRoomDTO>> GetAllHotelRooms(string checkInDate = null, string checkOutDate = null)
        {
            try
            {
                IEnumerable<HotelRoomDTO> hotelRoomDTOs =
                    _mapper.Map<IEnumerable<HotelRoom>, IEnumerable<HotelRoomDTO>>(_db.HotelRooms.Include(x => x.HotelRoomImages));
                if (!string.IsNullOrEmpty(checkInDate) && !string.IsNullOrEmpty(checkOutDate))
                {
                    foreach (HotelRoomDTO hotelRoom in hotelRoomDTOs)
                    {
                        hotelRoom.IsBooked = await IsRoomBooked(hotelRoom.Id, checkInDate, checkOutDate);
                    }
                }
                return hotelRoomDTOs;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task<HotelRoomDTO> GetHotelRoom(int roomID, string checkInDate = null, string checkOutDate = null)
        {
            try
            {
                HotelRoomDTO hotelRoom = _mapper.Map<HotelRoom,HotelRoomDTO>( await _db.HotelRooms.Include(x=> x.HotelRoomImages).FirstOrDefaultAsync(x => x.Id == roomID));
                
                //Another version of code that works without using Eager Loading in EF.
                //HotelRoomDTO hotelRoom = _mapper.Map<HotelRoom, HotelRoomDTO>(await _db.HotelRooms.FirstOrDefaultAsync(x => x.Id == roomID));
                //var hotelImages = _mapper.Map<IEnumerable<HotelRoomImage>, IEnumerable<HotelRoomImageDTO>>(await _db.HotelRoomImages.Where(x => x.RoomId == roomID).ToListAsync());
                //ICollection<HotelRoomImageDTO> imagesCollection = hotelImages.ToList();
                //hotelRoom.HotelRoomImages = imagesCollection;
                if(!string.IsNullOrEmpty(checkInDate) && !string.IsNullOrEmpty(checkOutDate))
                {
                    hotelRoom.IsBooked = await IsRoomBooked(roomID, checkInDate, checkOutDate);
                }
                return hotelRoom;
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public async Task<bool> IsRoomBooked(int RoomId, string checkInDateString, string checkOutDateString)
        {
            try
            {
                if(!string.IsNullOrEmpty(checkOutDateString)&& !string.IsNullOrEmpty(checkInDateString))
                {
                    DateTime checkInDate = DateTime.ParseExact(checkInDateString, "MM/dd/yyyy", null);
                    DateTime checkOutDate = DateTime.ParseExact(checkOutDateString, "MM/dd/yyyy", null);

                    //confusing
                    var existingBooking = await _db.RoomOrderDetails.Where(x => x.RoomId == RoomId && x.IsPaymentSuccessful &&
                    //check if checkin date that user wants does not fall in between any dates for room that is booked
                    ((checkInDate < x.CheckOutDate && checkInDate.Date >= x.CheckInDate)
                    //check if checkout date that user wnats does not fall in between any dates for rom that is booked
                    || (checkOutDate.Date > x.CheckInDate.Date && checkInDate.Date <= x.CheckInDate.Date)
                     )).FirstOrDefaultAsync();

                    if(existingBooking != null)
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            }catch(Exception e)
            {
                throw e;
            }
            
            

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

        public async Task<HotelRoomDTO> IsRoomUnique(string name, int roomId = 0)
        {
            try
            {
                if (roomId == 0) //create
                {
                    HotelRoomDTO hotelRoom = _mapper.Map<HotelRoom, HotelRoomDTO>(
                        await _db.HotelRooms.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()));

                    return hotelRoom;
                }
                else 
                {
                    HotelRoomDTO hotelRoom = _mapper.Map<HotelRoom, HotelRoomDTO>(
                        await _db.HotelRooms.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.Id!=roomId));
                    //checks to see that: there isnt already a room with that same name but different id = duplicate

                    return hotelRoom;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HotelRoomDTO> UpdateHotelRoom(int roomId, HotelRoomDTO hotelRoomDTO)
        {
            try
            {
                if (roomId == hotelRoomDTO.Id)
                {
                    HotelRoom roomDetails = await _db.HotelRooms.FindAsync(roomId);
                    HotelRoom room = _mapper.Map<HotelRoomDTO, HotelRoom>(hotelRoomDTO, roomDetails);
                    room.UpdatedBy = "";
                    room.UpdatedDate = DateTime.Now;
                    var updatedRoom = _db.HotelRooms.Update(room);
                    await _db.SaveChangesAsync();
                    return _mapper.Map<HotelRoom, HotelRoomDTO>(updatedRoom.Entity);
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

       public async Task<int> DeleteHotelRoom(int roomId)
        {
            var roomDetails = await _db.HotelRooms.FindAsync(roomId);
            if (roomDetails != null)
            {
                var allimages = await _db.HotelRoomImages.Where(x => x.RoomId == roomId).ToListAsync();
                

                _db.HotelRoomImages.RemoveRange(allimages);
                _db.HotelRooms.Remove(roomDetails);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }
    }
}
