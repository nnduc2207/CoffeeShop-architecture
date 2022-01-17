using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CoffeeShop.Model;
using CoffeeShop.Models;

namespace CoffeeShop.Services.ModelServices
{
    public class SanPhamService : NotifyPropertyChanged
    {
        private int _ma;
        public int Ma { get => _ma; set { _ma = value; OnPropertyChanged(); } }
        private string _ten;
        public string Ten { get => _ten; set { _ten = value; OnPropertyChanged(); } }
        private byte[] _anh;
        public byte[] Anh { get => _anh; set { _anh = value; OnPropertyChanged(); } }
        private int _gia;
        public int Gia { get => _gia; set { _gia = value; OnPropertyChanged(); } }
        private int _maLoai;
        public int MaLoai { get => _maLoai; set { _maLoai = value; OnPropertyChanged(); } }

        public SanPhamService() { }

        public SanPhamService(SanPham sp)
        {
            this.Ma = sp.Ma;
            this.Ten = sp.Ten;
            this.Anh = sp.Anh;
            this.Gia = sp.Gia;
            this.MaLoai = sp.MaLoai;
        }

        static public SanPhamService GetById(int id)
        {
            SanPham sp = DataProvider.Ins.DB.SanPham.FirstOrDefault(x => x.Ma == id);
            return sp == null ? null : new SanPhamService(sp);
        }

        static public List<SanPhamService> GetAll()
        {
            List<SanPhamService> res = new List<SanPhamService>();
            foreach (SanPham item in DataProvider.Ins.DB.SanPham.ToList())
            {
                res.Add(new SanPhamService(item));
            }
            return res;
        }
        
        public SanPhamService Create(string imageDir)
        {
            SanPham newsp = new SanPham
            {
                Ma = DataProvider.Ins.DB.SanPham.Count() == 0 ? 1 : DataProvider.Ins.DB.SanPham.Max(x => x.Ma) + 1,
                Ten = this.Ten,
                Anh = ImageToBytes(imageDir),
                Gia = this.Gia,
                MaLoai = this.MaLoai,
            };
            DataProvider.Ins.DB.SanPham.Add(newsp);
            DataProvider.Ins.DB.SaveChanges();
            return new SanPhamService(newsp);
        }

        public void Update()
        {
            SanPham sanpham = DataProvider.Ins.DB.SanPham.FirstOrDefault(x => x.Ma == this.Ma);
            sanpham.Ten = this.Ten;
            sanpham.Anh = this.Anh;
            sanpham.Gia = this.Gia;
            sanpham.MaLoai = this.MaLoai;
            DataProvider.Ins.DB.SaveChanges();
        }

        public void Delete()
        {
            SanPham sanpham = DataProvider.Ins.DB.SanPham.FirstOrDefault(x => x.Ma == this.Ma);
            DataProvider.Ins.DB.SanPham.Remove(sanpham);
            DataProvider.Ins.DB.SaveChanges();
        }

        private static byte[] ImageToBytes(string imageDir)
        {
            BitmapImage image = new BitmapImage(
                    new Uri(imageDir,
                    UriKind.Absolute));
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        internal static List<SanPhamService> GetByType(int maloai)
        {
            List<SanPhamService> res = new List<SanPhamService>();
            foreach (SanPham item in DataProvider.Ins.DB.SanPham.Where(x => x.MaLoai == maloai).ToList())
            {
                res.Add(new SanPhamService(item));
            }
            return res;
        }
    }
}
