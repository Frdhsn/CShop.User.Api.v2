using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Service.CustomException
{
    public class BadRequestHandler : Exception
    {
        public BadRequestHandler() { }
        public BadRequestHandler(string message) : base(message) { }
    }
}
