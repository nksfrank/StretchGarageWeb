﻿using System;
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
    public class UnitManager
    {
        private dbDataContext DB = new dbDataContext();
        public IError CreateUnit(string name, UnitType type)
        {
            Unit unit = new Unit() {
                Name = name,
                Type = (int)type,
                Settled = DateTime.UtcNow,
            };
            DB.Units.InsertOnSubmit(unit);

            try {
                DB.SubmitChanges();
            }
            catch (Exception) {
                return new Error() { Success = false, Message = "Could not add unit with " + name};
            }

            return new ApiResponse(true, "", unit.Id);
        }

        public IError GetUnitById(int id)
        {
            var unit = DB.Units.FirstOrDefault(a => a.Id == id);
            if (unit == null) return new Error() { Success = false, Message = "Could not find unit with id " + id };

            var unitResponse = new UnitResponse() { Id = unit.Id, Name = unit.Name, Type = (UnitType)unit.Type };

            return new ApiResponse(true, "", unitResponse);
        }
    }
}
