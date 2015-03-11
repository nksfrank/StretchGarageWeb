using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Interface;

namespace Objects.WebApiResponse
{
    public class ApiResponse : Interface.IError
    {
        public ApiResponse(bool success, string message, IEnumerable<object> content)
        {
            Success = success;
            Message = message;
            Content = content;
        }
        public bool Success { get; set; }
        public string Message { get; set; }

        public IEnumerable<object> Content { get; set; }
    }
}
