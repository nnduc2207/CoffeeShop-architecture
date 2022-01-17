using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Model;
using CoffeeShop.Models;

namespace CoffeeShop.Services.ModelServices
{
    public class LoaiSanPhamService : NotifyPropertyChanged
    {
        private int _ma;
        public int Ma { get => _ma; set { _ma = value; OnPropertyChanged(); } }
        private string _ten;
        public string Ten { get => _ten; set { _ten = value; OnPropertyChanged(); } }

        public LoaiSanPhamService() { }

        public LoaiSanPhamService(LoaiSanPham lsp)
        {
            this.Ma = lsp.Ma;
            this.Ten = lsp.Ten;
        }

        static public LoaiSanPhamService GetById(int id)
        {
            LoaiSanPham lsp = DataProvider.Ins.DB.LoaiSanPham.FirstOrDefault(x => x.Ma == id);
            return lsp == null ? null : new LoaiSanPhamService(lsp);
        }

        static public LoaiSanPhamService GetByName(string name)
        {
            LoaiSanPham lsp = DataProvider.Ins.DB.LoaiSanPham.FirstOrDefault(x => x.Ten == name);
            return lsp == null ? null : new LoaiSanPhamService(lsp);
        }
        static public List<LoaiSanPhamService> GetAll()
        {
            List<LoaiSanPhamService> res = new List<LoaiSanPhamService>();
            foreach (LoaiSanPham item in DataProvider.Ins.DB.LoaiSanPham.ToList())
            {
                res.Add(new LoaiSanPhamService(item));
            }
            return res;
        }

        public LoaiSanPhamService Create()
        {
            LoaiSanPham newlsp = new LoaiSanPham
            {
                Ma = DataProvider.Ins.DB.LoaiSanPham.Count() == 0 ? 1 : DataProvider.Ins.DB.LoaiSanPham.Max(x => x.Ma) + 1,
                Ten = this.Ten,
            };
            DataProvider.Ins.DB.LoaiSanPham.Add(newlsp);
            DataProvider.Ins.DB.SaveChanges();
            return new LoaiSanPhamService(newlsp);
        }

        public void Delete()
        {
            LoaiSanPham lsp = DataProvider.Ins.DB.LoaiSanPham.FirstOrDefault(x => x.Ma == this.Ma);
            DataProvider.Ins.DB.LoaiSanPham.Remove(lsp);
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
