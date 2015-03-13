using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.WebApiResponse
{
    public class ParkedCarResponse
    {
        public ParkedCarResponse(bool isAvailable, string status, string cssClass)
        {
            IsAvailable = isAvailable;
            Status = status;
            CssClass = cssClass;
        }
        public bool IsAvailable { get; set; }
        public string Status{ get; set; }
        public string CssClass { get; set; }
    }
}
