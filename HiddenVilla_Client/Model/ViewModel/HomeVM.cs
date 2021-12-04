using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla_Client.Model.ViewModel
{    //This class keeps track of the initial user inputs on the home page.
    public class HomeVM 
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
        public int NoOfNights { get; set; } = 1;
    }
}
