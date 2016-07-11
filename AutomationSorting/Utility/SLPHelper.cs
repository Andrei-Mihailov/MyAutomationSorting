using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutomationSorting.Utility
{
    public static class SLPHelper
    {

        public static bool IsValidSLP(string SLP)
        {
            Regex regex = new Regex(@"^\d[8-10]$");
            if (!regex.IsMatch(SLP))
                return false;

            return true;
        }
    }
}
