using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IHotelRoomRepository
    {
        public Task<HotelRoomDTO> CreateHotelRoom(HotelRoomDTO hotelRoomDTO);
        public Task<HotelRoomDTO> UpdateHotelRoom(int roomID, HotelRoomDTO hotelRoomDTO);
        public Task<HotelRoomDTO> GetHotelRoom(int roomID, HotelRoomDTO hotelRoomDTO);
        public Task<IEnumerable<HotelRoomDTO>> GetAllHotelRoom();
        public Task<HotelRoomDTO> IsSameNameRoomAlreadyPresent(string name);

    }
}
