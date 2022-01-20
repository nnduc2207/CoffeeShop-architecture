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
    public class EventManager
    {
        static private string strDefaultFile = "events.txt";

        static private Dictionary<string, Event> events;
        static EventManager()
        {
            events = new Dictionary<string, Event>();
            LoadDefaultEvent();
        }

        private static void LoadDefaultEvent()
        {
            LoadEventsFromFile(Path.GetFullPath(strDefaultFile));
        }

        public static List<string> AllCurrentEventList()
        {
            List<string> res = new List<string>();
            foreach (Event item in events.Values)
            {
                res.Add(item.GetName());
            }
            return res;
        }

        public static bool LoadEventsFromFile(string strDir)
        {
            try
            {
                StreamReader sr = new StreamReader(strDir);
                int count = int.Parse(sr.ReadLine());
                for (int i = 0; i < count; i++)
                {
                    Event @event = ReadEvent(sr);
                    if (@event != null)
                    {
                        if (events.ContainsKey(@event.GetProductName()))
                        {
                            events[@event.GetProductName()] = @event;
                        }
                        else
                        {
                            events.Add(@event.GetProductName(), @event);
                        }
                    }
                }
                sr.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static Event ReadEvent(StreamReader sr)
        {
            string eventType = sr.ReadLine();
            string[] strParams = new string[int.Parse(sr.ReadLine())];
            for (int i = 0; i < strParams.Length; i++)
            {
                strParams[i] = sr.ReadLine();
            }
            return Event.Create(eventType, strParams);
        }

        public static bool SaveToDefault()
        {
            try
            {
                StreamWriter sw = new StreamWriter(Path.GetFullPath(strDefaultFile));
                sw.WriteLine(events.Count);
                foreach (Event item in events.Values)
                {
                    item.SaveToFile(sw);
                }
                sw.Flush();
                sw.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveAllEvents()
        {
            try
            {
                events = new Dictionary<string, Event>();
                StreamWriter sw = new StreamWriter(Path.GetFullPath(strDefaultFile));
                sw.WriteLine(events.Count);
                sw.Flush();
                sw.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void ExecuteEvent(Cart cart)
        {
            AsyncObservableCollection<dynamic> newProductList = cart.ProductList;
            foreach (Event e in events.Values)
            {
                AsyncObservableCollection<dynamic> tmp = new AsyncObservableCollection<dynamic>();
                foreach (dynamic item in newProductList)
                {
                    tmp.Add(e.Execute(item));
                }
                newProductList = tmp;
            }
            cart.ProductList = newProductList;
            cart.Total = 0;
            foreach (dynamic item in cart.ProductList)
            {
                cart.Total += item.TongGia;
            }
        }
    }
}
