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
    public class HoaDonService : INotifyPropertyChanged
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
        private int _maKH;
        public int MaKH { get => _maKH; set { _maKH = value; NotifyPropertyChanged("MaKH"); } }
        private string _ngayTao;
        public string NgayTao { get => _ngayTao; set { _ngayTao = value; NotifyPropertyChanged("NgayTao"); } }
        private int _tongTien;
        public int TongTien { get => _tongTien; set { _tongTien = value; NotifyPropertyChanged("TongTien"); } }

        public HoaDonService(HoaDon hd)
        {
            Ma = hd.Ma;
            MaKH = hd.MaKH.GetValueOrDefault(0);
            NgayTao = hd.NgayTao.ToString();
            TongTien = hd.TongTien;
        }

        static public HoaDon GetById(int id)
        {
            return DataProvider.Ins.DB.HoaDon.FirstOrDefault(x => x.Ma == id);
        }

        static public List<HoaDon> GetAll()
        {
            return DataProvider.Ins.DB.HoaDon.ToList();
        }
        static public HoaDon Create(int tongtien, int diemtichluy = 0, int makh = 0)
        {
            int mahd = DataProvider.Ins.DB.HoaDon.Count() == 0 ? 1 : DataProvider.Ins.DB.HoaDon.Max(x => x.Ma) + 1;
            HoaDon newHD = new HoaDon
            {
                Ma = mahd,
                NgayTao = DateTime.Now,
                TongTien = tongtien,
                DiemTichLuy = diemtichluy,
            };
            if (makh != 0)
            {
                newHD.MaKH = makh;
                newHD.DiemTichLuy = diemtichluy;
            }
            DataProvider.Ins.DB.HoaDon.Add(newHD);
            DataProvider.Ins.DB.SaveChanges();
            return newHD;
        }

        static public void Delete(HoaDon hoadon)
        {
            DataProvider.Ins.DB.HoaDon.Remove(hoadon);
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
