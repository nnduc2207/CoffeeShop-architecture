using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Model;
using CoffeeShop.Models;

namespace CoffeeShop.Services.ModelServices
{
    public class ThongSoService : NotifyPropertyChanged
    {
        private int _ma;
        public int Ma { get => _ma; set { _ma = value; OnPropertyChanged(); } }
        private string _ten;
        public string Ten { get => _ten; set { _ten = value; OnPropertyChanged(); } }
        private string _giaTri;
        public string GiaTri { get => _giaTri; set { _giaTri = value; OnPropertyChanged(); } }
        private string _kieu;
        public string Kieu { get => _kieu; set { _kieu = value; OnPropertyChanged(); } }

        public ThongSoService() { }

        public ThongSoService(ThongSo ts)
        {
            this.Ma = ts.Ma;
            this.Ten = ts.Ten;
            this.GiaTri = ts.GiaTri;
            this.Kieu = ts.Kieu;
        }

        static public ThongSoService GetById(int id)
        {
            ThongSo ts = DataProvider.Ins.DB.ThongSo.FirstOrDefault(x => x.Ma == id);
            return ts == null ? null : new ThongSoService(ts);
        }

        static public ThongSoService GetByName(string name)
        {
            ThongSo ts = DataProvider.Ins.DB.ThongSo.FirstOrDefault(x => x.Ten == name);
            return ts == null ? null : new ThongSoService(ts);
        }

        public void Update()
        {
            ThongSo ts = DataProvider.Ins.DB.ThongSo.FirstOrDefault(x => x.Ma == this.Ma);
            ts.GiaTri = this.GiaTri;
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
