using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Model;
using CoffeeShop.Models;

namespace CoffeeShop.Services.ModelServices
{
    public class KhoNguyenLieuService : NotifyPropertyChanged
    {
        private int _ma;
        public int Ma { get => _ma; set { _ma = value; OnPropertyChanged(); } }
        private string _ten;
        public string Ten { get => _ten; set { _ten = value; OnPropertyChanged(); } }
        private int _soLuong;
        public int SoLuong { get => _soLuong; set { _soLuong = value; OnPropertyChanged(); } }
        private string _donVi;
        public string DonVi { get => _donVi; set { _donVi = value; OnPropertyChanged(); } }

        public KhoNguyenLieuService() { }

        public KhoNguyenLieuService(KhoNguyenLieu knl)
        {
            this.Ma = knl.MaNL;
            this.Ten = knl.Ten;
            this.SoLuong = knl.SoLuong;
            this.DonVi = knl.DonVi;
        }

        static public KhoNguyenLieuService GetById(int id)
        {
            return new KhoNguyenLieuService(DataProvider.Ins.DB.KhoNguyenLieu.FirstOrDefault(x => x.MaNL == id));
        }

        static public List<KhoNguyenLieu> GetAll()
        {
            return DataProvider.Ins.DB.KhoNguyenLieu.ToList();
        }

        public KhoNguyenLieuService Create()
        {
            if (this.Ten == null || this.Ten.Split(' ').Length == this.Ten.Length + 1)
            {
                throw new Exception("Vui lòng không để trống tên nguyên liệu");
            }
            if (this.SoLuong < 0)
            {
                throw new Exception("Vui lòng đặt số lượng hợp lý");
            }
            if (this.DonVi == null || this.DonVi.Split(' ').Length == this.DonVi.Length + 1)
            {
                throw new Exception("Vui lòng không để trống tên đơn vị");
            }
            KhoNguyenLieu newknl = new KhoNguyenLieu
            {
                MaNL = DataProvider.Ins.DB.KhoNguyenLieu.Count() == 0 ? 1 : DataProvider.Ins.DB.KhoNguyenLieu.Max(x => x.MaNL) + 1,
                Ten = this.Ten,
                SoLuong = this.SoLuong,
                DonVi = this.DonVi,
            };
            DataProvider.Ins.DB.KhoNguyenLieu.Add(newknl);
            DataProvider.Ins.DB.SaveChanges();
            return new KhoNguyenLieuService(newknl);
        }   

        public void Update()
        {
            KhoNguyenLieu knl = DataProvider.Ins.DB.KhoNguyenLieu.FirstOrDefault(x => x.MaNL == this.Ma);
            knl.SoLuong = this.SoLuong;
            knl.Ten = this.Ten;
            knl.DonVi = this.DonVi;
            DataProvider.Ins.DB.SaveChanges();
        }

        public void Delete()
        {
            KhoNguyenLieu knl = DataProvider.Ins.DB.KhoNguyenLieu.FirstOrDefault(x => x.MaNL == this.Ma);
            DataProvider.Ins.DB.KhoNguyenLieu.Remove(knl);
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
