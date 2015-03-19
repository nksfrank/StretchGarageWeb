using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Objects.Interface;

namespace Objects.WebApiResponse
{
   
    public class CheckLocationResponse : IError
    {
       
        public int Interval { get; set; }
        
        public bool CheckSpeed { get; set; }

        public CheckLocationResponse() { }

        public CheckLocationResponse(int interval, bool checkSpeed)
        {
            Interval = interval;
            CheckSpeed = checkSpeed;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
