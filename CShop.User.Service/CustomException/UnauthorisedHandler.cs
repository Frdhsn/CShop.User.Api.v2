using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Service.CustomException
{
    public class UnauthorisedHandler : Exception
    {
        public UnauthorisedHandler() { }
        public UnauthorisedHandler(string message) : base(message) { }
    }
}
