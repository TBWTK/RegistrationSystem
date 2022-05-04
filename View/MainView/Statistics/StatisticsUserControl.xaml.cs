using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms.DataVisualization.Charting;
using RegistrationSystem.Model;

namespace RegistrationSystem.View.MainView.Statistics
{
    /// <summary>
    /// Логика взаимодействия для StatisticsUserControl.xaml
    /// </summary>
    public partial class StatisticsUserControl : UserControl
    {
        List<Orders> orders { get; set; }
        List<Baskets> baskets { get; set; }
        List<Cars> cars { get; set; }
        List<Models> models { get; set; }
        List<Brands> brands { get; set; }
        List<Users> users { get; set; }


        public StatisticsUserControl()
        {
            InitializeComponent();

            orders = new List<Orders>();
            baskets = new List<Baskets>();
            cars = new List<Cars>();
            models = new List<Models>();
            brands = new List<Brands>();
            users = new List<Users>();

            using (var context = new TestDataBaseEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;
                orders = context.Orders.ToList();
                baskets = context.Baskets.ToList();
                cars = context.Cars.ToList();
                models = context.Models.ToList();
                brands = context.Brands.ToList();
                users = context.Users.ToList();
            }

            ChartPayments.ChartAreas.Add(new ChartArea("Main"));

            var currentSeries = new Series("Payments")
            {
                IsValueShownAsLabel = true
            };

            ChartPayments.Series.Add(currentSeries);

            ComboChart.ItemsSource = Enum.GetValues(typeof(SeriesChartType));

            foreach (var car in cars)
            {
                foreach (var model in models)
                {
                    if (car.NameCar == model.Id)
                    {
                        car.ModelCar = model.NameModel;
                        foreach (var brand in brands)
                        {
                            if (model.Brand == brand.Id)
                            {
                                car.BrandCar = brand.NameBrand;
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            DataGridUsers.ItemsSource = users;
            ChoiceCar.ItemsSource = cars;
            StartDate.SelectedDate = new DateTime(2022, 04, 28);
            EndDate.SelectedDate = DateTime.Now;
        }

        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            DataGridUsers.SelectedIndex = -1;
            ChoiceCar.SelectedIndex = -1;
            StartDate.SelectedDate = new DateTime(2022, 04, 28);
            EndDate.SelectedDate = DateTime.Now;
            UpdateChart();
        }
        private void ChoiceCar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateChart();
        }
        private void ComboChart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateChart();
        }
        private void DataGridUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateChart();
        }
        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateChart();
        }
        private void EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateChart();
        }
        private void UpdateChart()
        {
            Series currentSeries = ChartPayments.Series.FirstOrDefault();
            if(ComboChart.SelectedItem is SeriesChartType currentType)
            {
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();
            }


            if (DataGridUsers.SelectedItem is Users currentUsers && ChoiceCar.SelectedItem is Cars currentCars)
            {
                var filterBasket = baskets.Where(b => b.Car == currentCars.Id);               

                foreach (var order in filterBasket.Where(b => b.Orders.Users.Id == currentUsers.Id && b.Orders.Date >= StartDate.SelectedDate.Value && b.Orders.Date <= EndDate.SelectedDate.Value))
                {
                    currentSeries.Points.AddXY($"{order.Orders.Date.Value.Day}.{order.Orders.Date.Value.Month}.{order.Orders.Date.Value.Year}\n{order.Orders.Users.SurName} {order.Orders.Users.Name}", order.Orders.Ammount);
                }
            }
            else if (DataGridUsers.SelectedItem is Users onlyUser)
            {
                var filterUsers = orders.Where(o => o.Users.Id == onlyUser.Id && o.Date >= StartDate.SelectedDate.Value && o.Date <= EndDate.SelectedDate.Value);

                foreach (var order in filterUsers)
                {
                    currentSeries.Points.AddXY($"{order.Date.Value.Day}.{order.Date.Value.Month}.{order.Date.Value.Year}\n{order.Users.SurName} {order.Users.Name}", order.Ammount);
                }
            }
            else if (ChoiceCar.SelectedItem is Cars onlyCar)
            {
                var filterBasket = baskets.Where(b => b.Car == onlyCar.Id && b.Orders.Date >= StartDate.SelectedDate.Value && b.Orders.Date <= EndDate.SelectedDate.Value);
                foreach (var order in filterBasket)
                {
                    currentSeries.Points.AddXY($"{order.Orders.Date.Value.Day}.{order.Orders.Date.Value.Month}.{order.Orders.Date.Value.Year}\n{order.Orders.Users.SurName} {order.Orders.Users.Name}", order.Orders.Ammount);
                }
            }
            else
            {
                if(StartDate.SelectedDate != null && EndDate.SelectedDate != null)
                {
                    var filterDate = orders.Where(o => o.Date >= StartDate.SelectedDate.Value && o.Date <= EndDate.SelectedDate.Value);
                    foreach (var order in filterDate)
                    {
                        currentSeries.Points.AddXY($"{order.Date.Value.Day}.{order.Date.Value.Month}.{order.Date.Value.Year}\n{order.Users.SurName} {order.Users.Name}", order.Ammount);
                    }
                }
                else
                {
                    foreach (var order in orders)
                    {
                        currentSeries.Points.AddXY($"{order.Date.Value.Day}.{order.Date.Value.Month}.{order.Date.Value.Year}\n{order.Users.SurName} {order.Users.Name}", order.Ammount);
                    }
                }
            }
        }
    }
}
