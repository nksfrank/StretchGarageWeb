using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Objects.Interface;

namespace Objects.WebApiResponse
{
    [DataContract]
    [KnownType(typeof(CheckLocationResponse))]
    [KnownType(typeof(ParkedCarResponse))]
    [KnownType(typeof(ParkingPlaceResponse))]
    [KnownType(typeof(UnitResponse))]
    public class ApiResponse : IError
    {
	    public ApiResponse() {}

	    public ApiResponse(bool success, string message, object content)
        {
            Success = success;
            Message = message;
            Content = content;
        }
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public object Content { get; set; }
    }
}
