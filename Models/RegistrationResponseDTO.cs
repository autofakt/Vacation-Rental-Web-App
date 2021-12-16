using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//output
namespace Models
{
    public class RegistrationResponseDTO
    {
        //used to show registration reponse messages. Output.
        public bool IsRegistrationSuccessful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
