using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Objects.WebApiResponse
{
    [DataContract]
    public class UnitResponse
    {
        public UnitResponse(DataLayer.Database.Unit unit)
        {
            Id = unit.Id;
            Name = unit.Name;
            Phonenumber = unit.Phonenumber;
            Type = (UnitType)unit.Type;
        }
        public UnitResponse() { }

        public UnitResponse(string name, string number)
        {
            Name = name;
            Phonenumber = number;
        }
        public UnitResponse(string name, string number, UnitType type)
        {
            Name = name;
            Phonenumber = number;
            Type = type;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public UnitType Type { get; set; }
        [DataMember]
        public string Phonenumber { get; set; }
    }
}
