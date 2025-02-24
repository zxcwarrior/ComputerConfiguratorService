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
using ComputerConfiguratorService.Model;

namespace ComputerConfiguratorService.View
{
    /// <summary>
    /// Логика взаимодействия для MotherboardsPage.xaml
    /// </summary>
    public partial class MotherboardsPage : Page
    {
        private Motherboards selectedMB = null;
        public MotherboardsPage()
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadMotherboards();
        }
        private void LoadComboBoxes()
        {
            var context = DatabaseEntities.GetContext();
            cbManufacturer.ItemsSource = context.Manufacturers.ToList();
            cbSocket.ItemsSource = context.Sockets.ToList();
            cbRAMType.ItemsSource = context.RAMTypes.ToList();
            cbFormFactor.ItemsSource = context.CaseFormFactors.ToList();
        }
        private void LoadMotherboards()
        {
            LVMotherboards.ItemsSource = DatabaseEntities.GetContext().Motherboards.ToList();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVMotherboards.SelectedItem is Motherboards mb)
            {
                selectedMB = mb;
                cbManufacturer.SelectedValue = mb.ManufacturerID;
                cbSocket.SelectedValue = mb.SocketID;
                cbRAMType.SelectedValue = mb.RAMTypeID;
                cbFormFactor.SelectedValue = mb.MotherboardFFID;
                tbModel.Text = mb.Model;
                tbPrice.Text = mb.Price.ToString();
                tbImagePath.Text = mb.ImagePath;
            }
            else
            {
                selectedMB = null;
                ClearForm();
            }
            EditPanel.Visibility = Visibility.Visible;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbManufacturer.SelectedValue == null || cbSocket.SelectedValue == null ||
                    cbRAMType.SelectedValue == null || cbFormFactor.SelectedValue == null)
                {
                    MessageBox.Show("Выберите все обязательные параметры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                int manufacturerID = (int)cbManufacturer.SelectedValue;
                int socketID = (int)cbSocket.SelectedValue;
                int ramTypeID = (int)cbRAMType.SelectedValue;
                int formFactorID = (int)cbFormFactor.SelectedValue;
                string model = tbModel.Text;
                decimal price = decimal.Parse(tbPrice.Text);
                string imagePath = tbImagePath.Text;
                var context = DatabaseEntities.GetContext();
                if (selectedMB == null)
                {
                    Motherboards newMB = new Motherboards
                    {
                        ManufacturerID = manufacturerID,
                        SocketID = socketID,
                        RAMTypeID = ramTypeID,
                        MotherboardFFID = formFactorID,
                        Model = model,
                        Price = price,
                        ImagePath = imagePath
                    };
                    context.Motherboards.Add(newMB);
                }
                else
                {
                    selectedMB.ManufacturerID = manufacturerID;
                    selectedMB.SocketID = socketID;
                    selectedMB.RAMTypeID = ramTypeID;
                    selectedMB.MotherboardFFID = formFactorID;
                    selectedMB.Model = model;
                    selectedMB.Price = price;
                    selectedMB.ImagePath = imagePath;
                }
                context.SaveChanges();
                MessageBox.Show("Данные сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                EditPanel.Visibility = Visibility.Collapsed;
                LoadMotherboards();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVMotherboards.SelectedItem is Motherboards mb)
            {
                if (MessageBox.Show("Удалить выбранную материнскую плату?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = DatabaseEntities.GetContext();
                        context.Motherboards.Remove(mb);
                        context.SaveChanges();
                        MessageBox.Show("Материнская плата удалена.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadMotherboards();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите материнскую плату для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            EditPanel.Visibility = Visibility.Collapsed;
        }
        private void ClearForm()
        {
            cbManufacturer.SelectedIndex = -1;
            cbSocket.SelectedIndex = -1;
            cbRAMType.SelectedIndex = -1;
            cbFormFactor.SelectedIndex = -1;
            tbModel.Text = "";
            tbPrice.Text = "";
            tbImagePath.Text = "";
            selectedMB = null;
        }
    }
}
