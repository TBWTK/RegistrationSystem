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

        private List<Cars> carsHandler { get; set; }
        private List<int> IdCars { get; set; }
     

        private int IndexItemCatalog = 0;

        private int IdUser = 0;

        public ProductCatalogUserControl(int IDUSER)
        {
            InitializeComponent();
            cars = new List<Cars>();
            carsHandler = new List<Cars>();

            IdCars = new List<int>();

            IdUser = IDUSER;

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


        // Событие выбора отображения товаров на панели
        private void OneProductDisplay_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate template = (DataTemplate)this.Resources["listTemplateBig"];
            ProductCatalog.ItemTemplate = template;

            ButtonHadlerOneProductDisplay.Visibility = Visibility.Visible;
            ButtonHadlerFourProductDisplay.Visibility = Visibility.Hidden;
            ClearSearchPanels();
        }
        private void FourProductDisplay_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate template = (DataTemplate)this.Resources["listTemplateSmall"];
            ProductCatalog.ItemTemplate = template;

            ButtonHadlerOneProductDisplay.Visibility = Visibility.Hidden;
            ButtonHadlerFourProductDisplay.Visibility = Visibility.Visible;
            ClearSearchPanels();
        }
        private void ListProductDisplay_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate template = (DataTemplate)this.Resources["listTemplateSmall"];
            ProductCatalog.ItemTemplate = template;

            ButtonHadlerOneProductDisplay.Visibility = Visibility.Hidden;
            ButtonHadlerFourProductDisplay.Visibility = Visibility.Hidden;
            ClearSearchPanels();


        }


        // Событие корзины
        private void ButtonChoice_Click(object sender, RoutedEventArgs e)
        {
            Cars curItem = (Cars)((ListBoxItem)ProductCatalog.ContainerFromElement((Button)sender)).Content;
            MessageBox.Show(Convert.ToString(curItem.Id));
        }

        // Функция по выгрузке в лист айдишников машин 
        private void LoadIdFromIndex()
        {
            IdCars.Clear();
            IndexItemCatalog = 0;
            foreach (var item in carsHandler)
            {
                IdCars.Add(item.Id);
            }
        }

        // Событие переключения отображения ОДНОГО товаров
        private void ButtonPreviousOneProduct_Click(object sender, RoutedEventArgs e)
        {
            if (IndexItemCatalog < 1)
                IndexItemCatalog = 0;
            else
                IndexItemCatalog--;

            //var carsItems = ProductCatalog.ItemsSource.Cast<Cars>();
            var filtered = carsHandler.Where(c => c.Id == IdCars[IndexItemCatalog]);
            ProductCatalog.ItemsSource = filtered;
        }
        private void ButtonNextOneProduct_Click(object sender, RoutedEventArgs e)
        {
            //var carsItems = ProductCatalog.ItemsSource.Cast<Cars>();

            IndexItemCatalog++;

            if (IndexItemCatalog < IdCars.Count())
            {
                var filtered = carsHandler.Where(c => c.Id == IdCars[IndexItemCatalog]);
                ProductCatalog.ItemsSource = filtered;
            }
            else
                IndexItemCatalog--;
        }


        // Событие переключения отображения ЧЕТЫРЕХ товаров
        private void ButtonPreviousFourProduct_Click(object sender, RoutedEventArgs e)
        {
            int numberForCorrectDisplayDuringTransitions = 5;
            int numberToDisplayWhenEmptyClicks = 3;
            int numberForAccessingProductsByIndex = 3;


            if (IndexItemCatalog - numberForAccessingProductsByIndex >= 0)
            {
                IndexItemCatalog -= numberForAccessingProductsByIndex;
            }

            if (IndexItemCatalog - numberForAccessingProductsByIndex >= 0)
            {
                var filtered = carsHandler.Where(u => u.Id > IdCars[IndexItemCatalog] && u.Id < IdCars[IndexItemCatalog + numberForCorrectDisplayDuringTransitions]);
                ProductCatalog.ItemsSource = filtered;
            }
            else
            {
                if (IndexItemCatalog + numberToDisplayWhenEmptyClicks < IdCars.Count())
                {
                    var filtered = carsHandler.Where(u => u.Id >= IdCars[IndexItemCatalog] && u.Id <= IdCars[IndexItemCatalog + numberToDisplayWhenEmptyClicks]);
                    ProductCatalog.ItemsSource = filtered;
                }
                else
                {
                    var filtered = carsHandler.Where(u => u.Id >= IdCars[IndexItemCatalog] && u.Id <= IdCars[IdCars.Count() - 1]);
                    ProductCatalog.ItemsSource = filtered;
                }
            }

        }
        private void ButtonNextFourProduct_Click(object sender, RoutedEventArgs e)
        {
            int numberForCorrectDisplayDuringTransitions = 5;
            int numberToDisplayFinalProducts = 1;
            int numberForAccessingProductsByIndex = 3;

            if (IndexItemCatalog + numberForAccessingProductsByIndex < IdCars.Count() - 1)
            {
                IndexItemCatalog += numberForAccessingProductsByIndex;
                if (IndexItemCatalog + numberForAccessingProductsByIndex > IdCars.Count() - 1)
                {
                    IndexItemCatalog -= numberForAccessingProductsByIndex;
                }
            }

            if (IndexItemCatalog + numberForCorrectDisplayDuringTransitions < IdCars.Count())
            {
                var filtered = carsHandler.Where(u => u.Id > IdCars[IndexItemCatalog] && u.Id < IdCars[IndexItemCatalog + numberForCorrectDisplayDuringTransitions]);
                ProductCatalog.ItemsSource = filtered;
            }
            else
            {
                if(IndexItemCatalog + numberToDisplayFinalProducts < IdCars.Count() - 1)
                {
                    var filtered = carsHandler.Where(u => u.Id > IdCars[IndexItemCatalog + numberToDisplayFinalProducts] && u.Id <= IdCars[IdCars.Count() - 1]);
                    ProductCatalog.ItemsSource = filtered;
                }
                else
                {
                    var filtered = carsHandler.Where(u => u.Id >= IdCars[IndexItemCatalog] && u.Id <= IdCars[IdCars.Count() - 1]);
                    ProductCatalog.ItemsSource = filtered;
                }

            }
        }


        // Выбор бренда и открытие выбора модели машины
        private void BrandComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = BrandComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                // Выгрузка в объект выбранного экземпляра
                Brands selectedItem = (Brands)BrandComboBox.SelectedItem;
                
                // Обновление ComboBox для вбора моделей на основе выбранного бренда
                var filteredModel = models.Where(c => c.Brand == selectedItem.Id);
                ModelComboBox.ItemsSource = filteredModel;

                // Обновление лист бокса на основе выбранного бренда
                var filtered = cars.Where(c => c.BrandCar.StartsWith(selectedItem.NameBrand));

                // выгрузка из лист бокса в список помощник для пролистывания
                carsHandler = filtered.ToList();

                LoadIdFromIndex();

                if (OneProductDisplay.IsChecked == true)
                {
                    var filteredOneProduct = carsHandler.Where(c => c.Id == IdCars[IndexItemCatalog]);

                    ProductCatalog.ItemsSource = filteredOneProduct;
                }
                else if (FourProductDisplay.IsChecked == true)
                {
                    if (IdCars.Count() < 4)
                    {
                        var filteredFourProduct = carsHandler.Where(u => u.Id >= IdCars[IndexItemCatalog] && u.Id <= IdCars[IdCars.Count() - 1]);
                        ProductCatalog.ItemsSource = filteredFourProduct;
                    }
                    else
                    {
                        int numberForCorrectDisplayDuringTransitions = 3;

                        var filteredFourProduct = carsHandler.Where(u => u.Id >= IdCars[IndexItemCatalog] && u.Id <= IdCars[IndexItemCatalog + numberForCorrectDisplayDuringTransitions]);
                        ProductCatalog.ItemsSource = filteredFourProduct;
                    }
                }
                else
                {
                    ProductCatalog.ItemsSource = carsHandler;
                }
            }
        }
        private void ModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = BrandComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                Models selectedItem = (Models)ModelComboBox.SelectedItem;
                var filtered = carsHandler.Where(c => c.ModelCar.StartsWith(selectedItem.NameModel));
                ProductCatalog.ItemsSource = filtered;
                LoadIdFromIndex();
            }
        }


        // Переключение значений слайдеров цены и событие поиска
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
            var filtered = cars.Where(c => c.Price > Convert.ToDouble(TextSliderStart.Text) && c.Price < Convert.ToDouble(TextSliderEnd.Text));
            ProductCatalog.ItemsSource = filtered;
        }


        // Событие отчищениия поиска
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchPanels();
        }

        // Функция отчистки поиска
        private void ClearSearchPanels()
        {
            BrandComboBox.SelectedIndex = -1;
            ModelComboBox.SelectedIndex = -1;

            ModelComboBox.ItemsSource = models;
            ProductCatalog.ItemsSource = cars;

            carsHandler = (List<Cars>)ProductCatalog.ItemsSource.Cast<Cars>();


            if (OneProductDisplay.IsChecked == true)
            {
                var filtered = cars.Where(c => c.Id == 1);
                ProductCatalog.ItemsSource = filtered;
            }
            else if(FourProductDisplay.IsChecked == true)
            {
                var filtered = cars.Where(u => u.Id >= 1 && u.Id < 5);
                ProductCatalog.ItemsSource = filtered;
            }

            LoadIdFromIndex();


            SliderStart.Value = 0;
            SliderEnd.Value = 100;
            TextSliderStart.Text = "0,00";
            TextSliderEnd.Text = "100,00";
        }

    }
}
