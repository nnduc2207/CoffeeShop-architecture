using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Model;
using CoffeeShop.Models;

namespace CoffeeShop.Services.ModelServices
{
    public class HoaDonService
    {
        static public HoaDon GetById(int id)
        {
            return DataProvider.Ins.DB.HoaDon.FirstOrDefault(x => x.Ma == id);
        }

        static public List<HoaDon> GetAll()
        {
            return DataProvider.Ins.DB.HoaDon.ToList();
        }
        static public HoaDon Create(int tongtien, int diemtichluy = 0, int makh = 0)
        {
            int mahd = DataProvider.Ins.DB.HoaDon.Count() == 0 ? 1 : DataProvider.Ins.DB.HoaDon.Max(x => x.Ma) + 1;
            HoaDon newHD = new HoaDon
            {
                Ma = mahd,
                NgayTao = DateTime.Now,
                TongTien = tongtien,
                DiemTichLuy = diemtichluy,
            };
            if (makh != 0)
            {
                newHD.MaKH = makh;
                newHD.DiemTichLuy = diemtichluy;
            }
            DataProvider.Ins.DB.HoaDon.Add(newHD);
            DataProvider.Ins.DB.SaveChanges();
            return newHD;
        }

        static public void Delete(HoaDon hoadon)
        {
            DataProvider.Ins.DB.HoaDon.Remove(hoadon);
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
