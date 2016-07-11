using AutomationSorting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSorting.Data.Abstract
{
    public interface IUpcRepository
    {
        UPCDataModel GetUPC(string UPC);
    }
}
