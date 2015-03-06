using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Database;
using BusinessLayer.Object;
using BusinessLayer.Object.Interface;

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

            DB.ParkedCars.InsertOnSubmit(park);

            try
            {
                DB.SubmitChanges();
            }
            catch (Exception ex)
            {
                return new Error() { Success = false, Message = ex.Message };
            }
            return new Error() { Success = true, Message = "" };
        }
    }
}
