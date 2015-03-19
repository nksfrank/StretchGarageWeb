using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Objects.WebApiResponse
{
    [DataContract]
    public class ParkingPlaceResponse
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string CssClass { get; set; }
        [DataMember]
        public int ParkingSpots { get; set; }
        [DataMember]
        public List<ParkedCarResponse> ParkedCars { get; set; }
    }
}
