using CoffeeShop.Services.ModelServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Services.SortStrategies
{
    class PriceSort : ProductSort
    {
        public PriceSort(bool isInc) : base(isInc) { }

        public override string GetName()
        {
            if (isIncrement)
            {
                return "Cheap to expensive";
            }
            return "Expensive to cheap";
        }

        public override void Sort(ref List<SanPhamService> productList)
        {
            for (int i = 0; i < productList.Count - 1; i++)
            {
                for (int j = i + 1; j < productList.Count; j++)
                {
                    if (isIncrement && productList[i].Gia > productList[j].Gia)
                    {
                        Swap(ref productList, i, j);
                    }
                    else if (!isIncrement && productList[i].Gia < productList[j].Gia)
                    {
                        Swap(ref productList, i, j);
                    }
                }
            }
        }
    }
}
