using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSLibrary.ADO.NET.Repositories
{
    public interface IRepository<T> where T :class
    {
        IEnumerable<T> GetAll();

    }
}
