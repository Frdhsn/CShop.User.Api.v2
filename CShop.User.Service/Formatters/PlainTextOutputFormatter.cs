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
//    public class PlainTextOutputFormatter : TextOutputFormatter
//    {
//        public PlainTextOutputFormatter()
//        {
//            SupportedMediaTypes.Add("text/plain");
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
//                    ConvertToPlain(buffer, post);
//                }
//            }
//            else
//            {
//                ConvertToPlain(buffer, (CartDTO)context.Object);
//            }
//            await response.WriteAsync(buffer.ToString());
//        }

//        private static void ConvertToPlain(StringBuilder buffer, CartDTO story)
//        {
//            buffer.AppendLine($"Story Id: {story.Id}");
//            buffer.AppendLine($"Title: {story.Title}");
//            buffer.AppendLine($"AuthorID: {story.AuthorID}");
//            buffer.AppendLine($"Description: {story.Description}");
//            buffer.AppendLine($"Created At: {story.CreationTime}");
//            buffer.AppendLine($"Modified At: {story.LastModifiedTime}");
//            buffer.AppendLine();
//        }
//    }
//}
