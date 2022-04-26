using System;
using System.Collections.Generic;
using System.IO;
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
using RegistrationSystem.Model;

namespace RegistrationSystem.View.MainView.ProductCatalog
{
    /// <summary>
    /// Логика взаимодействия для CreateNewProduct.xaml
    /// </summary>
    public partial class CreateNewProduct : UserControl
    {
        public CreateNewProduct()
        {
            InitializeComponent();
        }

        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
        private void DowlandPhoto(object sender, RoutedEventArgs e)
        {
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            Microsoft.Win32.OpenFileDialog ofdPicture = new Microsoft.Win32.OpenFileDialog();
            ofdPicture.Filter =
                "Image files|*.bmp;*.jpg;*.JPG;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                myBitmapImage.UriSource = new Uri(ofdPicture.FileName);
                myBitmapImage.EndInit();
                PhotoCar.Source = myBitmapImage;
            }
        }

        private void CreateCar_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new TestDataBaseEntities())
            {

                var checkBrand = context.Brands.SingleOrDefault(u => u.NameBrand == BrandText.Text);
                if(checkBrand == null)
                {
                    var brand = new Brands()
                    {
                        NameBrand = BrandText.Text
                    };
                    context.Brands.Add(brand);
                    context.SaveChanges();
                }

                var modelToBrand = context.Brands.SingleOrDefault(u => u.NameBrand == BrandText.Text);

                var checkModel = context.Models.SingleOrDefault(u => u.NameModel == ModelText.Text);
                if (checkModel == null)
                {
                    var model = new Models()
                    {
                        Brand = modelToBrand.Id,
                        NameModel = ModelText.Text
                    };
                    context.Models.Add(model);
                    context.SaveChanges();
                }



                var status = context.Statuses.SingleOrDefault(x => x.NameStatus == "Active");
                var Model = context.Models.SingleOrDefault(u => u.NameModel == ModelText.Text);

                var newCar = new Cars()
                {
                    Status = status.Id,
                    NameCar = Model.Id,
                    PhotoCar = getJPGFromImageControl(PhotoCar.Source as BitmapImage)
                };
                context.Cars.Add(newCar);
                context.SaveChanges();
                MessageBox.Show("Добавлено");
            }
        }
    }
}
