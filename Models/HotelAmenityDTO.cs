using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; /*for the require stuff*/
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class HotelAmenityDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter room name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }

        [Required(ErrorMessage = " Please enter a description")]
        public string Timing { get; set; }

        public string IconStyle { get; set; }

    }
}
