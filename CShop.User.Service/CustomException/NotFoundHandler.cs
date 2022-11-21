using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Service.CustomException
{
    public class NotFoundHandler : Exception
    {
        public NotFoundHandler() { }
        public NotFoundHandler(string message) : base(message) { }
    }
}
