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
    public class Menu : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        static private AsyncObservableCollection<dynamic> _categoryList;
        static private Dictionary<int, AsyncObservableCollection<dynamic>> _productListByType;

        private Dictionary<int, dynamic> _hideProductList;

        private int _currentCategory;
        private AsyncObservableCollection<dynamic> _currentMenu;

        public AsyncObservableCollection<dynamic> CurrentMenu
        {
            get => _currentMenu;
            set {
                _currentMenu = value;
                NotifyPropertyChanged("CurrentMenu");
            }
        }
        public AsyncObservableCollection<dynamic> CategoryList { get => _categoryList; }
        static Menu()
        {
            _categoryList = new AsyncObservableCollection<dynamic>();
            _productListByType = new Dictionary<int, AsyncObservableCollection<dynamic>>();
            _categoryList.Add(new
            {
                Ma = 0,
                Ten = "Tất cả",
            });
            _productListByType.Add(0, new AsyncObservableCollection<dynamic>());
            foreach (LoaiSanPham item in LoaiSanPhamService.GetAll())
            {
                _categoryList.Add(new
                {
                    Ma = item.Ma,
                    Ten = item.Ten,
                });
                _productListByType.Add(item.Ma, new AsyncObservableCollection<dynamic>());
            }
            foreach (SanPham item in SanPhamService.GetAll())
            {
                _productListByType[0].Add(new
                {
                    Ma = item.Ma,
                    Ten = item.Ten,
                    Gia = item.Gia,
                    Anh = item.Anh,
                });
                _productListByType[item.MaLoai].Add(new
                {
                    Ma = item.Ma,
                    Ten = item.Ten,
                    Gia = item.Gia,
                    Anh = item.Anh,
                });
            }
        }
        public Menu()
        {
            _hideProductList = new Dictionary<int, dynamic>();
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
            CurrentMenu = new AsyncObservableCollection<dynamic>();
            foreach (dynamic item in _productListByType[_currentCategory])
            {
                if (!_hideProductList.ContainsKey(item.Ma))
                {
                    CurrentMenu.Add(item);
                }
            }
        }
    }
}
