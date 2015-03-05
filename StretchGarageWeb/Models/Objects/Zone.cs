using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StretchGarageWeb.Models.Objects
{
    public class Zone
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public int PollFrequency { get; set; }
        public int Speed { get; set; }
        public int ParkingPlaceId { get; set; }
        public ParkingPlace ParkingPlace { get; set; }
    }
}