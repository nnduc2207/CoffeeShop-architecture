using CoffeeShop.Services.ModelServices;
using CoffeeShop.Services.SortStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Services
{
    public abstract class ProductSort
    {
        protected bool isIncrement;

        protected ProductSort(bool isInc) { this.isIncrement = isInc; }

        public static List<ProductSort> GetAll()
        {
            List<ProductSort> res = new List<ProductSort>();
            res.Add(new NameSort(true));
            res.Add(new NameSort(false));
            res.Add(new PriceSort(true));
            res.Add(new PriceSort(false));
            return res;
        }

        public virtual string GetName() { return ""; }

        public virtual void Sort(ref List<SanPhamService> productList)
        {
            // Do nothing
        }

        protected void Swap(ref List<SanPhamService> productList, int i, int j)
        {
            dynamic tmp1 = productList[i];
            dynamic tmp2 = productList[j];
            productList.RemoveAt(i);
            productList.Insert(i, tmp2);
            productList.RemoveAt(j);
            productList.Insert(j, tmp1);
        }
    }
}
