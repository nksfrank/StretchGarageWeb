using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Object.Interface
{
    public interface IError
    {
        bool Success { get; set; }
        string Message { get; set; }
    }
}
