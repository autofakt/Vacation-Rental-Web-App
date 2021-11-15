using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla_Client.Service.IService
{
    public interface IAuthenticationService
    {
        Task<RegistrationResponseDTO> RegisterUser(UserRequestDTO userForRegistration);
        Task<AuthenticationResponseDTO> Login(AuthenticationDTO userFromAuthentication);
        Task LogOut(); //remove local storage and log user out

    }
}
