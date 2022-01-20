using CoffeeShop.Services.Events;
using CoffeeShop.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Services
{
    public abstract class Event
    {
        protected string productName;

        protected Event(string productname)
        {
            this.productName = productname.ToUpper();
        }

        public string GetProductName() { return productName; }

        public static Event Create(string eventtype, string[] strParams)
        {
            Event @event = null;
            switch (eventtype)
            {
                case "Discount":
                case "DiscountEvent":
                    @event = new DiscountEvent(strParams[0], int.Parse(strParams[1]));
                    break;
                case "BuyNGetMEvent": 
                case "BuyNGetM":
                    @event = new BuyNGetMEvent(strParams[0], int.Parse(strParams[1]), int.Parse(strParams[2]));
                    break;
            }
            return @event;
        }

        public virtual string GetName()
        {
            return "";
        }

        protected virtual bool CanExecute(dynamic item) { return true; }

        protected virtual dynamic Calculate(dynamic item) { return null; }

        public dynamic Execute(dynamic item)
        {
            if (item.Ten.ToUpper() == productName)
            {
                if (CanExecute(item))
                {
                    return Calculate(item);
                }
            }
            return item;
        }

        public virtual void SaveToFile(StreamWriter sw)
        {
            // Do nothing
        }
    }
}
