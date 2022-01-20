using CoffeeShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CoffeeShop.ViewModels.BaseViewModel;

namespace CoffeeShop.Services
{
    public class SearchService
    {
        public static AsyncObservableCollection<dynamic> SearchByName(string inputKeyWord, AsyncObservableCollection<dynamic> inputList, string fieldName)
        {
            AsyncObservableCollection<dynamic> outputList = new AsyncObservableCollection<dynamic>();
            if (inputKeyWord != "" && inputKeyWord != null)
            {
                string formatKeywords = RemoveSign(inputKeyWord.Trim().ToLower());
                foreach (var item in inputList)
                {
                    string fieldValue = GetPropValue(item, fieldName);
                    string itemFieldString = RemoveSign(fieldValue.ToLower());
                    if (itemFieldString.Contains(formatKeywords))
                    {
                        outputList.Add(item);
                    }
                }
            }
            else
            {
                foreach (var item in inputList)
                {
                    outputList.Add(item);
                }
            }
            return outputList;
        }
        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private static string RemoveSign(string inputString)
        {
            string[] VietNamChar = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };

            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    inputString = inputString.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            return inputString;
        }
    }
}
