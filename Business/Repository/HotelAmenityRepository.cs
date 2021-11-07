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
    
    public class HotelAmenityRepository : IHotelAmenityRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public HotelAmenityRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<HotelAmenityDTO> CreateHotelAmenity(HotelAmenityDTO hotelAmenityDTO)
        {
            HotelAmenity hotelAmenity = _mapper.Map<HotelAmenityDTO, HotelAmenity>(hotelAmenityDTO);
            hotelAmenity.CreatedDate = DateTime.Now;
            hotelAmenity.CreatedBy = "";
            var addedHotelAmenity = await _db.HotelAmenity.AddAsync(hotelAmenity);
            await _db.SaveChangesAsync();
            return _mapper.Map<HotelAmenity, HotelAmenityDTO>(addedHotelAmenity.Entity);
        }

        public async Task<int> DeleteHotelAmenity(int amenityId)
        {
            var amenityDetails = await _db.HotelAmenity.FindAsync(amenityId);
            if (amenityDetails != null)
            {
                
                _db.HotelAmenity.Remove(amenityDetails);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<HotelAmenityDTO>> GetAllHotelAmenities()
        {
            try
            {
                IEnumerable<HotelAmenityDTO> hotelAmenityDTOs =
                    _mapper.Map<IEnumerable<HotelAmenity>, IEnumerable<HotelAmenityDTO>>(await _db.HotelAmenity.ToListAsync());
                return hotelAmenityDTOs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HotelAmenityDTO> GetHotelAmenity(int amenityID)
        {
            try
            {
                HotelAmenityDTO hotelAmenity = _mapper.Map<HotelAmenity, HotelAmenityDTO>(await _db.HotelAmenity.FirstOrDefaultAsync(x => x.Id == amenityID));

                //Another version of code that works without using Eager Loading in EF.
                //HotelRoomDTO hotelRoom = _mapper.Map<HotelRoom, HotelRoomDTO>(await _db.HotelRooms.FirstOrDefaultAsync(x => x.Id == roomID));
                //var hotelImages = _mapper.Map<IEnumerable<HotelRoomImage>, IEnumerable<HotelRoomImageDTO>>(await _db.HotelRoomImages.Where(x => x.RoomId == roomID).ToListAsync());
                //ICollection<HotelRoomImageDTO> imagesCollection = hotelImages.ToList();
                //hotelRoom.HotelRoomImages = imagesCollection;

                return hotelAmenity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HotelAmenityDTO> IsAmenityUnique(string name, int amenityId = 0)
        {
            try
            {
                if (amenityId == 0) //create
                {
                    HotelAmenityDTO hotelAmenity = _mapper.Map<HotelAmenity, HotelAmenityDTO>(
                        await _db.HotelAmenity.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()));

                    return hotelAmenity;
                }
                else
                {
                    HotelAmenityDTO hotelAmenity = _mapper.Map<HotelAmenity, HotelAmenityDTO>(
                        await _db.HotelAmenity.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.Id != amenityId));
                    //checks to see that: there isnt already a room with that same name but different id = duplicate

                    return hotelAmenity;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HotelAmenityDTO> UpdateHotelAmenity(int amenityID, HotelAmenityDTO hotelAmenityDTO)
        {
            try
            {
                if (amenityID == hotelAmenityDTO.Id)
                {
                    HotelAmenity amenityDetails = await _db.HotelAmenity.FindAsync(amenityID);
                    HotelAmenity amenity = _mapper.Map<HotelAmenityDTO, HotelAmenity>(hotelAmenityDTO, amenityDetails);
                    amenity.UpdatedBy = "";
                    amenity.UpdatedDate = DateTime.Now;
                    var updatedAmenity = _db.HotelAmenity.Update(amenity);
                    await _db.SaveChangesAsync();
                    return _mapper.Map<HotelAmenity, HotelAmenityDTO>(updatedAmenity.Entity);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
