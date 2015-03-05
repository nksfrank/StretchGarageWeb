using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StretchGarageWeb.Models.Objects
{
    public class ParkedCar
    {
        public int Id { get; set; }
        public int UnitId { get; set; }
        public Unit Unit {get; set;}
        public int ParkingPlaceId { get; set; }
        public ParkingPlace ParkingPlace { get; set; }

    }
}