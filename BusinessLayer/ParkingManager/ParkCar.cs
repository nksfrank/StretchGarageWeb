using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Database;
using Objects;
using Objects.Interface;

namespace BusinessLayer.ParkingManager
{
    public class ParkCar : MainHandler
    {
        public IError Add(Unit car, ParkingPlace parkingPlace)
        {
            return Add(car, parkingPlace, DateTime.Now);
        }

        public IError Add(Unit car, ParkingPlace parkingPlace, DateTime date)
        {
            ParkedCar park = new ParkedCar();
            park.ParkingDate = date;
            park.ParkingPlaceId = parkingPlace.Id;
            park.UnitId = car.Id;
            park.IsParked = true;

            DB.ParkedCars.InsertOnSubmit(park);

            try
            {
                DB.SubmitChanges();
            }
            catch (Exception ex)
            {
                return new Error() { Success = false, Message = "Could not add car " + car.Id + " to parking place " + parkingPlace.Id };
            }
            return new Error() { Success = true, Message = "" };
        }

        public IError Remove(Unit car, ParkingPlace parkingPlace)
        {
            ParkedCar park = DB.ParkedCars.Where(a => a.UnitId == car.Id && a.ParkingPlace.Id == parkingPlace.Id && a.IsParked == true).First(a => a.IsParked);
            park.IsParked = false;

            DB.ParkedCars.InsertOnSubmit(park);

            try
            {
                DB.SubmitChanges();
            }
            catch (Exception ex)
            {
                return new Error() { Success = false, Message = "Could not remove car " + car.Id + " from parking place " + parkingPlace.Id };
            }
            return new Error() { Success = true, Message = "" };
        }
    }
}
