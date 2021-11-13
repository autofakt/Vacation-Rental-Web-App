using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IRoomOrderDetailsRepository
    {
        public Task<RoomOrderDetailsDTO> Create(RoomOrderDetailsDTO details);
        public Task<RoomOrderDetailsDTO> MarkPaymentAsSuccessful(int id);
        public Task<RoomOrderDetailsDTO> GetOrderRoomDetail(int roomOrderId);
        public Task<IEnumerable<RoomOrderDetailsDTO>> GetAllOrderRoomDetails();
        public Task<bool> UpdateOrderStatus(int roomOrderId, string status);
        public Task<bool> IsRoomBooked(int RoomId, DateTime checkInDate, DateTime checkOutDate);
    }
}
