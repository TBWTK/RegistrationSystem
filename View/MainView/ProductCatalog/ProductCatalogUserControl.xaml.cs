using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RegistrationSystem.Model;


namespace RegistrationSystem.View.MainView.ProductCatalog
{
    /// <summary>
    /// Логика взаимодействия для ProductCatalogUserControl.xaml
    /// </summary>
    public partial class ProductCatalogUserControl : UserControl
    {
        public List<Cars> cars { get; set; }
        public List<Brands> brands { get; set; }
        public List<Models> models { get; set; }

        private int IndexItemCatalog = 1;

        public ProductCatalogUserControl()
        {
            InitializeComponent();
            cars = new List<Cars>();

            using (var context = new TestDataBaseEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;
                cars = context.Cars.ToList();
                brands = context.Brands.ToList();
                models = context.Models.ToList();
            }

            BrandComboBox.ItemsSource = brands;
            ModelComboBox.ItemsSource = models;

            foreach(var car in cars)
            {
                foreach(var model in models)
                {
                    if(car.NameCar == model.Id)
                    {
                        car.ModelCar = model.NameModel;
                        foreach(var brand in brands)
                        {
                            if(model.Brand == brand.Id)
                            {
                                car.BrandCar = brand.NameBrand;
                            }
                        }
                    }
                }
            }

            ProductCatalog.ItemsSource = cars;
        }
        private void OneProductDisplay_Click(object sender, RoutedEventArgs e)
        {
            ButtonHadlerOneProductDisplay.Visibility = Visibility.Visible;
            ButtonHadlerFourProductDisplay.Visibility = Visibility.Hidden;
            ClearSearchPanels();
        }

        private void FourProductDisplay_Click(object sender, RoutedEventArgs e)
        {
            ButtonHadlerOneProductDisplay.Visibility = Visibility.Hidden;
            ButtonHadlerFourProductDisplay.Visibility = Visibility.Visible;
            ClearSearchPanels();
        }

        private void ListProductDisplay_Click(object sender, RoutedEventArgs e)
        {
            ButtonHadlerOneProductDisplay.Visibility = Visibility.Hidden;
            ButtonHadlerFourProductDisplay.Visibility = Visibility.Hidden;
            ClearSearchPanels();
        }

        private void ButtonChoice_Click(object sender, RoutedEventArgs e)
        {
            Cars curItem = (Cars)((ListBoxItem)ProductCatalog.ContainerFromElement((Button)sender)).Content;
            MessageBox.Show(Convert.ToString(curItem.Id));
        }

        private Object LinqSearchCars()
        {
            var filtered = cars.Where(c => c.Id > 0);
            Brands selectedItemBrand = new Brands();
            Models selectedItemModel = new Models();

            if (TextSliderStart.Text != null || TextSliderEnd != null)
            {
                filtered = filtered.Where(c => c.Price > Convert.ToDouble(TextSliderStart.Text) && c.Price < Convert.ToDouble(TextSliderEnd.Text));
            }

            int selectedIndexBrand = BrandComboBox.SelectedIndex;
            if (selectedIndexBrand > -1)
            {
                selectedItemBrand = (Brands)BrandComboBox.SelectedItem;
                filtered = filtered.Where(c => c.BrandCar.StartsWith(selectedItemBrand.NameBrand));
            }

            int selectedIndexModel = ModelComboBox.SelectedIndex;
            if (selectedIndexModel > -1)
            {
                selectedItemModel = (Models)ModelComboBox.SelectedItem;
                filtered = filtered.Where(c => c.ModelCar.StartsWith(selectedItemModel.NameModel));
            }

            if (OneProductDisplay.IsChecked == true)
            {


                filtered = filtered.Where(u => u.Id == IndexItemCatalog && u.ModelCar.StartsWith(selectedItemModel.NameModel));

            }
            else if (FourProductDisplay.IsChecked == true)
            {
                filtered = filtered.Where(u => u.Id >= IndexItemCatalog && u.Id < IndexItemCatalog + 4);
            }
            else
            {
                ProductCatalog.ItemsSource = filtered;
            }

            return filtered;
        }

        private void ButtonPreviousOneProduct_Click(object sender, RoutedEventArgs e)
        {
            if (IndexItemCatalog > 0)
            {
                IndexItemCatalog--;
                var filtered = LinqSearchCars();
                //filtered = (List<Cars>)filtered.Where(c => c.Id == IndexItemCatalog);

                ProductCatalog.ItemsSource = (System.Collections.IEnumerable)filtered;
            }
        }

