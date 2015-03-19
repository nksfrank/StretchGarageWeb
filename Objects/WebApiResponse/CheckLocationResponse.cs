using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Objects.Interface;

namespace Objects.WebApiResponse
{
    [DataContract]
    public class CheckLocationResponse
    {
        [DataMember]
        public int Interval { get; set; }
        [DataMember]
        public bool CheckSpeed { get; set; }

        public CheckLocationResponse() { }

        public CheckLocationResponse(int interval, bool checkSpeed)
        {
            Interval = interval;
            CheckSpeed = checkSpeed;
        }
    }
}
