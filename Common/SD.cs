using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{   //static details
    public static class SD
    {
        public const string Role_Admin = "Admin";
        public const string Role_Customer = "Customer";
        public const string Role_Employee = "Employee";

        public const string Local_InitialBoking = "InitialRoomBookingInfo";  //used to store home screen inputs in localStorage (start,end,numOfDays)
        public const string Local_RoomOrderDetails = "RoomOrderDetails";
        //For auth
        public const string Local_Token = "JWT Token";
        public const string Local_UserDetails = "User Details";

        //Status codes for RoomOrderDetails
        public const string Status_Pending = "Pending";
        public const string Status_Booked = "Booked";
        public const string Status_CheckedIn = "Checked-In";
        public const string Status_CheckedOut = "Checked-Out";
        public const string Status_NoShow = "NoShow";
        public const string Status_Cancelled = "Cancelled";

    }
}
