using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Model;
using CoffeeShop.Models;

namespace CoffeeShop.Services.ModelServices
{
    public class LoaiSanPhamService
    {
        static public LoaiSanPham GetById(int id)
        {
            return DataProvider.Ins.DB.LoaiSanPham.FirstOrDefault(x => x.Ma == id);
        }

        static public LoaiSanPham GetByName(string name)
        {
            return DataProvider.Ins.DB.LoaiSanPham.FirstOrDefault(x => x.Ten == name);
        }
        static public List<LoaiSanPham> GetAll()
        {
            return DataProvider.Ins.DB.LoaiSanPham.ToList();
        }

        static public LoaiSanPham Create(string name)
        {
            LoaiSanPham newlsp = new LoaiSanPham
            {
                Ma = DataProvider.Ins.DB.LoaiSanPham.Count() == 0 ? 1 : DataProvider.Ins.DB.LoaiSanPham.Max(x => x.Ma) + 1,
                Ten = name
            };
            DataProvider.Ins.DB.LoaiSanPham.Add(newlsp);
            DataProvider.Ins.DB.SaveChanges();
            return newlsp;
        }

        static public void Delete(LoaiSanPham lsp)
        {
            DataProvider.Ins.DB.LoaiSanPham.Remove(lsp);
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
