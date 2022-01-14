using CoffeeShop.Services;
using CoffeeShop.Services.ModelServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Services.CustomerTypes
{
    public class KHThanThiet : ILoaiKhachHang
    {
        public int TinhDiemThuong(int money)
        {
            float rate = float.Parse(ThongSoService.GetByName("TiLeDoiDiemKHThanThiet").GiaTri);
            return (int)(rate * money);
        }
    }
}
