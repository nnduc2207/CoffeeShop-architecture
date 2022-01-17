using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Model;
using CoffeeShop.Models;

namespace CoffeeShop.Services.ModelServices
{
    public class NguyenLieuService : NotifyPropertyChanged
    {
        private int _maNL;
        public int MaNL { get => _maNL; set { _maNL = value; OnPropertyChanged(); } }
        private int _maSP;
        public int MaSP { get => _maSP; set { _maSP = value; OnPropertyChanged(); } }
        private int _soLuong;
        public int SoLuong { get => _soLuong; set { _soLuong = value; OnPropertyChanged(); } }

        public NguyenLieuService() { }

        public NguyenLieuService(NguyenLieu nl)
        {
            this.MaNL = nl.MaNL;
            this.MaSP = nl.MaSP;
            this.SoLuong = nl.SoLuong;
        }

        static public NguyenLieuService GetById(int maNL, int maSP)
        {
            NguyenLieu nl = DataProvider.Ins.DB.NguyenLieu.FirstOrDefault(x => x.MaNL == maNL && x.MaSP == maSP);
            return nl == null ? null : new NguyenLieuService(nl);
        }

        static public List<NguyenLieuService> GetByGradient(int maNL)
        {
            List<NguyenLieuService> res = new List<NguyenLieuService>();
            foreach (NguyenLieu item in DataProvider.Ins.DB.NguyenLieu.Where(x => x.MaNL == maNL).ToList())
            {
                res.Add(new NguyenLieuService(item));
            }
            return res;
        }

        static public List<NguyenLieuService> GetByProduct(int maSP)
        {
            List<NguyenLieuService> res = new List<NguyenLieuService>();
            foreach (NguyenLieu item in DataProvider.Ins.DB.NguyenLieu.Where(x => x.MaSP == maSP).ToList())
            {
                res.Add(new NguyenLieuService(item));
            }
            return res;
        }

        static public List<NguyenLieuService> GetAll()
        {
            List<NguyenLieuService> res = new List<NguyenLieuService>();
            foreach (NguyenLieu item in DataProvider.Ins.DB.NguyenLieu.ToList())
            {
                res.Add(new NguyenLieuService(item));
            }
            return res;
        }

        public NguyenLieuService Create()
        {
            NguyenLieu newnl = new NguyenLieu
            {
                MaNL = this.MaNL,
                MaSP = this.MaSP,
                SoLuong = this.SoLuong,
            };
            DataProvider.Ins.DB.NguyenLieu.Add(newnl);
            DataProvider.Ins.DB.SaveChanges();
            return new NguyenLieuService(newnl);
        } 

        public void Update()
        {
            NguyenLieu nl = DataProvider.Ins.DB.NguyenLieu.FirstOrDefault(x => x.MaNL == this.MaNL && x.MaSP == this.MaSP);
            nl.MaNL = this.MaNL;
            nl.MaSP = this.MaSP;
            nl.SoLuong = this.SoLuong;
            DataProvider.Ins.DB.SaveChanges();
        }

        public void Delete()
        {
            NguyenLieu nguyenlieu = DataProvider.Ins.DB.NguyenLieu.FirstOrDefault(x => x.MaNL == this.MaNL && x.MaSP == this.MaSP);
            DataProvider.Ins.DB.NguyenLieu.Remove(nguyenlieu);
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
