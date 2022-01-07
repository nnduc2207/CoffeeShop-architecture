using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Model;
using CoffeeShop.Models;

namespace CoffeeShop.Services.ModelServices
{
    public class KhoNguyenLieuService
    {
        static public KhoNguyenLieu GetById(int id)
        {
            return DataProvider.Ins.DB.KhoNguyenLieu.FirstOrDefault(x => x.MaNL == id);
        }

        static public List<KhoNguyenLieu> GetAll()
        {
            return DataProvider.Ins.DB.KhoNguyenLieu.ToList();
        }

        static public KhoNguyenLieu Create(string name, int soluong, string donvi)
        {
            KhoNguyenLieu newknl = new KhoNguyenLieu
            {
                MaNL = DataProvider.Ins.DB.KhoNguyenLieu.Count() == 0 ? 1 : DataProvider.Ins.DB.KhoNguyenLieu.Max(x => x.MaNL) + 1,
                Ten = name,
                SoLuong = soluong,
                DonVi = donvi,
            };
            DataProvider.Ins.DB.KhoNguyenLieu.Add(newknl);
            DataProvider.Ins.DB.SaveChanges();
            return newknl;
        }   

        static public void Update()
        {
            DataProvider.Ins.DB.SaveChanges();
        }

        static public void Delete(KhoNguyenLieu knl)
        {
            DataProvider.Ins.DB.KhoNguyenLieu.Remove(knl);
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
