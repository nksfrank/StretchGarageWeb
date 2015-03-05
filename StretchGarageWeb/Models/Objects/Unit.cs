using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StretchGarageWeb.Models.Objects
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int ZoneId { get; set; }
        public Zone Zone { get; set; }
        public DateTime EnteredZone { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
    }
}