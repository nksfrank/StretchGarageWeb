using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public abstract class MainHandler
    {
        protected DataLayer.Database.dbDataContext DB = new DataLayer.Database.dbDataContext();
    }
}
