using CoffeeShop.Model;
using CoffeeShop.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CoffeeShop.Services.ModelServices;

namespace CoffeeShop.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        #region variables
        private static ProductViewModel _instance = null;
        private static object m_lock = new object();

        // menu display
        private AsyncObservableCollection<LoaiSanPhamService> _categories;
        private LoaiSanPhamService _selectedCategory;

        private AsyncObservableCollection<dynamic> _products;
        private SanPhamService _selectedProduct;

        // popup display: for add, edit and show product
        private AsyncObservableCollection<KhoNguyenLieuService> _materials;
        private KhoNguyenLieuService _selectedMaterial;

        private AsyncObservableCollection<dynamic> _productMaterials;
        private dynamic _selectedProductMaterial;

        private SanPhamService _productDetail;
        private LoaiSanPhamService _selectedProductCategory;
        private int _productMaterialAmount;

        // search
        private string _search;

        // dialog
        private bool _isOpenAddProductDialog;
        private bool _isOpenEditProductDialog;
        private bool _isOpenDeleteProductDialog;
        private bool _isOpenShowProductDialog;

        // mesage
        private string _message;
        private bool _isOpenMessageDialog;

        #endregion

        #region properties
        public AsyncObservableCollection<LoaiSanPhamService> Categories { get => _categories; set { _categories = value; OnPropertyChanged(); } }
        public LoaiSanPhamService SelectedCategory { get => _selectedCategory; set { _selectedCategory = value;
                if (value != null)
                {
                    GetProductsByType(value.Ma);
                }
                OnPropertyChanged(); 
            } 
        }

        private void GetProductsByType(int maLoai)
        {
            List<SanPhamService> sanPhamList = SanPhamService.GetByType(maLoai);
            Products = new AsyncObservableCollection<dynamic>();
            foreach (SanPhamService item in sanPhamList)
            {
                Products.Add(item);
            }
        }

        public AsyncObservableCollection<dynamic> Products { get => _products; set { _products = value; OnPropertyChanged(); } }
        public SanPhamService SelectedProduct { get => _selectedProduct; set { _selectedProduct = value;
                //click to show product detail popup
                if (value != null && IsOpenEditProductDialog == false && IsOpenDeleteProductDialog == false)
                {
                    IsOpenShowProductDialog = true;
                }
                OnPropertyChanged(); 
            } 
        }

        public AsyncObservableCollection<KhoNguyenLieuService> Materials { get => _materials; set { _materials = value; OnPropertyChanged(); } }
        public KhoNguyenLieuService SelectedMaterial { get => _selectedMaterial; set { _selectedMaterial = value; OnPropertyChanged(); } }


        public AsyncObservableCollection<dynamic> ProductMaterials { get => _productMaterials; set { _productMaterials = value; OnPropertyChanged(); } }
        public dynamic SelectedProductMaterial { get => _selectedProductMaterial; set { _selectedProductMaterial = value; OnPropertyChanged(); } }

        public SanPhamService ProductDetail { get => _productDetail; set { _productDetail = value; OnPropertyChanged(); } }
        public LoaiSanPhamService SelectedProductCategory
        {
            get => _selectedProductCategory; set
            {
                _selectedProductCategory = value;
                if (value != null)
                {
                    ProductDetail.MaLoai = value.Ma;
                }
                OnPropertyChanged();
            }
        }
        public int ProductMaterialAmount { get => _productMaterialAmount; set { _productMaterialAmount = value; OnPropertyChanged(); } }

        //search
        public string Search { get => _search; set { _search = value;
                Products = new AsyncObservableCollection<dynamic>();
                foreach (var item in SanPhamService.GetAll())
                {
                    Products.Add(item);
                }
                Products = SearchByName(Search, Products);
                OnPropertyChanged(); 
            } 
        }


        // dialog
        #region dialog
        public bool IsOpenAddProductDialog
        {
            get => _isOpenAddProductDialog; set
            {
                _isOpenAddProductDialog = value;
                if (value == true)
                {
                    ProductDetail = new SanPhamService();
                    ProductDetail.Ten = "";
                    ProductDetail.Gia = 0;
                    ProductDetail.Anh = null;

                    SelectedProductCategory = null;
                    SelectedMaterial = null;
                    SelectedProductMaterial = null;

                    Materials = new AsyncObservableCollection<KhoNguyenLieuService>();
                    List<KhoNguyenLieuService> KhoNguyenLieuList = KhoNguyenLieuService.GetAll();
                    foreach (KhoNguyenLieuService item in KhoNguyenLieuList)
                    {
                        Materials.Add(item);
                    }
                    
                    ProductMaterials = new AsyncObservableCollection<dynamic>();
                }
                OnPropertyChanged();
            }
        }



       
        public bool IsOpenEditProductDialog { get => _isOpenEditProductDialog; set { _isOpenEditProductDialog = value; OnPropertyChanged(); } }
        
        public bool IsOpenDeleteProductDialog { get => _isOpenDeleteProductDialog; set { _isOpenDeleteProductDialog = value; OnPropertyChanged(); } }


        public bool IsOpenShowProductDialog { get => _isOpenShowProductDialog; set { _isOpenShowProductDialog = value;
                int ma = SelectedProduct.Ma;
                ProductDetail = SanPhamService.GetById(ma);
                SelectedProductCategory = LoaiSanPhamService.GetById(ProductDetail.MaLoai);

                List<NguyenLieuService> NguyenLieuList = NguyenLieuService.GetByProduct(ProductDetail.Ma);
                ProductMaterials = new AsyncObservableCollection<dynamic>();
                foreach (var item in NguyenLieuList)
                {
                    KhoNguyenLieuService materialToShow = KhoNguyenLieuService.GetById(item.MaNL);
                    ProductMaterials.Add(new
                    {
                        MaNL = item.MaNL,
                        Ten = materialToShow.Ten,
                        DonVi = materialToShow.DonVi,
                        SoLuong = item.SoLuong,
                    });
                }
                OnPropertyChanged(); 
            } 
        }

        #endregion

        // message
        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }
        public bool IsOpenMessageDialog { get => _isOpenMessageDialog; set { _isOpenMessageDialog = value; OnPropertyChanged(); } }

        #endregion

        #region command
        public ICommand ChangeCategoryCommand { get; set; }

        public ICommand ClickAddProductButtonCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand ThumbnailCommand { get; set; }
        public ICommand AddProductRawMaterialCommand { get; set; }
        public ICommand DeleteProductRawMaterialCommand { get; set; }

        public ICommand ClickEditProductButtonCommand { get; set; }
        public ICommand EditProductCommand { get; set; }

        public ICommand ClickDeleteProductButtonCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ICommand CloseShowProductCommand { get; set; }

        public ICommand CloseMessageDialog { get; set; }

        #endregion

        public static ProductViewModel GetInstance()
        {
            // DoubleLock
            if (_instance == null)
            {
                lock (m_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ProductViewModel();
                    }
                }
            }
            return _instance;
        }

        public ProductViewModel()
        {
            // khởi tạo dữ liệu

            // Danh sách loại sản phẩm
            Categories = new AsyncObservableCollection<LoaiSanPhamService>();
            foreach (var item in LoaiSanPhamService.GetAll())
            {
                Categories.Add(item);
            }
            SelectedCategory = Categories.ElementAt(0);

            // Command

            ChangeCategoryCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                SelectedCategory = param;
            });

            ClickAddProductButtonCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                IsOpenAddProductDialog = true;
            });

            AddProductCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                if (bool.Parse(param.ToString()) == true)
                {
                    if (IsAllInputValid())
                    {
                        ProductDetail = ProductDetail.Create();
                        foreach (var item in ProductMaterials)
                        {
                            NguyenLieuService materialToAdd = new NguyenLieuService();
                            materialToAdd.MaSP = ProductDetail.Ma;
                            materialToAdd.MaNL = item.MaNL;
                            materialToAdd.SoLuong = item.SoLuong;
                            materialToAdd.Create();
                        }
                        if (SelectedCategory.Ma == ProductDetail.MaLoai)
                        {
                            Products.Add(ProductDetail);
                        }
                        IsOpenAddProductDialog = false;
                    }
                }
                else
                {
                    IsOpenAddProductDialog = false;

                }
            });


            ClickEditProductButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenEditProductDialog = true;

                ProductDetail = SanPhamService.GetById(param.Ma);
                SelectedProductCategory = LoaiSanPhamService.GetById(ProductDetail.MaLoai);

                SelectedMaterial = null;
                SelectedProductMaterial = null;

                ProductMaterials = new AsyncObservableCollection<dynamic>();

                foreach (var item in NguyenLieuService.GetByProduct(param.Ma))
                {
                    KhoNguyenLieuService materialToEdit = KhoNguyenLieuService.GetById(item.MaNL);
                    ProductMaterials.Add(new
                    {
                        MaNL = item.MaNL,
                        Ten = materialToEdit.Ten,
                        DonVi = materialToEdit.DonVi,
                        SoLuong = item.SoLuong,
                    });
                }

                Materials = new AsyncObservableCollection<KhoNguyenLieuService>();
                List<KhoNguyenLieuService> KhoNguyenLieuList = KhoNguyenLieuService.GetAll();
                foreach (KhoNguyenLieuService item in KhoNguyenLieuList)
                {
                    foreach (var material in ProductMaterials)
                    {
                        if (item.Ma != material.MaNL)
                            Materials.Add(item);
                    }
                   
                }
            });

            EditProductCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                if (bool.Parse(param.ToString()) == true)
                {
                    if (IsAllInputValid())
                    {
                        ProductDetail.Update();
                        
                        foreach (var item in ProductMaterials)
                        {
                            NguyenLieuService materialToAdd = new NguyenLieuService();
                            materialToAdd.MaSP = SelectedProduct.Ma;
                            materialToAdd.MaNL = item.MaNL;
                            materialToAdd.SoLuong = item.SoLuong;
                            item.Create();
                        }

                        // cập nhật danh sách
                        Products.Remove(SelectedProduct);
                        if (SelectedCategory.Ma == ProductDetail.MaLoai)
                        {
                            Products.Add(ProductDetail);
                        }
                        IsOpenEditProductDialog = false;
                    }
                }
                else
                {
                    IsOpenEditProductDialog = false;

                }
            });

            ThumbnailCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Open a Image File";
                ofd.Filter = "All Files(*.*) | *.*"; //Here you can filter which all files you wanted allow to open  

                if (ofd.ShowDialog() == true)
                {
                    ProductDetail.Anh = SanPhamService.ImageToBytes(ofd.FileName);
                }
            });

            AddProductRawMaterialCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                ProductMaterials.Add(new
                {
                    MaNL = SelectedMaterial.Ma,
                    Ten = SelectedMaterial.Ten,
                    SoLuong = ProductMaterialAmount,
                    DonVi = SelectedMaterial.DonVi,
                });
                Materials.Remove(SelectedMaterial);
                SelectedProductMaterial = null;
                ProductMaterialAmount = 0;
            });

            DeleteProductRawMaterialCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                KhoNguyenLieuService deletedMaterial = new KhoNguyenLieuService();
                deletedMaterial.Ma = param.MaNL;
                deletedMaterial.Ten = param.Ten;
                deletedMaterial.DonVi = param.DonVi;
                Materials.Add(deletedMaterial);
                ProductMaterials.Remove(param);
            });

            ClickDeleteProductButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenDeleteProductDialog = true;
                SelectedProduct = param;
            });

            DeleteProductCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                if (bool.Parse(param.ToString()) == true)
                {
                    ProductDetail.Delete();
                    foreach (var item in ProductMaterials)
                    {
                        NguyenLieuService materialToDelete = new NguyenLieuService();
                        materialToDelete.MaSP = SelectedProduct.Ma;
                        materialToDelete.MaNL = item.MaNL;
                        materialToDelete.SoLuong = item.SoLuong;
                        materialToDelete.Delete();
                    }
                    Products.Remove(SelectedProduct);
                }
                IsOpenDeleteProductDialog = false;
            });

            CloseShowProductCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                IsOpenShowProductDialog = false;
                SelectedProduct = null;
            });

            CloseMessageDialog = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenMessageDialog = false;
            });
        }
        private bool IsAllInputValid()
        {
            if (ProductDetail.Ten == null || ProductDetail.Ten.Split(' ').Length == ProductDetail.Ten.Length + 1)
            {
                Message = "Vui lòng không để trống tên sản phẩm";
                IsOpenMessageDialog = true;
                return false;
            }
            else if (SelectedProductCategory == null)
            {
                Message = "Vui lòng chọn loại sản phẩm";
                IsOpenMessageDialog = true;
                return false;
            }
            else if (ProductDetail.Gia <= 0)
            {
                Message = "Vui lòng đặt giá sản phẩm hợp lý";
                IsOpenMessageDialog = true;
                return false;
            }
            else if (ProductDetail.Anh == null)
            {
                Message = "Vui lòng không để trống ảnh sản phẩm";
                IsOpenMessageDialog = true;
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
