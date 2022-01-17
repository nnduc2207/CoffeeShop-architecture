using CoffeeShop.Model;
using CoffeeShop.Models;
using CoffeeShop.Services.ModelServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoffeeShop.ViewModels
{
    public class WarehouseViewModel : BaseViewModel
    {
        #region variables
        private static WarehouseViewModel _instance = null;
        private static object m_lock = new object();

        private bool _isOpenAddRawMaterialDialog;
        private bool _isOpenChangeAmountRawMaterialDialog;
        private bool _isOpenDeleteRawMaterialDialog;
        private AsyncObservableCollection<KhoNguyenLieuService> _rawMaterials;
        private KhoNguyenLieuService _selectedRawMaterial;
        private int _changeRawMaterialAmount;

        // search
        private string _search;

        // mesage
        private string _message;
        private bool _isOpenMessageDialog;

        #endregion

        #region properties
        public bool IsOpenAddRawMaterialDialog { get => _isOpenAddRawMaterialDialog; set { _isOpenAddRawMaterialDialog = value;
                SelectedRawMaterial = new KhoNguyenLieuService();
                OnPropertyChanged(); } }
        public bool IsOpenChangeAmountRawMaterialDialog
        {
            get => _isOpenChangeAmountRawMaterialDialog; set
            {
                _isOpenChangeAmountRawMaterialDialog = value;
                if (value == true)
                {
                    ChangeRawMaterialAmount = 0;
                }
                OnPropertyChanged();
            }
        }
        public bool IsOpenDeleteRawMaterialDialog { get => _isOpenDeleteRawMaterialDialog; set { _isOpenDeleteRawMaterialDialog = value; OnPropertyChanged(); } }
        public AsyncObservableCollection<KhoNguyenLieuService> RawMaterials { get => _rawMaterials; set { _rawMaterials = value; OnPropertyChanged(); } }
        public KhoNguyenLieuService SelectedRawMaterial { get => _selectedRawMaterial; set { _selectedRawMaterial = value; OnPropertyChanged(); } }
        public int ChangeRawMaterialAmount { get => _changeRawMaterialAmount; set { _changeRawMaterialAmount = value; OnPropertyChanged(); } }

        //search
        public string Search
        {
            get => _search; set
            {
                _search = value;
                AsyncObservableCollection<dynamic> tmp = new AsyncObservableCollection<dynamic>();
                foreach (var item in DataProvider.Ins.DB.KhoNguyenLieu)
                {
                    tmp.Add(new
                    {
                        Ma = item.MaNL,
                        Ten = item.Ten,
                        SoLuong = item.SoLuong,
                        DonVi = item.DonVi
                    });
                }
                tmp = SearchByName(Search, tmp);
                RawMaterials = new AsyncObservableCollection<KhoNguyenLieuService>();
                foreach (var item in tmp)
                {
                    RawMaterials.Add(KhoNguyenLieuService.GetById(item.Ma));
                }
                OnPropertyChanged();
            }
        }

        // message
        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }
        public bool IsOpenMessageDialog { get => _isOpenMessageDialog; set { _isOpenMessageDialog = value; OnPropertyChanged(); } }

        #endregion

        #region Commands
        public ICommand Loaded { get; set; }
        public ICommand AddRawMaterialCommand { get; set; }
        public ICommand ClickAddRawMaterialButtonCommand { get; set; }
        public ICommand ChangeAmountRawMaterialCommand { get; set; }
        public ICommand ClickChangeAmountRawMaterialCommand { get; set; }
        public ICommand DeleteRawMaterialCommand { get; set; }
        public ICommand ClickDeleteRawMaterialCommand { get; set; }
        public ICommand CloseMessageDialog { get; set; }

        #endregion

        public static WarehouseViewModel GetInstance()
        {
            // DoubleLock
            if (_instance == null)
            {
                lock (m_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new WarehouseViewModel();
                    }
                }
            }
            return _instance;
        }

        public WarehouseViewModel()
        {
            //Command
            Loaded = new RelayCommand<dynamic>((param) => { return true; }, (param) => {

                // Setup dữ liệu
                IsOpenAddRawMaterialDialog = false;
                // Danh sách nguyên liệu trong kho
                RawMaterials = new AsyncObservableCollection<KhoNguyenLieuService>();
                foreach (KhoNguyenLieu item in DataProvider.Ins.DB.KhoNguyenLieu)
                {
                    RawMaterials.Add(new KhoNguyenLieuService(item));
                }
            });


            ClickAddRawMaterialButtonCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                IsOpenAddRawMaterialDialog = true;
            });

            AddRawMaterialCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                if (bool.Parse(param.ToString()) == true)
                {
                    try
                    {
                        KhoNguyenLieuService newKnl = SelectedRawMaterial.Create();
                        RawMaterials.Add(newKnl);
                        SelectedRawMaterial = null;
                        IsOpenAddRawMaterialDialog = false;
                    }
                    catch (Exception e)
                    {
                        Message = e.Message;
                        IsOpenMessageDialog = true;
                    }
                }
                else
                {
                    IsOpenAddRawMaterialDialog = false;

                }

            });

            ClickChangeAmountRawMaterialCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) =>
            {
                SelectedRawMaterial = param;
                IsOpenChangeAmountRawMaterialDialog = true;
            });

            ChangeAmountRawMaterialCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                if (bool.Parse(param.ToString()) == true)
                {
                    if (SelectedRawMaterial.SoLuong + ChangeRawMaterialAmount < 0)
                    {
                        Message = "Vui lòng nhập số lượng thay đổi hợp lý";
                        IsOpenMessageDialog = true;
                    }
                    else
                    {
                        SelectedRawMaterial.SoLuong += ChangeRawMaterialAmount;
                        RawMaterials.First(x => x.Ma == SelectedRawMaterial.Ma).SoLuong = SelectedRawMaterial.SoLuong;
                    }
                }
                else
                {
                    SelectedRawMaterial = null;
                }
                IsOpenChangeAmountRawMaterialDialog = false;
            });

            ClickDeleteRawMaterialCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) =>
            {
                SelectedRawMaterial = param;
                IsOpenDeleteRawMaterialDialog = true;
            });

            DeleteRawMaterialCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                if (bool.Parse(param.ToString()) == true)
                {
                    SelectedRawMaterial.Delete();
                    RawMaterials.Remove(SelectedRawMaterial);
                }
                else
                {
                    SelectedRawMaterial = null;
                }
                IsOpenDeleteRawMaterialDialog = false;
            });

            CloseMessageDialog = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenMessageDialog = false;
            });

        }
    }
}
