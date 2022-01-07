using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Model;
using CoffeeShop.Models;

namespace CoffeeShop.Services.ModelServices
{
    public class ThongSoService
    {
        static public ThongSo GetById(int id)
        {
            return DataProvider.Ins.DB.ThongSo.FirstOrDefault(x => x.Ma == id);
        }

        static public ThongSo GetByName(string name)
        {
            return DataProvider.Ins.DB.ThongSo.FirstOrDefault(x => x.Ten == name);
        }

        static public void Update()
        {
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
