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
    public class HoaDonService : NotifyPropertyChanged
    {
        private int _ma;
        public int Ma { get => _ma; set { _ma = value; OnPropertyChanged(); } }
        private int _maKH;
        public int MaKH { get => _maKH; set { _maKH = value; OnPropertyChanged(); } }
        private DateTime _ngayTao;
        public DateTime NgayTao { get => _ngayTao; set { _ngayTao = value; OnPropertyChanged(); } }
        private int _tongTien;
        public int TongTien { get => _tongTien; set { _tongTien = value; OnPropertyChanged(); } }
        private int _diemTichLuy;
        public int DiemTichLuy { get => _diemTichLuy; set { _diemTichLuy = value; OnPropertyChanged(); } }

        public HoaDonService() { }

        public HoaDonService(HoaDon hd)
        {
            Ma = hd.Ma;
            MaKH = hd.MaKH.GetValueOrDefault(0);
            NgayTao = hd.NgayTao;
            TongTien = hd.TongTien;
            DiemTichLuy = hd.DiemTichLuy.HasValue ? hd.DiemTichLuy.Value : 0;
        }

        static public HoaDonService GetById(int id)
        {
            HoaDon hd = DataProvider.Ins.DB.HoaDon.FirstOrDefault(x => x.Ma == id);
            return hd == null ? null : new HoaDonService(hd);
        }

        static public List<HoaDonService> GetAll()
        {
            List<HoaDonService> res = new List<HoaDonService>();
            foreach (HoaDon item in DataProvider.Ins.DB.HoaDon.ToList())
            {
                res.Add(new HoaDonService(item));
            }
            return res;
        }
        public HoaDonService Create()
        {
            int mahd = DataProvider.Ins.DB.HoaDon.Count() == 0 ? 1 : DataProvider.Ins.DB.HoaDon.Max(x => x.Ma) + 1;
            HoaDon newHD = new HoaDon
            {
                Ma = mahd,
                NgayTao = DateTime.Now,
                TongTien = this.TongTien,
                DiemTichLuy = this.DiemTichLuy,
            };
            if (this.MaKH != 0)
            {
                newHD.MaKH = this.MaKH;
                newHD.DiemTichLuy = this.DiemTichLuy;
            }
            DataProvider.Ins.DB.HoaDon.Add(newHD);
            DataProvider.Ins.DB.SaveChanges();
            return new HoaDonService(newHD);
        }

        public void Delete()
        {
            HoaDon hoadon = DataProvider.Ins.DB.HoaDon.FirstOrDefault(x => x.Ma == this.Ma);
            DataProvider.Ins.DB.HoaDon.Remove(hoadon);
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
