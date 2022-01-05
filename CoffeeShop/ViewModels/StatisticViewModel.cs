using CoffeeShop.Model;
using CoffeeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;

namespace CoffeeShop.ViewModels
{
    public class StatisticViewModel : BaseViewModel
    {
        #region variables
        private static StatisticViewModel _instance = null;
        private static object m_lock = new object();
        private AsyncObservableCollection<int> _years;
        private int _selectedYear;

        // mesage
        private string _message;
        private bool _isOpenMessageDialog;

        // month venue
        private List<string> _timeLabels = new List<string>();
        private Func<int, string> _timeFormatter;
        private SeriesCollection _timeChartCollection;

        // Product venue
        private List<string> _productLabels = new List<string>();
        private Func<int, string> _productFormatter;
        private SeriesCollection _productChartCollection;

        #endregion

        #region properties
        public AsyncObservableCollection<int> Years { get => _years; set { _years = value; OnPropertyChanged(); } }
        public int SelectedYear
        {
            get => _selectedYear; set
            {
                _selectedYear = value;
                makeTimeChart();
                makeProductChart();
                OnPropertyChanged();
            }
        }

        // message
        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }
        public bool IsOpenMessageDialog { get => _isOpenMessageDialog; set { _isOpenMessageDialog = value; OnPropertyChanged(); } }

        // month venue
        public List<string> TimeLabels { get => _timeLabels; set { _timeLabels = value; OnPropertyChanged(); } }
        public Func<int, string> TimeFormatter { get => _timeFormatter; set { _timeFormatter = value; OnPropertyChanged(); } }
        public SeriesCollection TimeChartCollection { get => _timeChartCollection; set { _timeChartCollection = value; OnPropertyChanged(); } }

        // Product venue
        public List<string> ProductLabels { get => _productLabels; set { _productLabels = value; OnPropertyChanged(); } }
        public Func<int, string> ProductFormatter { get => _productFormatter; set { _productFormatter = value; OnPropertyChanged(); } }
        public SeriesCollection ProductChartCollection { get => _productChartCollection; set { _productChartCollection = value; OnPropertyChanged(); } }

        #endregion

        #region Commands
        public ICommand Loaded { get; set; }
        public ICommand CloseMessageDialog { get; set; }

        #endregion

        public static StatisticViewModel GetInstance()
        {
            // DoubleLock
            if (_instance == null)
            {
                lock (m_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new StatisticViewModel();
                    }
                }
            }
            return _instance;
        }

        public StatisticViewModel()
        {

            //Command
            Loaded = new RelayCommand<dynamic>((param) => { return true; }, (param) => {

                // Setup dữ liệu
                Years = new AsyncObservableCollection<int>();
                for (int i = DataProvider.Ins.DB.HoaDon.Min(x=>x.NgayTao).Year; i <= DateTime.Now.Year; i++)
                {
                    Years.Add(i);
                }
                SelectedYear = DateTime.Now.Year;
            });

            CloseMessageDialog = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenMessageDialog = false;
            });

        }

        void makeTimeChart()
        {
            TimeChartCollection = new SeriesCollection();
            ChartValues<int> values = new ChartValues<int>();
            List<String> months = new List<String>();
            List<dynamic> counts = new List<dynamic>();
            for (int i = 0; i < 12; i++)
            {
                months.Add($"Tháng {i + 1}");
                var hoadons = DataProvider.Ins.DB.HoaDon.Where(x => x.NgayTao.Month == (i + 1) && x.NgayTao.Year == SelectedYear);
                values.Add( (hoadons.Count() == 0) ? 0 : hoadons.Sum(x => x.TongTien));
            }
            TimeChartCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Revenue",
                        Values = values
                    }
                };
            // collumn name
            TimeLabels = months;
            TimeFormatter = value => value.ToString("N");
        }

        void makeProductChart()
        {
            ProductChartCollection = new SeriesCollection();
            ChartValues<int> values = new ChartValues<int>();
            List<String> names = new List<String>();
            var QuerySyntax = (from cthd in DataProvider.Ins.DB.ChiTietHoaDon
                               group cthd by cthd.TenSP into cthd2
                               select new
                               {
                                   TenSP = cthd2.Key,
                                   DoanhThu = cthd2.Sum(x=>x.GiaSP*x.SoLuong)
                               }).ToList();
            foreach (var item in QuerySyntax.OrderByDescending(x=>x.DoanhThu))
            {
                names.Add(item.TenSP);
                values.Add(item.DoanhThu);
            }
            ProductChartCollection = new SeriesCollection
                {
                    new RowSeries
                    {
                        Title = "Doanh Thu",
                        Values = values
                    }
            };
            // collumn name
            ProductLabels = names;
            ProductFormatter = value => value.ToString("N");
        }

    }
}
