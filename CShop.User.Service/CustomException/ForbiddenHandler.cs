using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Service.CustomException
{
    public class ForbiddenHandler : Exception
    {
        public ForbiddenHandler() { }
        public ForbiddenHandler(string message) : base(message) { }
    }
}
