﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutomationSorting.Utility
{
    public class UPCHelper
    {
        public static bool IsValidGtin(string upc)
        {
            Regex regex = new Regex(@"^\d+$");
            if (!regex.IsMatch(upc) || (upc.Length != 12 && upc.Length != 13))
                return false;

            return IsValidChecksumDigit(upc);
        }

        public static bool IsNumericCode(string code)
        {
            if (code != (new Regex("[^0-9]")).Replace(code, ""))
            {
                // is not numeric
                return false;
            }

            return true;
        }


        private static bool IsValidChecksumDigit(string code)
        {
            if (code == "6438158572584") // hardcode for Ken
                return true;

            if (code != (new Regex("[^0-9]")).Replace(code, ""))
            {
                // is not numeric
                return false;
            }
            // pad with zeros to lengthen to 14 digits
            switch (code.Length)
            {
                case 8:
                    code = "000000" + code;
                    break;
                case 12:
                    code = "00" + code;
                    break;
                case 13:
                    code = "0" + code;
                    break;
                case 14:
                    break;
                default:
                    // wrong number of digits
                    return false;
            }
            // calculate check digit
            int[] a = new int[13];
            a[0] = int.Parse(code[0].ToString()) * 3;
            a[1] = int.Parse(code[1].ToString());
            a[2] = int.Parse(code[2].ToString()) * 3;
            a[3] = int.Parse(code[3].ToString());
            a[4] = int.Parse(code[4].ToString()) * 3;
            a[5] = int.Parse(code[5].ToString());
            a[6] = int.Parse(code[6].ToString()) * 3;
            a[7] = int.Parse(code[7].ToString());
            a[8] = int.Parse(code[8].ToString()) * 3;
            a[9] = int.Parse(code[9].ToString());
            a[10] = int.Parse(code[10].ToString()) * 3;
            a[11] = int.Parse(code[11].ToString());
            a[12] = int.Parse(code[12].ToString()) * 3;
            int sum = a[0] + a[1] + a[2] + a[3] + a[4] + a[5] + a[6] + a[7] + a[8] + a[9] + a[10] + a[11] + a[12];
            int check = (10 - (sum % 10)) % 10;
            // evaluate check digit
            int last = int.Parse(code[13].ToString());
            return check == last;
        }

        public static bool IsPseudoValid(string upc)
        {
            if (string.IsNullOrWhiteSpace(upc)) { return false; }
            if (upc.Length < 12) { return false; }
            if (upc.Length > 14) { return false; }
            if (!new Regex(@"^\d+$").IsMatch(upc)) { return false; }

            return true;
        }
    }
}
