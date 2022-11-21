//using CShop.User.Service.DTO;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc.Formatters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CShop.User.Service.Formatters
//{
//    public class CSVOutputFormatter: TextOutputFormatter
//    {
//        public CSVOutputFormatter()
//        {
//            SupportedMediaTypes.Add("text/csv");
//            SupportedEncodings.Add(Encoding.UTF8);
//            SupportedEncodings.Add(Encoding.Unicode);
//        }

//        protected override bool CanWriteType(Type type)
//        {
//            if (typeof(CartDTO).IsAssignableFrom(type) || typeof(IEnumerable<CartDTO>).IsAssignableFrom(type))
//                return base.CanWriteType(type);
//            return false;
//        }

//        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
//        {
//            var response = context.HttpContext.Response;
//            var buffer = new StringBuilder();

//            if (context.Object is IEnumerable<CartDTO>)
//            {
//                IEnumerable<CartDTO> posts = (IEnumerable<CartDTO>)context.Object;
//                foreach (CartDTO post in posts)
//                {
//                    ConvertToCsv(buffer, post);
//                }
//            }
//            else
//            {
//                ConvertToCsv(buffer, (CartDTO)context.Object);
//            }
//            await response.WriteAsync(buffer.ToString());
//        }

//        private static void ConvertToCsv(StringBuilder buffer, CartDTO post)
//        {
//            buffer.AppendLine($"{post.Id},{post.AuthorID},{post.Title},{post.Description},{post.Topic},{post.Difficulty},{post.CreationTime},{post.LastModifiedTime}");
//        }
//    }
//}
