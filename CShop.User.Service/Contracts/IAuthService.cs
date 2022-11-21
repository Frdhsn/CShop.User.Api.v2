using CShop.User.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Service.Contracts
{
    public interface IAuthService
    {

        Task<UserDTO> Login(LoginDTO req);
        Task<UserDTO> SignUp(SignUpDTO req);
    }
}
