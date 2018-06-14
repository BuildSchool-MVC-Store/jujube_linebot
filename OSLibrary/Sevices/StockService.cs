using OSLibrary.ADO.NET.Repositories;
using OSLibrary.Containers;
using OSLibrary.Models;
using OSLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSLibrary.Sevices
{
    public class StockService
    {
        public IEnumerable<StockDetail> GetAll()
        {
            return RepositoryContainer.GetInstance<StockRepository>().GetAllOfBackStage();
        }
    }
}
