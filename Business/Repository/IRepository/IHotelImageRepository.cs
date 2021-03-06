using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Business.Repository.IRepository
{
    public interface IHotelImageRepository
    {
        public Task<int> CreateHotelRoomImage(HotelRoomImageDTO image);
        public Task<int> DeleteHotelRoomImageByImageId(int imageId);
        public Task<int> DeleteHotelRoomImageByRoomId(int roomId);

        //added this
        public Task<int> DeleteHotelRoomByImageUrl(string roomImageUrl);

        public Task<IEnumerable<HotelRoomImageDTO>> GetHotelRoomImages(int roomId);
    }
}
