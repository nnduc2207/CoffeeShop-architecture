using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Model;
using CoffeeShop.Models;

namespace CoffeeShop.Services.ModelServices
{
    public class NguyenLieuService
    {
        static public NguyenLieu GetById(int maNL, int maSP)
        {
            return DataProvider.Ins.DB.NguyenLieu.FirstOrDefault(x => x.MaNL == maNL && x.MaSP == maSP);
        }

        static public List<NguyenLieu> GetByGradient(int maNL)
        {
            return DataProvider.Ins.DB.NguyenLieu.Where(x => x.MaNL == maNL).ToList();
        }

        static public List<NguyenLieu> GetByProduct(int maSP)
        {
            return DataProvider.Ins.DB.NguyenLieu.Where(x => x.MaSP == maSP).ToList();
        }

        static public List<NguyenLieu> GetAll()
        {
            return DataProvider.Ins.DB.NguyenLieu.ToList();
        }

        static public NguyenLieu Create(int masp, int manl, int soluong)
        {
            NguyenLieu newnl = new NguyenLieu
            {
                MaNL = manl,
                MaSP = masp,
                SoLuong = soluong,
            };
            DataProvider.Ins.DB.NguyenLieu.Add(newnl);
            DataProvider.Ins.DB.SaveChanges();
            return newnl;
        } 

        static public void Update()
        {
            DataProvider.Ins.DB.SaveChanges();
        }

        static public void Delete(NguyenLieu nguyenlieu)
        {
            DataProvider.Ins.DB.NguyenLieu.Remove(nguyenlieu);
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
