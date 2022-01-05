using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Configuration;
using CoffeeShop.Model;

namespace CoffeeShop.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        #region private variables
        //private BaseViewModel _currentPageViewModel = null;
        //private Visibility _leftPanelVisibility = Visibility.Visible;
        private String _addPlaceColor = Brushes.White.ToString();
        private String _addMemberColor = Brushes.White.ToString();
        private String _settingColor = Brushes.White.ToString();
        private String _aboutColor = Brushes.White.ToString();
        private String _versionTextBlock = Brushes.White.ToString();
        private string _nextPageVM;

        // mesage
        private string _message;
        private bool _isOpenMessageDialog;

        // enter password
        private bool _isOpenEnterPasswordDialog;
        private string _password;
        #endregion

        #region properties

        // message
        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }
        public bool IsOpenMessageDialog { get => _isOpenMessageDialog; set { _isOpenMessageDialog = value; OnPropertyChanged(); } }

        // enter password
        public bool IsOpenEnterPasswordDialog { get => _isOpenEnterPasswordDialog; set { _isOpenEnterPasswordDialog = value;
                if (value == true)
                {
                    Password = null;
                }
                OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value;  OnPropertyChanged(); } }

        #endregion

        #region Commands

        public ICommand HomeCommand { get; set; }
        public ICommand WarehouseCommand { get; set; }
        public ICommand ProductCommand { get; set; }
        public ICommand ReceiptCommand { get; set; }
        public ICommand StatisticCommand { get; set; }
        public ICommand SettingCommand { get; set; }

        public ICommand CloseMessageDialog { get; set; }
        public ICommand CloseEnterPasswordDialog { get; set; }

        #endregion

        #region Panel

        public String VersionTextBlock { get => _versionTextBlock; set { _versionTextBlock = value; OnPropertyChanged(); } }

        public Global global = Global.GetInstance();

        #endregion

        private string GetPublishedVersion()
        {
            var version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            string appVersion = $"{version.Major}.{version.Minor}";
            return appVersion;
        }


        #region static variable

        public static bool IsShowed = false;

        #endregion

        public MainViewModel()
        {
            if (DateTime.Now.Year - int.Parse(DataProvider.Ins.DB.ThongSo.First(x => x.Ten == "LanCuoiResetChiTieu").GiaTri) >= int.Parse(DataProvider.Ins.DB.ThongSo.First(x => x.Ten == "KyHanResetChiTieu").GiaTri))
            {
                foreach (var item in DataProvider.Ins.DB.KhachHang)
                {
                    item.TongChiTieu = 0;
                    DataProvider.Ins.DB.SaveChanges();
                }
                DataProvider.Ins.DB.ThongSo.First(x => x.Ten == "LanCuoiResetChiTieu").GiaTri = DateTime.Now.Year.ToString();
                DataProvider.Ins.DB.SaveChanges();
            };
            ResetPanelColor();
            global.HomeColor = "#3e2723";
            global.HomeTextColor = Brushes.SandyBrown.ToString();

            VersionTextBlock = GetPublishedVersion();
            if (VersionTextBlock == null || VersionTextBlock == "")
                VersionTextBlock = "not installed";

            HomeCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                ResetPanelColor();
                global.HomeColor = "#3e2723";
                global.CurrentPageViewModel = HomeViewModel.GetInstance();

            });

            WarehouseCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                _nextPageVM = "WarehouseVM";
                IsOpenEnterPasswordDialog = true;
            });

            ProductCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                _nextPageVM = "ProductVM";
                IsOpenEnterPasswordDialog = true;
            });

            ReceiptCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                _nextPageVM = "ReceiptVM";
                IsOpenEnterPasswordDialog = true;
            });

            StatisticCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                _nextPageVM = "StatisticVM";
                IsOpenEnterPasswordDialog = true;
            });

            SettingCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                _nextPageVM = "SettingVM";
                IsOpenEnterPasswordDialog = true;
            });

            CloseMessageDialog = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenMessageDialog = false;
            });

            CloseEnterPasswordDialog = new RelayCommand<object>((param) => { return true; }, (param) => {
                if (bool.Parse(param.ToString()) == true)
                {
                    if (Password == DataProvider.Ins.DB.ThongSo.First(x=>x.Ten == "MaBaoMat").GiaTri)
                    {
                        IsOpenEnterPasswordDialog = false;
                        switch (_nextPageVM)
                        {
                            case "WarehouseVM":
                                ResetPanelColor();
                                global.WarehouseColor = "#3e2723";
                                global.CurrentPageViewModel = WarehouseViewModel.GetInstance();
                                break;
                            case "ProductVM":
                                ResetPanelColor();
                                global.ProductColor = "#3e2723";
                                global.CurrentPageViewModel = ProductViewModel.GetInstance();
                                break;
                            case "ReceiptVM":
                                ResetPanelColor();
                                global.ReceiptColor = "#3e2723";
                                global.CurrentPageViewModel = ReceiptViewModel.GetInstance();
                                break;
                            case "StatisticVM":
                                ResetPanelColor();
                                global.StatisticColor = "#3e2723";
                                global.CurrentPageViewModel = StatisticViewModel.GetInstance();
                                break;
                            case "SettingVM":
                                ResetPanelColor();
                                global.SettingColor = "#3e2723";
                                global.CurrentPageViewModel = SettingViewModel.GetInstance();
                                break;
                        }
                    }
                    else
                    {
                        Message = "Mật khẩu sai";
                        IsOpenMessageDialog = true;
                    }
                }
                else
                {
                    IsOpenEnterPasswordDialog = false;
                }
            });
        }

        void ResetPanelColor()
        {
            global.HomeColor = Brushes.SaddleBrown.ToString();
            global.HomeTextColor = Brushes.SandyBrown.ToString();

            global.WarehouseColor = Brushes.SaddleBrown.ToString();
            global.WarehouseTextColor = Brushes.SandyBrown.ToString();

            global.ProductColor = Brushes.SaddleBrown.ToString();
            global.ProductTextColor = Brushes.SandyBrown.ToString();

            global.ReceiptColor = Brushes.SaddleBrown.ToString();
            global.ReceiptTextColor = Brushes.SandyBrown.ToString();

            global.StatisticColor = Brushes.SaddleBrown.ToString();
            global.StatisticTextColor = Brushes.SandyBrown.ToString();

            global.SettingColor = Brushes.SaddleBrown.ToString();
            global.SettingTextColor = Brushes.SandyBrown.ToString();

        }
    }
}