        private void ButtonNextOneProduct_Click(object sender, RoutedEventArgs e)
        {
            if (IndexItemCatalog < cars.Count() - 1)
            {
                IndexItemCatalog++;

                //var ChoiceProduct = cars.Where(c => c.Id == IndexItemCatalog);
                //ProductCatalog.ItemsSource = ChoiceProduct;
                
                var filtered = LinqSearchCars();
                ProductCatalog.ItemsSource = (System.Collections.IEnumerable)filtered;
            }
        }

        private void ButtonPreviousFourProduct_Click(object sender, RoutedEventArgs e)
        {
            if (IndexItemCatalog < cars.Count() - 1)
                IndexItemCatalog = 1;
            else
                IndexItemCatalog -= 4;
            //var ChoiceProduct = cars.Where(u => u.Id >= IndexItemCatalog && u.Id < IndexItemCatalog + 4);
            //ProductCatalog.ItemsSource = ChoiceProduct;
            var filtered = LinqSearchCars();
            ProductCatalog.ItemsSource = (System.Collections.IEnumerable)filtered;
        }

        private void ButtonNextFourProduct_Click(object sender, RoutedEventArgs e)
        {
            if (IndexItemCatalog < cars.Count() - 1)
            {
                IndexItemCatalog += 4;
                if (IndexItemCatalog > cars.Count() - 1)
                    IndexItemCatalog -= 4;
                //var ChoiceProduct = cars.Where(u => u.Id >= IndexItemCatalog && u.Id < IndexItemCatalog + 4);
                //ProductCatalog.ItemsSource = ChoiceProduct;
                var filtered = LinqSearchCars();
                ProductCatalog.ItemsSource = (System.Collections.IEnumerable)filtered;
            }
        }

        private void ClearSearchPanels()
        {
            IndexItemCatalog = 1;
            if (OneProductDisplay.IsChecked == true)
            {
                var ChoiceProduct = cars.Where(u => u.Id == IndexItemCatalog);
                ProductCatalog.ItemsSource = ChoiceProduct;

            }
            else if (FourProductDisplay.IsChecked == true)
            {
                var ChoiceProduct = cars.Where(u => u.Id >= IndexItemCatalog && u.Id < IndexItemCatalog + 4);
                ProductCatalog.ItemsSource = ChoiceProduct;
            }
            else
            {
                ProductCatalog.ItemsSource = cars;
            }
            BrandComboBox.SelectedIndex = -1;
            ModelComboBox.SelectedIndex = -1;
            SliderStart.Value = 0;
            SliderEnd.Value = 0;
            TextSliderStart.Text = "0";
            TextSliderEnd.Text = "100";
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchPanels();
        }

        private void BrandComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //int selectedIndex = BrandComboBox.SelectedIndex;
            //if (selectedIndex > -1)
            //{

            //    Brands selectedItem = (Brands)BrandComboBox.SelectedItem;
            //    var filtered = cars.Where(c => c.BrandCar.StartsWith(selectedItem.NameBrand));
            //    ProductCatalog.ItemsSource = cars;
            //}
            var filtered = LinqSearchCars();
            ProductCatalog.ItemsSource = (System.Collections.IEnumerable)filtered;
        }

        private void ModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //int selectedIndex = ModelComboBox.SelectedIndex;
            //if (selectedIndex > -1)
            //{
            //    Models selectedItem = (Models)ModelComboBox.SelectedItem;
            //    var filtered = cars.Where(c => c.ModelCar.StartsWith(selectedItem.NameModel));
            //    ProductCatalog.ItemsSource = filtered;
            //}
            var filtered = LinqSearchCars();
            ProductCatalog.ItemsSource = (System.Collections.IEnumerable)filtered;
        }

        private void SliderStart_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((Slider)sender).SelectionEnd = e.NewValue;
            TextSliderStart.Text = $"{e.NewValue:0.00}";
        }

        private void SliderEnd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((Slider)sender).SelectionEnd = e.NewValue;
            TextSliderEnd.Text = $"{e.NewValue:0.00}";
        }

        private void SearchPrice_Click(object sender, RoutedEventArgs e)
        {
            //var filtered = cars.Where(c => c.Price > Convert.ToDouble(TextSliderStart.Text) && c.Price < Convert.ToDouble(TextSliderEnd.Text));
            //ProductCatalog.ItemsSource = filtered;
            var filtered = LinqSearchCars();
            ProductCatalog.ItemsSource = (System.Collections.IEnumerable)filtered;
        }
    }
}
