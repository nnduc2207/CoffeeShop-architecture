using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Model;
using CoffeeShop.Models;
using CoffeeShop.Model;
using CoffeeShop.Models;
using System.ComponentModel;

namespace CoffeeShop.Services.ModelServices
{
    public class KhachHangService : INotifyPropertyChanged
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
        private string _ten;
        public string Ten { get => _ten; set { _ten = value; NotifyPropertyChanged("Ten"); } }
        private string _sdt;
        public string SDT { get => _sdt; set { _sdt = value; NotifyPropertyChanged("SDT"); } }
        private int _diemTichLuy;
        public int DiemTichLuy { get => _diemTichLuy; set { _diemTichLuy = value; NotifyPropertyChanged("DiemTichLuy"); } }
        private int _tongChiTieu;
        public int TongChiTieu { get => _tongChiTieu; set { _tongChiTieu = value; NotifyPropertyChanged("TongChiTieu"); } }

        public ILoaiKhachHang LoaiKhachHang;

        public KhachHangService() { }

        public KhachHangService(KhachHang kh)
        {
            Ma = kh.Ma;
            Ten = kh.Ten;
            SDT = kh.SDT;
            DiemTichLuy = kh.DiemTichLuy;
            TongChiTieu = kh.TongChiTieu;
            LoaiKhachHang = LoaiKhachHangFactory.create(kh.TongChiTieu);
        }

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
