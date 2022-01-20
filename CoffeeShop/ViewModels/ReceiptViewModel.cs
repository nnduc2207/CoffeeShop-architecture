using CoffeeShop.Model;
using CoffeeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoffeeShop.Services.ModelServices;

namespace CoffeeShop.ViewModels
{
    public class ReceiptViewModel : BaseViewModel
    {
        #region variables
        private static ReceiptViewModel _instance = null;
        private static object m_lock = new object();

        private AsyncObservableCollection<dynamic> _receipts;
        private dynamic _selectedReceipt;

        // search
        private string _search;

        // Receipt detail
        private bool _isOpenShowReceiptDialog;

        private string _showCustomerName;
        private string _showCustomerPhone;
        private DateTime _showReceiptDate;
        private int _showReceiptTotal;
        private int _showReceiptPoint;
        private int _showReceiptRealPay;

        // mesage
        private string _message;
        private bool _isOpenMessageDialog;

        #endregion

        #region properties
        public AsyncObservableCollection<dynamic> Receipts { get => _receipts; set { _receipts = value; OnPropertyChanged(); } }
        public dynamic SelectedReceipt { get => _selectedReceipt; set { _selectedReceipt = value;
                if (value != null)
                {
                    IsOpenShowReceiptDialog = true;
                    int maHD = SelectedReceipt.Ma;
                    HoaDonService hoaDon = HoaDonService.GetById(maHD);
                    KhachHangService khachHang = KhachHangService.GetById(hoaDon.MaKH);
                    List<ChiTietHoaDonService> chiTietHoaDon = ChiTietHoaDonService.GetByHoaDon(maHD);

                    ShowCustomerName = (hoaDon.MaKH == 0) ? "<empty>" : khachHang.Ten;
                    ShowCustomerPhone = SelectedReceipt.SDT;
                    ShowReceiptDate = SelectedReceipt.NgayTao;
                    ShowReceiptPoint = hoaDon.DiemTichLuy;
                    ShowReceiptTotal = 0;
                    ShowReceiptProducts = new AsyncObservableCollection<dynamic>();
                    foreach (var item in chiTietHoaDon)
                    {
                        ShowReceiptProducts.Add(new { 
                            Ten = item.TenSP,
                            Gia = item.GiaSP,
                            SoLuong = item.SoLuong,
                            TongGia = item.GiaSP * item.SoLuong,
                        });
                        ShowReceiptTotal += item.GiaSP * item.SoLuong;
                    }
                    ShowReceiptRealPay = ShowReceiptTotal + ((ShowReceiptPoint < 0) ? ShowReceiptPoint : 0) * 1000;
                }
                OnPropertyChanged(); } }

        //search
        public string Search
        {
            get => _search; set
            {
                _search = value;
                LoadAllReceipt();
                foreach (dynamic item in Receipts.ToList())
                {
                    string sdt = item.SDT;
                    if (sdt.Contains(Search) == false)
                    {
                        Receipts.Remove(item);
                    }
                }
                OnPropertyChanged();
            }
        }
        private void LoadAllReceipt()
        {
            Receipts = new AsyncObservableCollection<dynamic>();
            List<HoaDonService> allReceipts = HoaDonService.GetAll();
            foreach (var item in allReceipts)
            {
                KhachHangService khachHang = KhachHangService.GetById(item.MaKH);
                Receipts.Add(new
                {
                    Ma = item.Ma,
                    NgayTao = item.NgayTao,
                    SDT = (item.MaKH == 0) ? "<empty>" : khachHang.SDT,
                    Diem = item.DiemTichLuy,
                    TongTien = item.TongTien,
                });
            }
        }

        // Receipt detail
        public bool IsOpenShowReceiptDialog { get => _isOpenShowReceiptDialog; set { _isOpenShowReceiptDialog = value; OnPropertyChanged(); } }

        public string ShowCustomerName { get => _showCustomerName; set { _showCustomerName = value; OnPropertyChanged(); } }
        public string ShowCustomerPhone { get => _showCustomerPhone; set { _showCustomerPhone = value; OnPropertyChanged(); } }
        public DateTime ShowReceiptDate { get => _showReceiptDate; set { _showReceiptDate = value; OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> ShowReceiptProducts { get => _receipts; set { _receipts = value; OnPropertyChanged(); } }
        public int ShowReceiptTotal { get => _showReceiptTotal; set { _showReceiptTotal = value; OnPropertyChanged(); } }
        public int ShowReceiptPoint { get => _showReceiptPoint; set { _showReceiptPoint = value; OnPropertyChanged(); } }
        public int ShowReceiptRealPay { get => _showReceiptRealPay; set { _showReceiptRealPay = value; OnPropertyChanged(); } }

        // message
        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }
        public bool IsOpenMessageDialog { get => _isOpenMessageDialog; set { _isOpenMessageDialog = value; OnPropertyChanged(); } }

        #endregion

        #region Commands
        public ICommand Loaded { get; set; }
        public ICommand CloseShowReceiptCommand { get; set; }
        public ICommand CloseMessageDialog { get; set; }

        #endregion

        public static ReceiptViewModel GetInstance()
        {
            // DoubleLock
            if (_instance == null)
            {
                lock (m_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ReceiptViewModel();
                    }
                }
            }
            return _instance;
        }

        public ReceiptViewModel()
        {

            //Command
            Loaded = new RelayCommand<dynamic>((param) => { return true; }, (param) => {

                // Setup dữ liệu
                LoadAllReceipt();
            });


            CloseShowReceiptCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenShowReceiptDialog = false;
                SelectedReceipt = null;
            });

            CloseMessageDialog = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenMessageDialog = false;
            });



        }
    }
}
