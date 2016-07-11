using AutomationSorting.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomationSorting.Model;

namespace AutomationSorting.Data
{
    public class  LocalDBSortingRepository : IUpcRepository, ISlpRepository, IProductRepository
    {
        public void CheckIn(ProductDataModel product)
        {
            throw new NotImplementedException();
        }

        public SLPDataModel GetSLP(string code)
        {
            throw new NotImplementedException();
        }

        public UPCDataModel GetUPC(string UPC)
        {
            throw new NotImplementedException();
        }
    }
}
