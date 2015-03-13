﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.WebApiResponse
{
    public class ParkingPlaceResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CssClass { get; set; }
        public int ParkingSpots { get; set; }
        public List<ParkedCar> ParkedCars { get; set; }
    }
}
