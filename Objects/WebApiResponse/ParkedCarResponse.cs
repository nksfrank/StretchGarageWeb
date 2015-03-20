using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Objects.WebApiResponse
{
    [DataContract]
    public class ParkedCarResponse
    {
        [DataMember]
        public bool IsAvailable { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string CssClass { get; set; }

        public ParkedCarResponse() {}
        public ParkedCarResponse(bool isAvailable, string status, string cssClass)
        {
            IsAvailable = isAvailable;
            Status = status;
            CssClass = cssClass;
        }
    }
}
