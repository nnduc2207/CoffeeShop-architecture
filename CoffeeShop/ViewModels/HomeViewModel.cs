using CoffeeShop.Model;
using CoffeeShop.Models;
using CoffeeShop.Services;
using CoffeeShop.Services.ModelServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoffeeShop.ViewModels
{
    public class HomeViewModel:BaseViewModel
    {
        #region variables
        private static HomeViewModel _instance = null;
        private static object m_lock = new object();

        private Menu _menu;
        private LoaiSanPhamService _selectedCategory;
        private LoaiSanPhamService _selectedProduct;
        private string _selectedSort;

        // Customer
        private bool _isCustomer;
        private string _customerName;
        private string _customerPhone;

        // Cart
        private bool _hasCustomer;
        private dynamic _selectedCartProduct;
        private Cart _myCart;

        // mesage
        private string _message;
        private bool _isOpenMessageDialog;

        // check out
        private bool _isOpenShowCheckoutDialog;
        #endregion

        #region properties
        public Menu Menu { get => _menu; set { _menu = value; OnPropertyChanged(); } }
        public LoaiSanPhamService SelectedCategory
        {
            get => _selectedCategory; set
            {
                _selectedCategory = value;
                if (value != null)
                {
                    Menu.ChangeProductListByType(value.Ma);
                }
                OnPropertyChanged();
            }
        }
        public LoaiSanPhamService SelectedProduct { get => _selectedProduct; set { _selectedProduct = value; OnPropertyChanged(); } }
        public string SelectedSort { get => _selectedSort; set { _selectedSort = value; Menu.CurrentSort = Menu.Sorts[value]; OnPropertyChanged(); } }
 
        // Customer
        public bool IsCustomer { get => _isCustomer; set { _isCustomer = value; OnPropertyChanged(); } }
        public string CustomerName { get => _customerName; set { _customerName = (value == null) ? null : value.ToUpper(); OnPropertyChanged(); } }
        public string CustomerPhone { 
            get => _customerPhone; 
            set { 
                _customerPhone = value; 
                IsCustomer = true; 
                HasCustomer = false;
                CustomerName = null;
                OnPropertyChanged(); 
            } 
        }

        // Cart
        public bool HasCustomer { get => _hasCustomer; set { _hasCustomer = value; OnPropertyChanged(); } }
        public dynamic SelectedCartProduct { get => _selectedCartProduct; set { _selectedCartProduct = value; OnPropertyChanged(); } }
        public Cart MyCart { get => _myCart; set { _myCart = value; OnPropertyChanged(); } }

        // message
        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }
        public bool IsOpenMessageDialog { get => _isOpenMessageDialog; set { _isOpenMessageDialog = value; OnPropertyChanged(); } }

        // check out
        public bool IsOpenShowCheckoutDialog { get => _isOpenShowCheckoutDialog; set { _isOpenShowCheckoutDialog = value; OnPropertyChanged(); } }
        #endregion

        #region command
        public ICommand Loaded { get; set; }
        public ICommand ChangeCategoryCommand { get; set; }
        public ICommand ClickCheckCustommerButtonCommand { get; set; }
        public ICommand CloseMessageDialog { get; set; }
        public ICommand AddToCartCommand { get; set; }
        public ICommand ClickDecreaseCartProductButtonCommand { get; set; }
        public ICommand ClickIncreaseCartProductButtonCommand { get; set; }
        public ICommand ClickCheckoutButtonCommand { get; set; }
        public ICommand CloseShowCheckoutCommand { get; set; }
        public ICommand ClickCurrentEventButtonCommand { get; set; }

        #endregion

        public static HomeViewModel GetInstance()
        {
            // DoubleLock
            if (_instance == null)
            {
                lock (m_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new HomeViewModel();
                    }
                }
            }
            return _instance;
        }

        public HomeViewModel()
        {
            // Command
            Loaded = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                Menu = new Menu();
                SelectedCategory = Menu.CategoryList[0];
                // khởi tạo dữ liệu
                MyCart = Cart.GetInstance();
                HasCustomer = false;
                IsCustomer = true;
            });

            ChangeCategoryCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                SelectedCategory = param;
            });

            ClickCheckCustommerButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (HasCustomer == false)
                {
                    if (IsCustomer == true)
                    {
                        KhachHangService khachhang = KhachHangService.GetByPhone(CustomerPhone);
                        if (khachhang == null)
                        {
                            Alert("Khách hàng mới");
                            IsCustomer = false;
                        }
                        else
                        {
                            CustomerName = khachhang.Ten;
                            MyCart.Customer = khachhang;
                            HasCustomer = true;
                        }
                    }
                    else
                    {
                        if (CustomerName != null && CustomerName.Split(' ').Length != CustomerName.Length + 1)
                        {
                            MyCart.Customer = new KhachHangService();
                            MyCart.Customer.Ten = CustomerName;
                            MyCart.Customer.SDT = CustomerPhone;
                            MyCart.Customer = MyCart.Customer.Create();
                            HasCustomer = true;
                            IsCustomer = true;
                        }
                        else
                        {
                            Alert("Vui lòng không để trống tên khách hàng");
                        }
                    }
                }
            });

            CloseMessageDialog = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenMessageDialog = false;
            });

            AddToCartCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (MyCart.AddProduct(param.Ma) == false)
                {
                    Alert("Kho nguyên liệu không đủ để chế biến món này");
                }
                else
                {
                    Menu.HideProduct(param);
                }
            });

            ClickIncreaseCartProductButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (MyCart.AddProduct(param.Ma) == false)
                {
                    Alert("Kho nguyên liệu không đủ để chế biến món này");
                }
            });

            ClickDecreaseCartProductButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (param.SoLuong == 1)
                {
                    Menu.ShowProduct(param);
                }
                MyCart.RemoveProduct(param.Ma);
            });

            ClickCheckoutButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenShowCheckoutDialog = true;
            });

            CloseShowCheckoutCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                if (bool.Parse(param.ToString()) == true)
                {
                    try
                    {
                        MyCart.Checkout();
                    }
                    catch (Exception e)
                    {
                        Alert(e.Message);
                    }
                    MyCart = Cart.GetInstance(true);
                    CustomerPhone = null;
                    CustomerName = null;
                    Alert("thanh toán thành công");
                }
                IsOpenShowCheckoutDialog = false;
            });

            ClickCurrentEventButtonCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                string[] eventArray = EventManager.AllCurrentEventList().ToArray();
                if (eventArray.Length == 0)
                {
                    Alert("No event currently");
                }
                else
                {
                    Alert("List events:\n" + String.Join("\n", eventArray));
                }
            });
        }

        private void Alert(string message)
        {
            Message = message;
            IsOpenMessageDialog = true;
        }

    }
}
