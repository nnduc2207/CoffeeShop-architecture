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
    public class ChiTietHoaDonService : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        private int _ma;
        public int Ma { get => _ma; set { _ma = value; NotifyPropertyChanged("Ma"); } }
        private int _maHD;
        public int MaHD { get => _maHD; set { _maHD = value; NotifyPropertyChanged("MaHD"); } }
        private string _tenSP;
        public string TenSP { get => _tenSP; set { _tenSP = value; NotifyPropertyChanged("TenSP"); } }
        private int _giaSP;
        public int GiaSP { get => _giaSP; set { _giaSP = value; NotifyPropertyChanged("GiaSP"); } }
        private int _soLuong;
        public int SoLuong { get => _soLuong; set { _soLuong = value; NotifyPropertyChanged("SoLuong"); } }

        public ChiTietHoaDonService(ChiTietHoaDon cthd)
        {
            Ma = cthd.Ma;
            MaHD = cthd.MaHD;
            TenSP = cthd.TenSP;
            GiaSP = cthd.GiaSP;
            SoLuong = cthd.SoLuong;
        }

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
