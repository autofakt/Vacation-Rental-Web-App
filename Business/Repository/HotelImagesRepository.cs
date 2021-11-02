using AutoMapper;
using Business.Repository.IRepository;
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
    public class HotelImagesRepository: IHotelImageRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public HotelImagesRepository(ApplicationDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<int> CreateHotelRoomImage(HotelRoomImageDTO imageDTO)
        {
            var image = _mapper.Map<HotelRoomImageDTO, HotelRoomImage>(imageDTO);
            await _db.HotelRoomImages.AddAsync(image);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> DeleteHotelRoomImageByImageId(int imageId)
        {
            var imageDetails = await _db.HotelRoomImages.FindAsync(imageId);
            if (imageDetails != null)  //this section can work without the null check and 0 return...
            {
                _db.HotelRoomImages.Remove(imageDetails);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> DeleteHotelRoomImageByRoomId(int roomId)
        {
            var imageList = await _db.HotelRoomImages.Where(x=>x.RoomId == roomId).ToListAsync();
            foreach(var roomImageObject in imageList)
            {
                _db.HotelRoomImages.Remove(roomImageObject);
            }

            //Can also use this instead:
            // _db.HotelRoomImages.RemoveRange(imageList); 
            return await _db.SaveChangesAsync();
        }

        //I added this - different than tutorial
        public async Task<int> DeleteHotelRoomByImageUrl(string roomImageUrl)
        {
            var imageList = await _db.HotelRoomImages.Where(x => x.RoomImageUrl.Contains(roomImageUrl)).ToListAsync();
            foreach (var roomImageObject in imageList)
            {
                _db.HotelRoomImages.Remove(roomImageObject);
            }

            //Can also use this instead:
            // _db.HotelRoomImages.RemoveRange(imageList); 
            return await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<HotelRoomImageDTO>> GetHotelRoomImages(int roomId)
        {
            //my first implementation
            //var imageList = await _db.HotelRoomImages.Where(x => x.RoomId == roomId).ToListAsync();
            //List<HotelRoomImageDTO> imageListDTO = new List<HotelRoomImageDTO>();
            //foreach (var image in imageList)
            //{
            //    var tempConversion = _mapper.Map<HotelRoomImage, HotelRoomImageDTO>(image);
            //    imageListDTO.Add(tempConversion);
            //}
            //return imageListDTO;
            return _mapper.Map<IEnumerable<HotelRoomImage>,IEnumerable<HotelRoomImageDTO>>(
            await _db.HotelRoomImages.Where(x => x.RoomId == roomId).ToListAsync());
        }
    }
}
