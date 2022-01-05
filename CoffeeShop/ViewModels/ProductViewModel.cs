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

namespace CoffeeShop.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        #region variables
        private static ProductViewModel _instance = null;
        private static object m_lock = new object();

        private AsyncObservableCollection<dynamic> _categories;
        private dynamic _selectedCategory;
        private AsyncObservableCollection<dynamic> _products;
        private dynamic _selectedProduct;

        // search
        private string _search;

        // Add product
        private bool _isOpenAddProductDialog;
        private string _newProductName;
        private int _newProductPrice;
        private byte[] _newProductThumbnail;
        private dynamic _selectedNewProductCategory;
        private AsyncObservableCollection<dynamic> _newProductCategories;
        private dynamic _selectedAddNewProductRawMaterial;
        private string _selectedAddNewProductRawMaterialUnit;
        private int _newProductRawMaterialAmount;
        private AsyncObservableCollection<dynamic> _addNewProductRawMaterials;
        private dynamic _selectedNewProductRawMaterial;
        private AsyncObservableCollection<dynamic> _newProductRawMaterials;

        // Edit product
        private bool _isOpenEditProductDialog;
        private string _editProductName;
        private int _editProductPrice;
        private byte[] _editProductThumbnail;
        private dynamic _selectedEditProductCategory;
        private AsyncObservableCollection<dynamic> _editProductCategories;
        private dynamic _selectedAddEditProductRawMaterial;
        private string _selectedAddEditProductRawMaterialUnit;
        private int _editProductRawMaterialAmount;
        private AsyncObservableCollection<dynamic> _addEditProductRawMaterials;
        private dynamic _selectedEditProductRawMaterial;
        private AsyncObservableCollection<dynamic> _editProductRawMaterials;

        // Delete product
        private bool _isOpenDeleteProductDialog;

        // Show product
        private bool _isOpenShowProductDialog;
        private string _showProductName;
        private int _showProductPrice;
        private byte[] _showProductThumbnail;
        private string _showProductCategory;
        private dynamic _selectedShowProductRawMaterial;
        private AsyncObservableCollection<dynamic> _showProductRawMaterials;

        // mesage
        private string _message;
        private bool _isOpenMessageDialog;

        #endregion

        #region properties
        public AsyncObservableCollection<dynamic> Categories { get => _categories; set { _categories = value; OnPropertyChanged(); } }
        public dynamic SelectedCategory { get => _selectedCategory; set { _selectedCategory = value;
                if (value != null)
                {
                    int maloai = value.Ma;
                    Products = new AsyncObservableCollection<dynamic>();
                    foreach (var item in DataProvider.Ins.DB.SanPham.Where(x=>x.MaLoai == maloai))
                    {
                        Products.Add(new { 
                            Ma = item.Ma,
                            Ten = item.Ten,
                            Gia = item.Gia,
                            Anh = item.Anh,
                        });
                    }
                }
                OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> Products { get => _products; set { _products = value; OnPropertyChanged(); } }
        public dynamic SelectedProduct { get => _selectedProduct; set { _selectedProduct = value;
                if (value != null && IsOpenEditProductDialog == false && IsOpenDeleteProductDialog == false)
                {
                    IsOpenShowProductDialog = true;
                }
                OnPropertyChanged(); } }

        //search
        public string Search { get => _search; set { _search = value;
                Products = new AsyncObservableCollection<dynamic>();
                foreach (var item in DataProvider.Ins.DB.SanPham)
                {
                    Products.Add(new
                    {
                        Ma = item.Ma,
                        Ten = item.Ten,
                        Gia = item.Gia,
                        Anh = item.Anh,
                    });
                }
                Products = SearchByName(Search, Products);
                OnPropertyChanged(); } }

        // Add product
        public bool IsOpenAddProductDialog
        {
            get => _isOpenAddProductDialog; set
            {
                _isOpenAddProductDialog = value;
                if (value == true)
                {
                    NewProductName = "";
                    NewProductPrice = 0;
                    SelectedNewProductCategory = null;
                    NewProductCategories = new AsyncObservableCollection<dynamic>();
                    foreach (var item in DataProvider.Ins.DB.LoaiSanPham)
                    {
                        NewProductCategories.Add(new
                        {
                            Ma = item.Ma,
                            Ten = item.Ten
                        });
                    }
                    NewProductThumbnail = null;
                    SelectedAddNewProductRawMaterial = null;
                    AddNewProductRawMaterials = new AsyncObservableCollection<dynamic>();
                    foreach (var item in DataProvider.Ins.DB.KhoNguyenLieu)
                    {
                        AddNewProductRawMaterials.Add(new
                        {
                            MaNL = item.MaNL,
                            Ten = item.Ten,
                            DonVi = item.DonVi,
                        });
                    }
                    SelectedNewProductRawMaterial = null;
                    NewProductRawMaterials = new AsyncObservableCollection<dynamic>();
                }
                OnPropertyChanged();
            }
        }
        public string NewProductName { get => _newProductName; set { value = value.ToUpper(); _newProductName = value; OnPropertyChanged(); } }
        public int NewProductPrice { get => _newProductPrice; set { _newProductPrice = value; OnPropertyChanged(); } }
        public byte[] NewProductThumbnail { get => _newProductThumbnail; set { _newProductThumbnail = value; OnPropertyChanged(); } }
        public dynamic SelectedNewProductCategory { get => _selectedNewProductCategory; set { _selectedNewProductCategory = value; OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> NewProductCategories { get => _newProductCategories; set { _newProductCategories = value; OnPropertyChanged(); } }
        public dynamic SelectedAddNewProductRawMaterial { get => _selectedAddNewProductRawMaterial; set { _selectedAddNewProductRawMaterial = value; SelectedAddNewProductRawMaterialUnit = (value == null) ?  "" : value.DonVi; OnPropertyChanged(); } }
        public string SelectedAddNewProductRawMaterialUnit { get => _selectedAddNewProductRawMaterialUnit; set { _selectedAddNewProductRawMaterialUnit = value; OnPropertyChanged(); } }
        public int NewProductRawMaterialAmount { get => _newProductRawMaterialAmount; set { _newProductRawMaterialAmount = value; OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> AddNewProductRawMaterials { get => _addNewProductRawMaterials; set { _addNewProductRawMaterials = value; OnPropertyChanged(); } }
        public dynamic SelectedNewProductRawMaterial { get => _selectedNewProductRawMaterial; set { _selectedNewProductRawMaterial = value; OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> NewProductRawMaterials { get => _newProductRawMaterials; set { _newProductRawMaterials = value; OnPropertyChanged(); } }

        // Edit product
        public bool IsOpenEditProductDialog { get => _isOpenEditProductDialog; set { _isOpenEditProductDialog = value; OnPropertyChanged(); } }
        public string EditProductName { get => _editProductName; set { value = value.ToUpper(); _editProductName = value; OnPropertyChanged(); } }
        public int EditProductPrice { get => _editProductPrice; set { _editProductPrice = value; OnPropertyChanged(); } }
        public byte[] EditProductThumbnail { get => _editProductThumbnail; set { _editProductThumbnail = value; OnPropertyChanged(); } }
        public dynamic SelectedEditProductCategory { get => _selectedEditProductCategory; set { _selectedEditProductCategory = value; OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> EditProductCategories { get => _editProductCategories; set { _editProductCategories = value; OnPropertyChanged(); } }
        public dynamic SelectedAddEditProductRawMaterial { get => _selectedAddEditProductRawMaterial; set { _selectedAddEditProductRawMaterial = value; SelectedAddEditProductRawMaterialUnit = (value == null) ? "" : value.DonVi; OnPropertyChanged(); } }
        public string SelectedAddEditProductRawMaterialUnit { get => _selectedAddEditProductRawMaterialUnit; set { _selectedAddEditProductRawMaterialUnit = value; OnPropertyChanged(); } }
        public int EditProductRawMaterialAmount { get => _editProductRawMaterialAmount; set { _editProductRawMaterialAmount = value; OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> AddEditProductRawMaterials { get => _addEditProductRawMaterials; set { _addEditProductRawMaterials = value; OnPropertyChanged(); } }
        public dynamic SelectedEditProductRawMaterial { get => _selectedEditProductRawMaterial; set { _selectedEditProductRawMaterial = value; OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> EditProductRawMaterials { get => _editProductRawMaterials; set { _editProductRawMaterials = value; OnPropertyChanged(); } }

        // Delete product
        public bool IsOpenDeleteProductDialog { get => _isOpenDeleteProductDialog; set { _isOpenDeleteProductDialog = value; OnPropertyChanged(); } }

        // Show product
        public bool IsOpenShowProductDialog { get => _isOpenShowProductDialog; set { _isOpenShowProductDialog = value;
                int ma = SelectedProduct.Ma;
                SanPham showproduct = DataProvider.Ins.DB.SanPham.First(x => x.Ma ==ma);
                ShowProductName = showproduct.Ten;
                ShowProductCategory = showproduct.LoaiSanPham.Ten;
                ShowProductPrice = showproduct.Gia;
                ShowProductThumbnail = showproduct.Anh;
                ShowProductRawMaterials = new AsyncObservableCollection<dynamic>();
                foreach (var item in showproduct.NguyenLieu)
                {
                    ShowProductRawMaterials.Add(new {
                        MaNL = item.MaNL,
                        Ten = item.KhoNguyenLieu.Ten,
                        DonVi = item.KhoNguyenLieu.DonVi,
                        SoLuong = item.SoLuong,
                    });
                }
                OnPropertyChanged(); } }
        public string ShowProductName { get => _showProductName; set { _showProductName = value; OnPropertyChanged(); } }
        public int ShowProductPrice { get => _showProductPrice; set { _showProductPrice = value; OnPropertyChanged(); } }
        public byte[] ShowProductThumbnail { get => _showProductThumbnail; set { _showProductThumbnail = value; OnPropertyChanged(); } }
        public string ShowProductCategory { get => _showProductCategory; set { _showProductCategory = value; OnPropertyChanged(); } }
        public dynamic SelectedShowProductRawMaterial { get => _selectedShowProductRawMaterial; set { _selectedShowProductRawMaterial = value; OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> ShowProductRawMaterials { get => _showProductRawMaterials; set { _showProductRawMaterials = value; OnPropertyChanged(); } }

        // message
        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }
        public bool IsOpenMessageDialog { get => _isOpenMessageDialog; set { _isOpenMessageDialog = value; OnPropertyChanged(); } }

        #endregion

        #region command
        public ICommand ChangeCategoryCommand { get; set; }

        public ICommand ClickAddProductButtonCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand AddThumbnailCommand { get; set; }
        public ICommand AddNewProductRawMaterialCommand { get; set; }
        public ICommand DeleteNewProductRawMaterialCommand { get; set; }

        public ICommand ClickEditProductButtonCommand { get; set; }
        public ICommand EditProductCommand { get; set; }
        public ICommand EditThumbnailCommand { get; set; }
        public ICommand AddEditProductRawMaterialCommand { get; set; }
        public ICommand DeleteEditProductRawMaterialCommand { get; set; }

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
                    if (NewProductName == null || NewProductName.Split(' ').Length == NewProductName.Length + 1)
                    {
                        Message = "Vui lòng không để trống tên sản phẩm";
                        IsOpenMessageDialog = true;
                    }
                    else if (SelectedNewProductCategory == null)
                    {
                        Message = "Vui lòng chọn loại sản phẩm";
                        IsOpenMessageDialog = true;
                    }
                    else if (NewProductPrice <= 0)
                    {
                        Message = "Vui lòng đặt giá sản phẩm hợp lý";
                        IsOpenMessageDialog = true;
                    }
                    else if (NewProductThumbnail == null)
                    {
                        Message = "Vui lòng không để trống ảnh sản phẩm";
                        IsOpenMessageDialog = true;
                    }
                    else
                    {
                        SanPham newsanpham = new SanPham();
                        newsanpham.Ma = (DataProvider.Ins.DB.SanPham.Count() == 0) ? 1 : DataProvider.Ins.DB.SanPham.Max(x => x.Ma) + 1;
                        newsanpham.Ten = NewProductName;
                        newsanpham.MaLoai = SelectedNewProductCategory.Ma;
                        newsanpham.Gia = NewProductPrice;
                        newsanpham.Anh = NewProductThumbnail;
                        DataProvider.Ins.DB.SanPham.Add(newsanpham);
                        DataProvider.Ins.DB.SaveChanges();
                        foreach (var item in NewProductRawMaterials)
                        {
                            DataProvider.Ins.DB.NguyenLieu.Add(new NguyenLieu
                            {
                                MaSP = newsanpham.Ma,
                                MaNL = item.MaNL,
                                SoLuong = item.SoLuong,
                            });
                        }
                        DataProvider.Ins.DB.SaveChanges();

                        if (SelectedCategory.Ma == newsanpham.MaLoai)
                        {

                            // cập nhật danh sách
                            Products.Remove(SelectedProduct);
                            Products.Add(new
                            {
                                Ma = newsanpham.Ma,
                                Ten = newsanpham.Ten,
                                Anh = newsanpham.Anh,
                                Gia = newsanpham.Gia,
                            });
                        }
                        IsOpenAddProductDialog = false;
                    }
                }
                else
                {
                    IsOpenAddProductDialog = false;

                }
            });

            AddThumbnailCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Open a Image File";
                ofd.Filter = "All Files(*.*) | *.*"; //Here you can filter which all files you wanted allow to open  

                if (ofd.ShowDialog() == true)
                {
                    BitmapImage image = new BitmapImage(
                    new Uri(ofd.FileName,
                    UriKind.Absolute));
                    MemoryStream memStream = new MemoryStream();
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(memStream);
                    NewProductThumbnail = memStream.ToArray();
                }
            });

            AddNewProductRawMaterialCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                NewProductRawMaterials.Add(new { 
                    MaNL = SelectedAddNewProductRawMaterial.MaNL,
                    Ten = SelectedAddNewProductRawMaterial.Ten,
                    SoLuong = NewProductRawMaterialAmount,
                    DonVi = SelectedAddNewProductRawMaterialUnit,
                });
                AddNewProductRawMaterials.Remove(SelectedAddNewProductRawMaterial);
                SelectedAddNewProductRawMaterial = null;
                NewProductRawMaterialAmount = 0;
            });

            DeleteNewProductRawMaterialCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                AddNewProductRawMaterials.Add(new
                {
                    MaNL = param.MaNL,
                    Ten = param.Ten,
                    DonVi = param.DonVi,
                });
                NewProductRawMaterials.Remove(param);
            });

            ClickEditProductButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenEditProductDialog = true;
                SelectedProduct = param;
                int maSP = param.Ma;
                SanPham sanpham = DataProvider.Ins.DB.SanPham.First(x => x.Ma == maSP);
                EditProductName = sanpham.Ten;
                EditProductPrice = sanpham.Gia;
                EditProductCategories = new AsyncObservableCollection<dynamic>();
                foreach (var item in DataProvider.Ins.DB.LoaiSanPham)
                {
                    EditProductCategories.Add(new
                    {
                        Ma = item.Ma,
                        Ten = item.Ten
                    });
                }
                SelectedEditProductCategory = EditProductCategories.First(x => x.Ma == sanpham.MaLoai);
                EditProductThumbnail = sanpham.Anh;
                SelectedEditProductRawMaterial = null;
                EditProductRawMaterials = new AsyncObservableCollection<dynamic>();
                foreach (var item in sanpham.NguyenLieu)
                {
                    EditProductRawMaterials.Add(new
                    {
                        MaNL = item.MaNL,
                        Ten = item.KhoNguyenLieu.Ten,
                        DonVi = item.KhoNguyenLieu.DonVi,
                        SoLuong = item.SoLuong,
                    });
                }
                SelectedAddEditProductRawMaterial = null;
                AddEditProductRawMaterials = new AsyncObservableCollection<dynamic>();
                foreach (var item in DataProvider.Ins.DB.KhoNguyenLieu)
                {
                    if (EditProductRawMaterials.Where(x => x.MaNL == item.MaNL).Count() == 0)
                    {
                        AddEditProductRawMaterials.Add(new
                        {
                            MaNL = item.MaNL,
                            Ten = item.Ten,
                            DonVi = item.DonVi,
                        });
                    }
                }
            });

            EditProductCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                if (bool.Parse(param.ToString()) == true)
                {
                    if (EditProductName == null || EditProductName.Split(' ').Length == EditProductName.Length + 1)
                    {
                        Message = "Vui lòng không để trống tên sản phẩm";
                        IsOpenMessageDialog = true;
                    }
                    else if (SelectedEditProductCategory == null)
                    {
                        Message = "Vui lòng chọn loại sản phẩm";
                        IsOpenMessageDialog = true;
                    }
                    else if (EditProductPrice <= 0)
                    {
                        Message = "Vui lòng đặt giá sản phẩm hợp lý";
                        IsOpenMessageDialog = true;
                    }
                    else if (EditProductThumbnail == null)
                    {
                        Message = "Vui lòng không để trống ảnh sản phẩm";
                        IsOpenMessageDialog = true;
                    }
                    else
                    {
                        int masanphamcu = SelectedProduct.Ma;

                        // xóa sản phẩm chưa chỉnh sửa
                        foreach (var item in DataProvider.Ins.DB.NguyenLieu.Where(x => x.MaSP == masanphamcu))
                        {
                            DataProvider.Ins.DB.NguyenLieu.Remove(item);
                        }
                        DataProvider.Ins.DB.SanPham.Remove(DataProvider.Ins.DB.SanPham.First(x => x.Ma == masanphamcu));
                        DataProvider.Ins.DB.SaveChanges();

                        //Thêm sản phẩm đã chỉnh sửa
                        SanPham editsanpham = new SanPham();
                        editsanpham.Ma = (DataProvider.Ins.DB.SanPham.Count() == 0) ? 1 : DataProvider.Ins.DB.SanPham.Max(x => x.Ma) + 1;
                        editsanpham.Ten = EditProductName;
                        editsanpham.MaLoai = SelectedEditProductCategory.Ma;
                        editsanpham.Gia = EditProductPrice;
                        editsanpham.Anh = EditProductThumbnail;
                        DataProvider.Ins.DB.SanPham.Add(editsanpham);
                        DataProvider.Ins.DB.SaveChanges();
                        foreach (var item in EditProductRawMaterials)
                        {
                            DataProvider.Ins.DB.NguyenLieu.Add(new NguyenLieu
                            {
                                MaSP = editsanpham.Ma,
                                MaNL = item.MaNL,
                                SoLuong = item.SoLuong,
                            });
                        }
                        DataProvider.Ins.DB.SaveChanges();

                        // cập nhật danh sách
                        Products.Remove(SelectedProduct);
                        if (SelectedCategory.Ma == editsanpham.MaLoai)
                        {
                            Products.Add(new
                            {
                                Ma = editsanpham.Ma,
                                Ten = editsanpham.Ten,
                                Anh = editsanpham.Anh,
                                Gia = editsanpham.Gia,
                            });
                        }
                        IsOpenEditProductDialog = false;
                    }
                }
                else
                {
                    IsOpenEditProductDialog = false;

                }
            });

            EditThumbnailCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Open a Image File";
                ofd.Filter = "All Files(*.*) | *.*"; //Here you can filter which all files you wanted allow to open  

                if (ofd.ShowDialog() == true)
                {
                    BitmapImage image = new BitmapImage(
                    new Uri(ofd.FileName,
                    UriKind.Absolute));
                    MemoryStream memStream = new MemoryStream();
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(memStream);
                    EditProductThumbnail = memStream.ToArray();
                }
            });

            AddEditProductRawMaterialCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                EditProductRawMaterials.Add(new
                {
                    MaNL = SelectedAddEditProductRawMaterial.MaNL,
                    Ten = SelectedAddEditProductRawMaterial.Ten,
                    SoLuong = EditProductRawMaterialAmount,
                    DonVi = SelectedAddEditProductRawMaterialUnit,
                });
                AddEditProductRawMaterials.Remove(SelectedAddEditProductRawMaterial);
                SelectedAddEditProductRawMaterial = null;
                EditProductRawMaterialAmount = 0;
            });

            DeleteEditProductRawMaterialCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                AddEditProductRawMaterials.Add(new
                {
                    MaNL = param.MaNL,
                    Ten = param.Ten,
                    DonVi = param.DonVi,
                });
                EditProductRawMaterials.Remove(param);
            });

            ClickDeleteProductButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenDeleteProductDialog = true;
                SelectedProduct = param;
            });

            DeleteProductCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                if (bool.Parse(param.ToString()) == true)
                {
                    int maDeleteProduct = SelectedProduct.Ma;

                    // xóa sản phẩm chưa chỉnh sửa
                    foreach (var item in DataProvider.Ins.DB.NguyenLieu.Where(x => x.MaSP == maDeleteProduct))
                    {
                        DataProvider.Ins.DB.NguyenLieu.Remove(item);
                    }
                    DataProvider.Ins.DB.SanPham.Remove(DataProvider.Ins.DB.SanPham.First(x => x.Ma == maDeleteProduct));
                    DataProvider.Ins.DB.SaveChanges();
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

    }
}
