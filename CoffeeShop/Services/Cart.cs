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
    public class Cart : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        static private Cart _instance = null;
        private KhachHangService _customer = new KhachHangService();
        private List<int> _productIdList = new List<int>();
        private Dictionary<int, int> _productCount = new Dictionary<int, int>();
        private Dictionary<int, int> _spendMaterialDic = new Dictionary<int, int>();
        private AsyncObservableCollection<dynamic> _productList = new AsyncObservableCollection<dynamic>();
        private AsyncObservableCollection<dynamic> _giveawayList = new AsyncObservableCollection<dynamic>();
        private int _total = 0;
        private int _point = 0;
        private int _spendPoint = 0;
        private int _realPayment = 0;

        public KhachHangService Customer { get => _customer; set { _customer = value; Point = CalculatePoint(); NotifyPropertyChanged("Customer"); } }
        public AsyncObservableCollection<dynamic> ProductList
        {
            get => _productList;
            set
            {
                _productList = value;
                NotifyPropertyChanged("ProductList");
            }
        }
        public int Total { get => _total; set { 
                _total = value; 
                RealPayment = value; 
                Point = CalculatePoint(); 
                NotifyPropertyChanged("Total"); } }
        public int Point { get => _point; set { _point = value; NotifyPropertyChanged("Point"); } }
        public int SpendPoint { get => _spendPoint; set {
                if (value < 0)
                {
                    value = 0;
                }
                _spendPoint = value;
                RealPayment -= value * 1000;
                if (value > 0)
                {
                    Point = 0;
                }
                NotifyPropertyChanged("SpendPoint"); } }
        public int RealPayment { get => _realPayment; set { _realPayment = value; NotifyPropertyChanged("RealPayment"); } }

        private Cart()
        {
        }

        static public Cart GetInstance(bool isNew = false)
        {
            if (isNew || _instance == null)
            {
                _instance = new Cart();
            }
            return _instance;
        }

        public bool AddProduct(int maSP)
        {
            if (!IsEnoughMaterial(maSP))
            {
                return false;
            }

            if (!_productCount.ContainsKey(maSP))
            {
                _productIdList.Add(maSP);
                _productCount.Add(maSP, 1);
                SanPham sanpham = SanPhamService.GetById(maSP);
                ProductList.Add(new
                {
                    Ma = sanpham.Ma,
                    Ten = sanpham.Ten,
                    Gia = sanpham.Gia,
                    SoLuong = 1,
                    TongGia = sanpham.Gia,
                });
            }
            else
            {
                _productCount[maSP]++;
                LoadProductList();
            }
            Total += SanPhamService.GetById(maSP).Gia;
            return true;
        }

        private int CalculatePoint()
        {
            if (Customer.Ma == 0)
            {
                return 0;
            }
            return Customer.LoaiKhachHang.TinhDiemThuong(Total);
        }

        public bool RemoveProduct(int maSP)
        {
            if (!_productCount.ContainsKey(maSP) || _productCount[maSP] < 1)
            {
                return false;
            }

            _productCount[maSP]--;
            LoadProductList();
            CalculateReturnMaterial(maSP);
            Total -= SanPhamService.GetById(maSP).Gia;
            return true;
        }

        private void CalculateReturnMaterial(int maSP)
        {
            foreach (NguyenLieu item in NguyenLieuService.GetByProduct(maSP))
            {
                _spendMaterialDic[item.MaNL] -= item.SoLuong;
            }
        }

        private void LoadProductList()
        {
            ProductList = new AsyncObservableCollection<dynamic>();
            foreach (int ma in _productIdList)
            {
                if (_productCount[ma] != 0)
                {
                    SanPham sanpham = SanPhamService.GetById(ma);
                    ProductList.Add(new
                    {
                        Ma = sanpham.Ma,
                        Ten = sanpham.Ten,
                        Gia = sanpham.Gia,
                        SoLuong = _productCount[ma],
                        TongGia = sanpham.Gia * _productCount[ma],
                    });
                }
            }
        }

        private bool IsEnoughMaterial(int maSP)
        {
            return CalculateSpendMaterial(0, NguyenLieuService.GetByProduct(maSP));
        }

        private bool CalculateSpendMaterial(int v, List<NguyenLieu> nguyenLieus)
        {
            // Dieu kien dung de qui
            if (v == nguyenLieus.Count())
            {
                return true;
            }

            // Cap nhat so nguyen lieu hao ton
            if (_spendMaterialDic.ContainsKey(nguyenLieus[v].MaNL))
            {
                _spendMaterialDic[nguyenLieus[v].MaNL] += nguyenLieus[v].SoLuong;
            }
            else
            {
                _spendMaterialDic.Add(nguyenLieus[v].MaNL, nguyenLieus[v].SoLuong);
            }

            // Kiemr tra kho nguyen lieu
            if (_spendMaterialDic[nguyenLieus[v].MaNL] > nguyenLieus[v].KhoNguyenLieu.SoLuong)
            {
                return false;
            }

            // de qui
            bool res = CalculateSpendMaterial(v + 1, nguyenLieus);

            // tra lai nguyen lieu neu that bai
            if (res == false)
            {
                _spendMaterialDic[nguyenLieus[v].MaNL] -= nguyenLieus[v].SoLuong;
            }
            return res;
        }

        public void Checkout()
        {
            if (SpendPoint > Customer.DiemTichLuy)
            {
                throw new Exception("Không đủ điểm thanh toán");
            }
            if (Customer.Ma != 0)
            {
                KhachHang kh = KhachHangService.GetById(Customer.Ma);
                kh.TongChiTieu += RealPayment;
                kh.DiemTichLuy += Point - SpendPoint;
                KhachHangService.Update();
            }
            HoaDon hd = HoaDonService.Create(RealPayment, Point, Customer.Ma);
            foreach (int id in _productIdList)
            {
                if (_productCount.ContainsKey(id) && _productCount[id] > 0)
                {
                    SanPham sp = SanPhamService.GetById(id);
                    ChiTietHoaDonService.Create(hd.Ma, sp.Ten, _productCount[id], sp.Gia);
                }
            }
            foreach (KhoNguyenLieu nl in KhoNguyenLieuService.GetAll())
            {
                if (_spendMaterialDic.ContainsKey(nl.MaNL))
                {
                    nl.SoLuong -= _spendMaterialDic[nl.MaNL];
                }
            }
            KhoNguyenLieuService.Update();
        }
    }
}
