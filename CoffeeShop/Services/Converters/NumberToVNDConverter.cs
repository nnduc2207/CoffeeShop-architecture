using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CoffeeShop.Services.Converters
{
    public class NumberToVNDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.ToString() == "0") return "0 VNĐ";
            bool isNegative = false;
            var tmp = value.ToString();
            if (tmp == null)
            {

            }
            if (tmp[0] == '-')
            {
                isNegative = true;
                tmp = tmp.Substring(1, tmp.Length - 1);
            }
            string res = " VNĐ";
            do
            {
                if (tmp.Length > 3)
                {
                    res = "." + tmp.Substring(tmp.Length - 3, 3) + res;
                    tmp = tmp.Substring(0, tmp.Length - 3);
                }
                else
                {
                    res = tmp + res;
                    tmp = "";
                }
            } while (tmp != "");
            if (isNegative)
            {
                res = "- " + res;
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
