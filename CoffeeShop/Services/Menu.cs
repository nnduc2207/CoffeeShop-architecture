using CoffeeShop.Models;
using CoffeeShop.Services.ModelServices;
using CoffeeShop.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Services
{
    public class Menu : NotifyPropertyChanged
    {
        static private AsyncObservableCollection<LoaiSanPhamService> _categoryList;
        static private Dictionary<int, AsyncObservableCollection<SanPhamService>> _productListByType;

        private Dictionary<int, SanPhamService> _hideProductList;

        private int _currentCategory;
        private AsyncObservableCollection<SanPhamService> _currentMenu;

        public AsyncObservableCollection<SanPhamService> CurrentMenu
        {
            get => _currentMenu;
            set {
                _currentMenu = value;
                OnPropertyChanged();
            }
        }
        public AsyncObservableCollection<LoaiSanPhamService> CategoryList { get => _categoryList; }
        static Menu()
        {
            _categoryList = new AsyncObservableCollection<LoaiSanPhamService>();
            _productListByType = new Dictionary<int, AsyncObservableCollection<SanPhamService>>();
            LoaiSanPhamService tatca = new LoaiSanPhamService();
            tatca.Ma = 0;
            tatca.Ten = "Tất cả";
            _categoryList.Add(tatca);
            _productListByType.Add(0, new AsyncObservableCollection<SanPhamService>());
            foreach (LoaiSanPhamService item in LoaiSanPhamService.GetAll())
            {
                _categoryList.Add(item);
                _productListByType.Add(item.Ma, new AsyncObservableCollection<SanPhamService>());
            }
            foreach (SanPhamService item in SanPhamService.GetAll())
            {
                _productListByType[0].Add(item);
                _productListByType[item.MaLoai].Add(item);
            }
        }
        public Menu()
        {
            _hideProductList = new Dictionary<int, SanPhamService>();
        }
        public void ChangeProductListByType(int typeId)
        {
            _currentCategory = typeId;
            LoadProductList();
        }
        public void HideProduct(dynamic product)
        {
            CurrentMenu.Remove(product);
            _hideProductList.Add(product.Ma, product);
        }
        public void ShowProduct(dynamic product)
        {
            _hideProductList.Remove(product.Ma);
            LoadProductList();
        }
        protected void LoadProductList()
        {
            CurrentMenu = new AsyncObservableCollection<SanPhamService>();
            foreach (SanPhamService item in _productListByType[_currentCategory])
            {
                if (!_hideProductList.ContainsKey(item.Ma))
                {
                    CurrentMenu.Add(item);
                }
            }
        }
    }
}
