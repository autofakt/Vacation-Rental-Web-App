using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla_Server.Model
{
    public class BlazorRoom
    {
        public int ID { get; set; }
        public string RoomName{ get; set; }
        public double price { get; set; }
        public bool IsActive { get; set; }

        public List<BlazorRoomProp> RoomProps { get; set; }
    }
}
