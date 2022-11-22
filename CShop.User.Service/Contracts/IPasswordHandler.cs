using CShop.User.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Service.Contracts
{
    public interface IPasswordHandler
    {
        Tuple<byte[], byte[]> HashPassword(string password);
        bool VerifyHash(string password, byte[] passwordHash, byte[] passwordSalt);

        string CreateToken(UserModel user);

        int GetLoggedInId();
        string GetLoggedInUsername();
        string GetTokenCreationTime();
        Boolean HttpContextExists();
        void DeleteToken();
    }
}
