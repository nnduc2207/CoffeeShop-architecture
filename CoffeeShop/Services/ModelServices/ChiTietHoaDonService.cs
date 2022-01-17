using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Model;
using CoffeeShop.Models;

namespace CoffeeShop.Services.ModelServices
{
    public class ChiTietHoaDonService : NotifyPropertyChanged
    {
        private int _ma;
        public int Ma { get => _ma; set { _ma = value; OnPropertyChanged(); } }
        private int _maHD;
        public int MaHD { get => _maHD; set { _maHD = value; OnPropertyChanged(); } }
        private string _tenSP;
        public string TenSP { get => _tenSP; set { _tenSP = value; OnPropertyChanged(); } }
        private int _giaSP;
        public int GiaSP { get => _giaSP; set { _giaSP = value; OnPropertyChanged(); } }
        private int _soLuong;
        public int SoLuong { get => _soLuong; set { _soLuong = value; OnPropertyChanged(); } }

        public ChiTietHoaDonService() { }

        public ChiTietHoaDonService(ChiTietHoaDon cthd)
        {
            Ma = cthd.Ma;
            MaHD = cthd.MaHD;
            TenSP = cthd.TenSP;
            GiaSP = cthd.GiaSP;
            SoLuong = cthd.SoLuong;
        }

        static public ChiTietHoaDonService GetById(int maHd, int ma)
        {
            ChiTietHoaDon cthd = DataProvider.Ins.DB.ChiTietHoaDon.FirstOrDefault(x => x.MaHD == maHd && x.Ma == ma);
            return cthd == null ? null : new ChiTietHoaDonService(cthd);
        }

        static public List<ChiTietHoaDonService> GetByHoaDon(int maHd)
        {
            List<ChiTietHoaDonService> res = new List<ChiTietHoaDonService>();
            foreach (ChiTietHoaDon item in DataProvider.Ins.DB.ChiTietHoaDon.Where(x => x.MaHD == maHd).ToList())
            {
                res.Add(new ChiTietHoaDonService(item));
            }
            return res;
        }

        static public List<ChiTietHoaDonService> GetAll()
        {
            List<ChiTietHoaDonService> res = new List<ChiTietHoaDonService>();
            foreach (ChiTietHoaDon item in DataProvider.Ins.DB.ChiTietHoaDon.ToList())
            {
                res.Add(new ChiTietHoaDonService(item));
            }
            return res;
        }

        public ChiTietHoaDonService Create()
        {
            ChiTietHoaDon newCthd = new ChiTietHoaDon
            {
                MaHD = this.MaHD,
                Ma = DataProvider.Ins.DB.ChiTietHoaDon.Where(x => x.MaHD == this.MaHD).Count() == 0 ? 1 : DataProvider.Ins.DB.ChiTietHoaDon.Where(x => x.MaHD == this.MaHD).Max(x => x.Ma) + 1,
                TenSP = this.TenSP,
                SoLuong = this.SoLuong,
                GiaSP = this.GiaSP
            };
            DataProvider.Ins.DB.ChiTietHoaDon.Add(newCthd);
            DataProvider.Ins.DB.SaveChanges();
            return new ChiTietHoaDonService(newCthd);
        }

        public void Delete()
        {
            ChiTietHoaDon cthd = DataProvider.Ins.DB.ChiTietHoaDon.FirstOrDefault(x => x.MaHD == this.MaHD && x.Ma == this.Ma);
            DataProvider.Ins.DB.ChiTietHoaDon.Remove(cthd);
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
