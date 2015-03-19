using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Database;
using Microsoft.SqlServer.Server;
using Objects;
using Objects.Interface;
using Objects.WebApiResponse;

namespace BusinessLayer.UnitMgr
{
    public class UnitManager: MainHandler
    {
        public static IError CreateUnit(string name, int type)
        {
            Unit unit = new Unit();
            unit.Name = name;
            unit.Type = type;
            unit.EnteredZone = DateTime.UtcNow;
            DB.Units.InsertOnSubmit(unit);

            try
            {
                DB.SubmitChanges();
            }
            catch (Exception)
            {
                return new Error() { Success = false, Message = "Could not add unit with " + name};
            }

            return new ApiResponse(true, "", unit.Id);
        }
    }
}
