using CoffeeShop.Services.CustomerTypes;
using CoffeeShop.Services.ModelServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Services
{
    class LoaiKhachHangFactory
    {
        static public ILoaiKhachHang create(int totalpayment)
        {
            if (totalpayment >= int.Parse(ThongSoService.GetByName("HanMucChiTieuKHThanThiet").GiaTri))
            {
                return new KHThanThiet();
            }
            if (totalpayment >= int.Parse(ThongSoService.GetByName("HanMucChiTieuKHThuongXuyen").GiaTri))
            {
                return new KHThuongXuyen();
            }
            if (totalpayment >= int.Parse(ThongSoService.GetByName("HanMucChiTieuKHThuong").GiaTri))
            {
                return new KHThuong();
            }
            return null;
        }
    }
}
