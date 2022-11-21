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
//    public class HtmlOutputFormatter: TextOutputFormatter
//    {
//        public HtmlOutputFormatter()
//        {
//            SupportedMediaTypes.Add("text/html");
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
//                    ConvertToHtml(buffer, post);
//                }
//            }
//            else
//            {
//                ConvertToHtml(buffer, (CartDTO)context.Object);
//            }
//            await response.WriteAsync(buffer.ToString());
//        }

//        private static void ConvertToHtml(StringBuilder buffer, CartDTO story)
//        {
//            buffer.AppendLine($"<p><h4>Id: {story.Id}</h4></p>");
//            buffer.AppendLine($"<p><h4>Title: {story.Title}</h4></p>");
//            buffer.AppendLine($"<p><h2>Authorname: {story.AuthorID}</h2></p>");
//            buffer.AppendLine($"<p>Description: {story.Description}</p>");
//            buffer.AppendLine($"<p><small>Created At: {story.CreationTime}</small></p>");
//            buffer.AppendLine($"<p><small>Modified At: {story.LastModifiedTime}</small></p>");
//            buffer.AppendLine();
//        }
//    }
//}
