using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Objects.WebApiResponse
{
    [DataContract]
    public class UnitResponse
    {
        public UnitResponse(string name, int type)
        {
            Name = name;
            Type = type;
        }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Type { get; set; }
    }
}
