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
    public class SanPhamService
    {
        static public SanPham GetById(int id)
        {
            return DataProvider.Ins.DB.SanPham.FirstOrDefault(x => x.Ma == id);
        }

        static public List<SanPham> GetAll()
        {
            return DataProvider.Ins.DB.SanPham.ToList();
        }
        
        static public SanPham Create(string name, string imageDir, int price, int typeid)
        {
            SanPham newsp = new SanPham
            {
                Ma = DataProvider.Ins.DB.SanPham.Count() == 0 ? 1 : DataProvider.Ins.DB.SanPham.Max(x => x.Ma) + 1,
                Ten = name,
                Anh = ImageToBytes(imageDir),
                Gia = price,
                MaLoai = typeid,
            };
            DataProvider.Ins.DB.SanPham.Add(newsp);
            DataProvider.Ins.DB.SaveChanges();
            return newsp;
        }

        static public void Update()
        {
            DataProvider.Ins.DB.SaveChanges();
        }

        static public void Delete(SanPham sanpham)
        {
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

        internal static List<SanPham> GetByType(int maloai)
        {
            return DataProvider.Ins.DB.SanPham.Where(x => x.MaLoai == maloai).ToList();
        }
    }
}
