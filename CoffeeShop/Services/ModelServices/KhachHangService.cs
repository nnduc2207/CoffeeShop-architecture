using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Model;
using CoffeeShop.Models;
using System.ComponentModel;

namespace CoffeeShop.Services.ModelServices
{
    public class KhachHangService : NotifyPropertyChanged
    {
        private int _ma;
        public int Ma { get => _ma; set { _ma = value; OnPropertyChanged(); } }
        private string _ten;
        public string Ten { get => _ten; set { _ten = value; OnPropertyChanged(); } }
        private string _sdt;
        public string SDT { get => _sdt; set { _sdt = value; OnPropertyChanged(); } }
        private int _diemTichLuy;
        public int DiemTichLuy { get => _diemTichLuy; set { _diemTichLuy = value; OnPropertyChanged(); } }
        private int _tongChiTieu;
        public int TongChiTieu { get => _tongChiTieu; set { _tongChiTieu = value; OnPropertyChanged(); } }

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

        static public KhachHangService GetById(int id)
        {
            KhachHang kh = DataProvider.Ins.DB.KhachHang.FirstOrDefault(x => x.Ma == id);
            return kh == null ? null : new KhachHangService(kh);
        }

        static public KhachHangService GetByPhone(string phonenumber)
        {
            KhachHang kh = DataProvider.Ins.DB.KhachHang.FirstOrDefault(x => x.SDT == phonenumber);
            return kh == null ? null : new KhachHangService(kh);
        }

        static public List<KhachHangService> GetAll()
        {
            List<KhachHangService> res = new List<KhachHangService>();
            foreach (KhachHang item in DataProvider.Ins.DB.KhachHang.ToList())
            {
                res.Add(new KhachHangService(item));
            }
            return res;
        }

        public KhachHangService Create()
        {
            KhachHang newkh = new KhachHang
            {
                Ma = (DataProvider.Ins.DB.KhachHang.Count() == 0) ? 1 : DataProvider.Ins.DB.KhachHang.Max(x => x.Ma) + 1,
                Ten = this.Ten,
                SDT = this.SDT,
                DiemTichLuy = 0,
                TongChiTieu = 0,
            };
            DataProvider.Ins.DB.KhachHang.Add(newkh);
            DataProvider.Ins.DB.SaveChanges();
            return new KhachHangService(newkh);
        }

        public void Update()
        {
            KhachHang kh = DataProvider.Ins.DB.KhachHang.FirstOrDefault(x => x.Ma == this.Ma);
            kh.Ten = this.Ten;
            kh.SDT = this.SDT;
            kh.TongChiTieu = this.TongChiTieu;
            kh.DiemTichLuy = this.DiemTichLuy;
            DataProvider.Ins.DB.SaveChanges();
        }

        public void Delete()
        {
            KhachHang khachhang = DataProvider.Ins.DB.KhachHang.FirstOrDefault(x => x.Ma == this.Ma);
            DataProvider.Ins.DB.KhachHang.Remove(khachhang);
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
