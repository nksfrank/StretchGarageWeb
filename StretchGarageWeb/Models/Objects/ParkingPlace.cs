using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StretchGarageWeb.Models.Objects
{
    public class ParkingPlace
    {
        public int Id { get; set; }
        public int Places { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public Zone Zone { get; set; }
    }
}