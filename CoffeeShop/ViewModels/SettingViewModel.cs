using CoffeeShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoffeeShop.Services.ModelServices;

namespace CoffeeShop.ViewModels
{
    public class SettingViewModel:BaseViewModel
    {
        #region variables
        private static SettingViewModel _instance = null;
        private static object m_lock = new object();

        // mesage
        private string _message;
        private bool _isOpenMessageDialog;

        // change password
        private string _passwordOld;
        private string _passwordNew;
        private string _passwordNew2;
        private bool _isOpenChangePasswordDialog;

        // thông số
        private float _tLDDKHT;
        private float _tLDDKHTX;
        private float _tLDDKHTT;
        private int _hMCTKHT;
        private int _hMCTKHTX;
        private int _hMCTKHTT;
        private int _yearsReset;

        #endregion

        #region properties

        // message
        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }
        public bool IsOpenMessageDialog { get => _isOpenMessageDialog; set { _isOpenMessageDialog = value; OnPropertyChanged(); } }

        // change password
        public string PasswordOld { get => _passwordOld; set { _passwordOld = value; OnPropertyChanged(); } }
        public string PasswordNew { get => _passwordNew; set { _passwordNew = value; OnPropertyChanged(); } }
        public string PasswordNew2 { get => _passwordNew2; set { _passwordNew2 = value; OnPropertyChanged(); } }
        public bool IsOpenChangePasswordDialog { get => _isOpenChangePasswordDialog; set { _isOpenChangePasswordDialog = value;
                PasswordOld = null;
                PasswordNew = null;
                PasswordNew2 = null;
                OnPropertyChanged(); } }

        // thông số
        public float TLDDKHT { get => _tLDDKHT; set { _tLDDKHT = value; OnPropertyChanged(); } }
        public float TLDDKHTX { get => _tLDDKHTX; set { _tLDDKHTX = value; OnPropertyChanged(); } }
        public float TLDDKHTT { get => _tLDDKHTT; set { _tLDDKHTT = value; OnPropertyChanged(); } }
        public int HMCTKHT { get => _hMCTKHT; set { _hMCTKHT = value; OnPropertyChanged(); } }
        public int HMCTKHTX { get => _hMCTKHTX; set { _hMCTKHTX = value; OnPropertyChanged(); } }
        public int HMCTKHTT { get => _hMCTKHTT; set { _hMCTKHTT = value; OnPropertyChanged(); } }
        public int YearsReset { get => _yearsReset; set { _yearsReset = value; OnPropertyChanged(); } }

        #endregion

        #region commands
        public ICommand CloseMessageDialog { get; set; }
        public ICommand ClickChangePasswordButtonCommand { get; set; }
        public ICommand CloseChangePasswordDialog { get; set; }
        public ICommand ClickUpdateButtonCommand { get; set; }

