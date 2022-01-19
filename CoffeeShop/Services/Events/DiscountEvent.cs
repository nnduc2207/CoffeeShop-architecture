using CoffeeShop.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Services.Events
{
    public class DiscountEvent : Event
    {
        private int discountPercent;

        public DiscountEvent(string productname, int discountpercent) : base(productname)
        {
            this.discountPercent = discountpercent;
        }

        public override string GetName()
        {
            return productName.ToUpper() + ": " + "Giảm giá " + discountPercent + "%";
        }

        protected override bool CanExecute(dynamic item)
        {
            return true;
        }

        protected override dynamic Calculate(dynamic item)
        {
            return new
            {
                Ma = item.Ma,
                Ten = item.Ten,
                Gia = item.Gia,
                SoLuong = item.SoLuong,
                TongGia = item.Gia * item.SoLuong * (100 - discountPercent) / 100,
            };
        }

        public override void SaveToFile(StreamWriter sw)
        {
            sw.WriteLine("DiscountEvent");
            sw.WriteLine(2);
            sw.WriteLine(this.productName);
            sw.WriteLine(this.discountPercent);
        }
    }
}
