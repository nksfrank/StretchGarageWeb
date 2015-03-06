using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Database;
using Objects;
using Objects.Interface;

namespace BusinessLayer
{
    public abstract class MainHandler
    {
        protected DataLayer.Database.dbDataContext DB = new DataLayer.Database.dbDataContext();
    }
}
