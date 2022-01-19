using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Services.Events
{
    class BuyNGetMEvent : Event
    {
        private int n;
        private int m;

        public BuyNGetMEvent(string productname, int buy, int get) : base(productname)
        {
            this.n = buy;
            this.m = get;
        }

        public override string GetName()
        {
            return productName.ToUpper() + ": " + "Mua " + n + " Tặng " + m;
        }

        protected override bool CanExecute(dynamic item)
        {
            return (item.TongGia / item.Gia) >= n;
        }

        protected override dynamic Calculate(dynamic item)
        {
            return new
            {
                Ma = item.Ma,
                Ten = item.Ten,
                Gia = item.Gia,
                SoLuong = (item.TongGia / item.Gia / n * m) + (item.TongGia / item.Gia),
                TongGia = item.TongGia,
            };
        }

        public override void SaveToFile(StreamWriter sw)
        {
            sw.WriteLine("DiscountEvent");
            sw.WriteLine(3);
            sw.WriteLine(this.productName);
            sw.WriteLine(this.n);
            sw.WriteLine(this.m);
        }
    }
}
