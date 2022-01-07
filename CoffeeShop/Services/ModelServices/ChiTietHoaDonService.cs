using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Model;
using CoffeeShop.Models;

namespace CoffeeShop.Services.ModelServices
{
    public class ChiTietHoaDonService
    {
        static public ChiTietHoaDon GetById(int maHd, int ma)
        {
            return DataProvider.Ins.DB.ChiTietHoaDon.FirstOrDefault(x => x.MaHD == maHd && x.Ma == ma);
        }

        static public List<ChiTietHoaDon> GetByHoaDon(int maHd)
        {
            return DataProvider.Ins.DB.ChiTietHoaDon.Where(x => x.MaHD == maHd).ToList();
        }

        static public List<ChiTietHoaDon> GetAll()
        {
            return DataProvider.Ins.DB.ChiTietHoaDon.ToList();
        }

        static public ChiTietHoaDon Create(int maHd, string tenSP, int soluong, int gia)
        {
            ChiTietHoaDon newCthd = new ChiTietHoaDon
            {
                MaHD = maHd,
                Ma = DataProvider.Ins.DB.ChiTietHoaDon.Where(x => x.MaHD == maHd).Count() == 0 ? 1 : DataProvider.Ins.DB.ChiTietHoaDon.Where(x => x.MaHD == maHd).Max(x => x.Ma) + 1,
                TenSP = tenSP,
                SoLuong = soluong,
                GiaSP = gia
            };
            DataProvider.Ins.DB.ChiTietHoaDon.Add(newCthd);
            DataProvider.Ins.DB.SaveChanges();
            return newCthd;
        }

        static public void Delete(ChiTietHoaDon cthd)
        {
            DataProvider.Ins.DB.ChiTietHoaDon.Remove(cthd);
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
