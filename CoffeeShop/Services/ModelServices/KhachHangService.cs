using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Model;
using CoffeeShop.Models;
using CoffeeShop.Model;
using CoffeeShop.Models;

namespace CoffeeShop.Services.ModelServices
{
    public class KhachHangService
    {
        static public KhachHang GetById(int id)
        {
            return DataProvider.Ins.DB.KhachHang.FirstOrDefault(x => x.Ma == id);
        }

        static public KhachHang GetByPhone(string phonenumber)
        {
            return DataProvider.Ins.DB.KhachHang.FirstOrDefault(x => x.SDT == phonenumber);
        }

        static public List<KhachHang> GetAll()
        {
            return DataProvider.Ins.DB.KhachHang.ToList();
        }

        static public KhachHang Create(string name, string sdt)
        {
            KhachHang newkh = new KhachHang
            {
                Ma = (DataProvider.Ins.DB.KhachHang.Count() == 0) ? 1 : DataProvider.Ins.DB.KhachHang.Max(x => x.Ma) + 1,
                Ten = name,
                SDT = sdt,
                DiemTichLuy = 0,
                TongChiTieu = 0,
            };
            DataProvider.Ins.DB.KhachHang.Add(newkh);
            DataProvider.Ins.DB.SaveChanges();
            return newkh;
        }

        static public void Update()
        {
            DataProvider.Ins.DB.SaveChanges();
        }

        static public void Delete(KhachHang khachhang)
        {
            DataProvider.Ins.DB.KhachHang.Remove(khachhang);
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
