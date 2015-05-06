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
        public UnitResponse() { }
        public UnitResponse(string name, UnitType type)
        {
            Name = name;
            Type = type;
        }
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public UnitType Type { get; set; }
    }
}