        #endregion
        public static SettingViewModel GetInstance()
        {
            // DoubleLock
            if (_instance == null)
            {
                lock (m_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SettingViewModel();
                    }
                }
            }
            return _instance;
        }

        public SettingViewModel()
        {
            // khởi tạo dữ liệu
            TLDDKHT = float.Parse(ThongSoService.GetByName("TiLeDoiDiemKHThuong").GiaTri)*10000;
            TLDDKHTX = float.Parse(ThongSoService.GetByName("TiLeDoiDiemKHThuongXuyen").GiaTri) * 10000;
            TLDDKHTT = float.Parse(ThongSoService.GetByName("TiLeDoiDiemKHThanThiet").GiaTri) * 10000;
            HMCTKHT = int.Parse(ThongSoService.GetByName("HanMucChiTieuKHThuong").GiaTri);
            HMCTKHTX = int.Parse(ThongSoService.GetByName("HanMucChiTieuKHThuongXuyen").GiaTri);
            HMCTKHTT = int.Parse(ThongSoService.GetByName("HanMucChiTieuKHThanThiet").GiaTri);
            YearsReset = int.Parse(ThongSoService.GetByName("KyHanResetChiTieu").GiaTri);

            // command
            CloseMessageDialog = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenMessageDialog = false;
            });

            ClickChangePasswordButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenChangePasswordDialog = true;
            });

            CloseChangePasswordDialog = new RelayCommand<object>((param) => { return true; }, (param) => {
                if (bool.Parse(param.ToString()) == true)
                {
                    ThongSoService oldPassword = ThongSoService.GetByName("MaBaoMat");
                    if (PasswordOld != oldPassword.GiaTri)
                    {
                        Message = "Mật khẩu cũ không chính xác";
                        IsOpenMessageDialog = true;
                    }
                    else
                    {
                        if (PasswordNew != PasswordNew2)
                        {
                            Message = "Hai lần nhập mật khẩu mới không trùng nhau";
                            IsOpenMessageDialog = true;
                        }
                        else
                        {
                            ThongSoService newPassword = new ThongSoService();
                            newPassword.Ma = oldPassword.Ma;
                            newPassword.Ten = oldPassword.Ten;
                            newPassword.Kieu = oldPassword.Kieu;
                            newPassword.GiaTri = PasswordNew;
                            newPassword.Update();
                            IsOpenChangePasswordDialog = false;
                        }
                    }
                }
                else
                {
                    IsOpenChangePasswordDialog = false;

                }
            });

            ClickUpdateButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (float.Parse(TLDDKHT.ToString()) < 0)
                {
                    Message = "Tỉ lệ đổi điểm khách hàng thường không đúng";
                    IsOpenMessageDialog = true;
                }
                else if (float.Parse(TLDDKHTX.ToString()) < 0)
                {
                    Message = "Tỉ lệ đổi điểm khách hàng thường xuyên không đúng";
                    IsOpenMessageDialog = true;
                }
                else if(float.Parse(TLDDKHTT.ToString()) < 0)
                {
                    Message = "Tỉ lệ đổi điểm khách hàng thân thiết không đúng";
                    IsOpenMessageDialog = true;
                }
                else if(int.Parse(HMCTKHT.ToString()) < 0)
                {
                    Message = "Hạn mức chi tiêu khách hàng thường không đúng";
                    IsOpenMessageDialog = true;
                }
                else if (int.Parse(HMCTKHTX.ToString()) < 0)
                {
                    Message = "Hạn mức chi tiêu khách hàng thường xuyên không đúng";
                    IsOpenMessageDialog = true;
                }
                else if (int.Parse(HMCTKHTT.ToString()) < 0)
                {
                    Message = "Hạn mức chi tiêu khách hàng thân thiết không đúng";
                    IsOpenMessageDialog = true;
                }
                else if (int.Parse(YearsReset.ToString()) < 0)
                {
                    Message = "Kỳ hạn reset chi tiêu khách hàng không đúng";
                    IsOpenMessageDialog = true;
                }
                else
                {

                    SaveSetting("TiLeDoiDiemKHThuong", (TLDDKHT / 10000).ToString());
                    SaveSetting("TiLeDoiDiemKHThuongXuyen", (TLDDKHTX / 10000).ToString());
                    SaveSetting("TiLeDoiDiemKHThanThiet", (TLDDKHTT / 10000).ToString());
                    SaveSetting("HanMucChiTieuKHThuong", HMCTKHT.ToString());
                    SaveSetting("HanMucChiTieuKHThuongXuyen", HMCTKHTX.ToString());
                    SaveSetting("HanMucChiTieuKHThanThiet", HMCTKHTT.ToString());
                    SaveSetting("KyHanResetChiTieu", YearsReset.ToString());
                    Message = "Cập nhật thành công";
                    IsOpenMessageDialog = true;
                }
            });

        }
        private void SaveSetting(string name, string newValue)
        {
            ThongSoService oldSetting = ThongSoService.GetByName(name);
            ThongSoService newSetting = new ThongSoService();
            newSetting.Ma = oldSetting.Ma;
            newSetting.Ten = oldSetting.Ten;
            newSetting.Kieu = oldSetting.Kieu;
            newSetting.GiaTri = newValue;
            newSetting.Update();
        }
    }
}
