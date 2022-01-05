using CoffeeShop.Model;
using CoffeeShop.Models;
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

        private AsyncObservableCollection<dynamic> _categories;
        private dynamic _selectedCategory;
        private AsyncObservableCollection<dynamic> _products;
        private dynamic _selectedProduct;
        private AsyncObservableCollection<dynamic> _spendMaterial;
        // Customer
        private bool _isCustomer;
        private string _customerName;
        private string _customerPhone;

        // Cart
        private bool _hasCustomer;
        private dynamic _selectedCartProduct;
        private AsyncObservableCollection<dynamic> _cartProducts;
        private int _tongTien;

        // mesage
        private string _message;
        private bool _isOpenMessageDialog;

        // check out
        private bool _isOpenShowCheckoutDialog;
        private string _showCheckoutCustomerName;
        private int _showCheckoutCustomerPoint;
        private int _showCheckoutPoint;
        private int _showCheckoutSpendPoint;
        private int _showCheckoutRealPay;
        #endregion

        #region properties
        public AsyncObservableCollection<dynamic> Categories { get => _categories; set { _categories = value; OnPropertyChanged(); } }
        public dynamic SelectedCategory
        {
            get => _selectedCategory; set
            {
                _selectedCategory = value;
                if (value != null)
                {
                    int maloai = value.Ma;
                    Products = new AsyncObservableCollection<dynamic>();
                    foreach (var item in DataProvider.Ins.DB.SanPham.Where(x => x.MaLoai == maloai))
                    {
                        if (CartProducts.Where(x=>x.MaSP==item.Ma).Count() == 0)
                        {
                            Products.Add(new
                            {
                                Ma = item.Ma,
                                Ten = item.Ten,
                                Gia = item.Gia,
                                Anh = item.Anh,
                            });
                        }
                    }
                }
                OnPropertyChanged();
            }
        }
        public AsyncObservableCollection<dynamic> Products { get => _products; set { _products = value; OnPropertyChanged(); } }
        public dynamic SelectedProduct { get => _selectedProduct; set { _selectedProduct = value; OnPropertyChanged(); } }

        // Customer
        public bool IsCustomer { get => _isCustomer; set { _isCustomer = value; OnPropertyChanged(); } }
        public string CustomerName { get => _customerName; set { _customerName = (value == null) ? null : value.ToUpper(); OnPropertyChanged(); } }
        public string CustomerPhone { get => _customerPhone; set { _customerPhone = value; IsCustomer = true; HasCustomer = false; OnPropertyChanged(); } }

        // Cart
        public bool HasCustomer { get => _hasCustomer; set { _hasCustomer = value; OnPropertyChanged(); } }
        public dynamic SelectedCartProduct { get => _selectedCartProduct; set { _selectedCartProduct = value; OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> CartProducts { get => _cartProducts; set { _cartProducts = value; 
                OnPropertyChanged(); } }
        public int TongTien { get => _tongTien; set { _tongTien = value; OnPropertyChanged(); } }

        // message
        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }
        public bool IsOpenMessageDialog { get => _isOpenMessageDialog; set { _isOpenMessageDialog = value; OnPropertyChanged(); } }

        // check out
        public bool IsOpenShowCheckoutDialog { get => _isOpenShowCheckoutDialog; set { _isOpenShowCheckoutDialog = value; OnPropertyChanged(); } }
        public string ShowCheckoutCustomerName { get => _showCheckoutCustomerName; set { _showCheckoutCustomerName = value; OnPropertyChanged(); } }
        public int ShowCheckoutCustomerPoint { get => _showCheckoutCustomerPoint; set { _showCheckoutCustomerPoint = value; OnPropertyChanged(); } }
        public int ShowCheckoutPoint { get => _showCheckoutPoint; set { _showCheckoutPoint = value; OnPropertyChanged(); } }
        public int ShowCheckoutSpendPoint { get => _showCheckoutSpendPoint; set { _showCheckoutSpendPoint = value;
                if (value <= 0)
                {
                    _showCheckoutSpendPoint = 0;
                    if (HasCustomer == true)
                    {
                        ShowCheckoutPoint = TinhDiemThuong(DataProvider.Ins.DB.KhachHang.First(x => x.SDT == CustomerPhone).Ma, TongTien);
                    }
                }
                else
                {
                    ShowCheckoutPoint = 0;
                    ShowCheckoutRealPay = TongTien - ShowCheckoutSpendPoint * 1000;
                }
                OnPropertyChanged(); } }
        public int ShowCheckoutRealPay { get => _showCheckoutRealPay; set { _showCheckoutRealPay = value; OnPropertyChanged(); } }
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

                // khởi tạo dữ liệu
                // Danh sách loại sản phẩm
                CartProducts = new AsyncObservableCollection<dynamic>();
                _spendMaterial = new AsyncObservableCollection<dynamic>();
                Categories = new AsyncObservableCollection<dynamic>();
                foreach (var item in DataProvider.Ins.DB.LoaiSanPham)
                {
                    Categories.Add(new
                    {
                        Ma = item.Ma,
                        Ten = item.Ten
                    });
                }
                SelectedCategory = Categories.ElementAt(0);

                TongTien = 0;

            });


            ChangeCategoryCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                SelectedCategory = param;
            });

            ClickCheckCustommerButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (HasCustomer == false)
                {
                    if (IsCustomer == true)
                    {
                        KhachHang khachhang = (DataProvider.Ins.DB.KhachHang.Where(x => x.SDT == CustomerPhone).Count() == 0) ? null : DataProvider.Ins.DB.KhachHang.First(x => x.SDT == CustomerPhone);
                        if (khachhang == null)
                        {
                            Message = "Khách hàng mới";
                            IsOpenMessageDialog = true;
                            IsCustomer = false;
                        }
                        else
                        {
                            CustomerName = khachhang.Ten;
                            HasCustomer = true;
                        }
                    }
                    else
                    {
                        if (CustomerName != null && CustomerName.Split(' ').Length != CustomerName.Length + 1)
                        {
                            DataProvider.Ins.DB.KhachHang.Add(new KhachHang
                            {
                                Ma = (DataProvider.Ins.DB.KhachHang.Count() == 0) ? 1 : DataProvider.Ins.DB.KhachHang.Max(x => x.Ma) + 1,
                                Ten = CustomerName,
                                SDT = CustomerPhone,
                                DiemTichLuy = 0,
                                TongChiTieu = 0,
                            });
                            DataProvider.Ins.DB.SaveChanges();
                            HasCustomer = true;
                            IsCustomer = true;
                        }
                        else
                        {
                            Message = "Vui lòng không để trống tên khách hàng";
                            IsOpenMessageDialog = true;
                        }
                    }
                }
            });

            CloseMessageDialog = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenMessageDialog = false;
            });

            AddToCartCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (CheckKho(param.Ma, 1) == true)
                {
                    CartProducts.Add(new
                    {
                        MaSP = param.Ma,
                        Ten = param.Ten,
                        Gia = param.Gia,
                        SoLuong = 1,
                        TongGia = param.Gia,
                    });
                    Products.Remove(param);
                    TongTien = TongTien + param.Gia;
                }
            });

            ClickIncreaseCartProductButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (CheckKho(param.MaSP, param.SoLuong + 1) == true)
                {
                    AsyncObservableCollection<dynamic> newcartproducts = new AsyncObservableCollection<dynamic>();
                    foreach (var item in CartProducts)
                    {
                        if (item.MaSP != param.MaSP)
                        {
                            newcartproducts.Add(item);
                        }
                        else
                        {
                            newcartproducts.Add(new
                            {
                                MaSP = param.MaSP,
                                Ten = param.Ten,
                                Gia = param.Gia,
                                SoLuong = param.SoLuong + 1,
                                TongGia = param.Gia * (param.SoLuong + 1),
                            });
                            TongTien = TongTien + param.Gia;
                        }
                        CartProducts = newcartproducts;
                    }
                }
            });

            ClickDecreaseCartProductButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                int maSP = param.MaSP;
                TongTien = TongTien - param.Gia;
                if (param.SoLuong == 1)
                {
                    SanPham sanpham = DataProvider.Ins.DB.SanPham.First(x => x.Ma == maSP);
                    if (SelectedCategory.Ma == sanpham.MaLoai)
                    {
                        Products.Add(new
                        {
                            Ma = sanpham.Ma,
                            Ten = sanpham.Ten,
                            Gia = sanpham.Gia,
                            Anh = sanpham.Anh,
                        });
                    }
                    CartProducts.Remove(param);
                }
                else
                {
                    AsyncObservableCollection<dynamic> newcartproducts = new AsyncObservableCollection<dynamic>();
                    foreach (var item in CartProducts)
                    {
                        if (item.MaSP != param.MaSP)
                        {
                            newcartproducts.Add(item);
                        }
                        else
                        {
                            newcartproducts.Add(new
                            {
                                MaSP = param.MaSP,
                                Ten = param.Ten,
                                Gia = param.Gia,
                                SoLuong = param.SoLuong - 1,
                                TongGia = param.Gia * (param.SoLuong - 1),
                            });
                        }
                        CartProducts = newcartproducts;
                    }
                }
                // trả lại số nguyên liệu hao tổn
                foreach (var item in DataProvider.Ins.DB.NguyenLieu.Where(x => x.MaSP == maSP))
                {
                    if (_spendMaterial.Where(x => x.Ma == item.MaNL).Count() != 0)
                    {
                        int soluongmoi = _spendMaterial.First(x => x.Ma == item.MaNL).SoLuong - item.SoLuong;
                        _spendMaterial.Remove(_spendMaterial.First(x => x.Ma == item.MaNL));
                        _spendMaterial.Add(new
                        {
                            Ma = item.MaNL,
                            SoLuong = soluongmoi,
                        });
                    }
                }
            });

            ClickCheckoutButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenShowCheckoutDialog = true;
                if (HasCustomer == true)
                {
                    KhachHang khachhang = DataProvider.Ins.DB.KhachHang.First(x => x.SDT == CustomerPhone);
                    ShowCheckoutCustomerName = CustomerName;
                    ShowCheckoutCustomerPoint = khachhang.DiemTichLuy;
                    ShowCheckoutPoint = TinhDiemThuong(khachhang.Ma, TongTien);
                }
                else
                {
                    ShowCheckoutCustomerName = "(Không)";
                    ShowCheckoutCustomerPoint = 0;
                    ShowCheckoutPoint = 0;
                }
                ShowCheckoutSpendPoint = 0;
                ShowCheckoutRealPay = TongTien;
            });

            CloseShowCheckoutCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                if (bool.Parse(param.ToString()) == true)
                {
                    if (ShowCheckoutSpendPoint > ShowCheckoutCustomerPoint)
                    {
                        Message = "Không đủ điểm thanh toán";
                        IsOpenMessageDialog = true;
                    }
                    else
                    {
                        // khách hàng
                        if (HasCustomer)
                        {
                            int makh = DataProvider.Ins.DB.KhachHang.First(x => x.SDT == CustomerPhone).Ma;
                            DataProvider.Ins.DB.KhachHang.First(x => x.Ma == makh).TongChiTieu += ShowCheckoutRealPay;
                            DataProvider.Ins.DB.KhachHang.First(x => x.Ma == makh).DiemTichLuy += ShowCheckoutPoint;
                            DataProvider.Ins.DB.KhachHang.First(x => x.Ma == makh).DiemTichLuy -= ShowCheckoutSpendPoint;
                            DataProvider.Ins.DB.SaveChanges();
                        }

                        // Hóa đơn
                        int mahd = (DataProvider.Ins.DB.HoaDon.Count() == 0) ? 1 : DataProvider.Ins.DB.HoaDon.Max(x => x.Ma) + 1;
                        if (HasCustomer == true)
                        {
                            DataProvider.Ins.DB.HoaDon.Add(new HoaDon
                            {
                                Ma = mahd,
                                NgayTao = DateTime.Now,
                                MaKH = DataProvider.Ins.DB.KhachHang.First(x => x.SDT == CustomerPhone).Ma,
                                DiemTichLuy = ShowCheckoutPoint - ShowCheckoutSpendPoint,
                                TongTien = ShowCheckoutRealPay,
                            });
                        }
                        else
                        {
                            DataProvider.Ins.DB.HoaDon.Add(new HoaDon
                            {
                                Ma = mahd,
                                NgayTao = DateTime.Now,
                                TongTien = ShowCheckoutRealPay,
                            });
                        }
                        DataProvider.Ins.DB.SaveChanges();

                        // chi tiet hoa don
                        foreach (var item in CartProducts)
                        {
                            int macthd = (DataProvider.Ins.DB.ChiTietHoaDon.Where(x => x.MaHD == mahd).Count() == 0) ? 1 : DataProvider.Ins.DB.ChiTietHoaDon.Where(x => x.MaHD == mahd).Max(x => x.Ma) + 1;
                            DataProvider.Ins.DB.ChiTietHoaDon.Add(new ChiTietHoaDon
                            {
                                Ma = macthd,
                                MaHD = mahd,
                                TenSP = item.Ten,
                                SoLuong = item.SoLuong,
                                GiaSP = item.Gia,
                            });
                            DataProvider.Ins.DB.SaveChanges();
                        }

                        // Kho nguyên liệu
                        foreach (var item in _spendMaterial)
                        {
                            int maNL = item.Ma;
                            DataProvider.Ins.DB.KhoNguyenLieu.First(x => x.MaNL == maNL).SoLuong -= item.SoLuong;
                        }
                        DataProvider.Ins.DB.SaveChanges();

                        CustomerPhone = null;
                        CustomerName = null;
                        CartProducts = new AsyncObservableCollection<dynamic>();
                        SelectedCategory = SelectedCategory;
                        IsOpenShowCheckoutDialog = false;
                        TongTien = 0;
                    }
                }
                else
                {
                    IsOpenShowCheckoutDialog = false;
                }
            });

        }

        private bool CheckKho(int maSP, int soluong)
        {
            bool result = true;
            // cập nhật số nguyên liệu hao tổn
            foreach (var item in DataProvider.Ins.DB.NguyenLieu.Where(x=>x.MaSP==maSP))
            {
                if (_spendMaterial.Where(x=>x.Ma==item.MaNL).Count() != 0)
                {
                    int soluongmoi = _spendMaterial.First(x => x.Ma == item.MaNL).SoLuong + item.SoLuong;
                    _spendMaterial.Remove(_spendMaterial.First(x => x.Ma == item.MaNL));
                    _spendMaterial.Add(new { 
                        Ma = item.MaNL,
                        SoLuong = soluongmoi,
                    });
                }
                else
                {
                    _spendMaterial.Add(new
                    {
                        Ma = item.MaNL,
                        SoLuong = item.SoLuong,
                    });
                }
            }
            // kiểm tra kho nguyên liệu
            foreach (var item in _spendMaterial)
            {
                int maNL = item.Ma;
                if (DataProvider.Ins.DB.KhoNguyenLieu.First(x=>x.MaNL== maNL).SoLuong < item.SoLuong)
                {
                    result = false;
                    break;
                }
            }

            if (result == false)
            {
                // trả lại số nguyên liệu hao tổn
                foreach (var item in DataProvider.Ins.DB.NguyenLieu.Where(x => x.MaSP == maSP))
                {
                    if (_spendMaterial.Where(x => x.Ma == item.MaNL).Count() != 0)
                    {
                        int soluongmoi = _spendMaterial.First(x => x.Ma == item.MaNL).SoLuong - item.SoLuong;
                        _spendMaterial.Remove(_spendMaterial.First(x => x.Ma == item.MaNL));
                        _spendMaterial.Add(new
                        {
                            Ma = item.MaNL,
                            SoLuong = soluongmoi,
                        });
                    }
                }
                Message = "Kho nguyên liệu không đủ để chế biến món này";
                IsOpenMessageDialog = true;
            }
            return result;
        }

        private int TinhDiemThuong(int maKH, int SoTien)
        {
            int result = 0;
            KhachHang khachhang = DataProvider.Ins.DB.KhachHang.First(x => x.Ma == maKH);
            if (khachhang.TongChiTieu >= int.Parse(DataProvider.Ins.DB.ThongSo.First(x => x.Ten == "HanMucChiTieuKHThuong").GiaTri))
            {
                float TiLeDoi = float.Parse(DataProvider.Ins.DB.ThongSo.First(x => x.Ten == "TiLeDoiDiemKHThuong").GiaTri);
                float diem = TiLeDoi * TongTien;
                result = (int)Math.Floor(diem);
            }
            if (khachhang.TongChiTieu >= int.Parse(DataProvider.Ins.DB.ThongSo.First(x => x.Ten == "HanMucChiTieuKHThuongXuyen").GiaTri))
            {
                float TiLeDoi = float.Parse(DataProvider.Ins.DB.ThongSo.First(x => x.Ten == "TiLeDoiDiemKHThuongXuyen").GiaTri);
                float diem = TiLeDoi * TongTien;
                result = (int)Math.Floor(diem);
            }
            if (khachhang.TongChiTieu >= int.Parse(DataProvider.Ins.DB.ThongSo.First(x => x.Ten == "HanMucChiTieuKHThanThiet").GiaTri))
            {
                float TiLeDoi = float.Parse(DataProvider.Ins.DB.ThongSo.First(x => x.Ten == "TiLeDoiDiemKHThanThiet").GiaTri);
                float diem = TiLeDoi * TongTien;
                result = (int)Math.Floor(diem);
            }
            return result;
        }

    }
}
