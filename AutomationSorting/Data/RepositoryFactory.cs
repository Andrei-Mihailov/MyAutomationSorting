using AutomationSorting.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSorting.Data
{
    public static class RepositoryFactory
    {
        public static IUpcRepository GetUpcRepository()
        {
            return new LocalDBSortingRepository();
        }

        public static ISlpRepository GetSlpRepository()
        {
            return new LocalDBSortingRepository();
        }

        public static IProductRepository GetProductRepository()
        {
            return new LocalDBSortingRepository();
        }
    }
}
