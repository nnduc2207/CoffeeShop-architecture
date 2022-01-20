using CoffeeShop.Services.ModelServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Services.SortStrategies
{
    public class NameSort : ProductSort
    {
        public NameSort(bool isInc) : base(isInc) { }

        public override string GetName()
        {
            if (isIncrement)
            {
                return "A to Z";
            }
            return "Z to A";
        }

        public override void Sort(ref List<SanPhamService> productList)
        {
            for (int i = 0; i < productList.Count - 1; i++)
            {
                for (int j = i + 1; j < productList.Count; j++)
                {
                    if (isIncrement && String.Compare(productList[i].Ten, productList[j].Ten) > 0)
                    {
                        Swap(ref productList, i, j);
                    }
                    else if (!isIncrement && String.Compare(productList[i].Ten, productList[j].Ten) < 0)
                    {
                        Swap(ref productList, i, j);
                    }
                }
            }
        }
    }
}
