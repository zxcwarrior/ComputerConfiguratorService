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
    /// Логика взаимодействия для StoragesPage.xaml
    /// </summary>
    public partial class StoragesPage : Page
    {
        private Storages selectedStorage = null;
        public StoragesPage()
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadStorages();
        }
        private void LoadComboBoxes()
        {
            var context = DatabaseEntities.GetContext();
            cbManufacturer.ItemsSource = context.Manufacturers.ToList();
            cbStorageType.ItemsSource = context.StorageTypes.ToList();
        }
        private void LoadStorages()
        {
            LVStorages.ItemsSource = DatabaseEntities.GetContext().Storages.ToList();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVStorages.SelectedItem is Storages storage)
            {
                selectedStorage = storage;
                cbManufacturer.SelectedValue = storage.ManufacturerID;
                cbStorageType.SelectedValue = storage.StorageTypeID;
                tbModel.Text = storage.Model;
                tbCapacity.Text = storage.CapacityGB.ToString();
                tbPrice.Text = storage.Price.ToString();
                tbImagePath.Text = storage.ImagePath;
            }
            else
            {
                selectedStorage = null;
                ClearForm();
            }
            EditPanel.Visibility = Visibility.Visible;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbManufacturer.SelectedValue == null || cbStorageType.SelectedValue == null)
                {
                    MessageBox.Show("Выберите производителя и тип хранилища.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                int manufacturerID = (int)cbManufacturer.SelectedValue;
                int storageTypeID = (int)cbStorageType.SelectedValue;
                string model = tbModel.Text;
                int capacity = int.Parse(tbCapacity.Text);
                decimal price = decimal.Parse(tbPrice.Text);
                string imagePath = tbImagePath.Text;
                var context = DatabaseEntities.GetContext();
                if (selectedStorage == null)
                {
                    Storages newStorage = new Storages
                    {
                        ManufacturerID = manufacturerID,
                        StorageTypeID = storageTypeID,
                        Model = model,
                        CapacityGB = capacity,
                        Price = price,
                        ImagePath = imagePath
                    };
                    context.Storages.Add(newStorage);
                }
                else
                {
                    selectedStorage.ManufacturerID = manufacturerID;
                    selectedStorage.StorageTypeID = storageTypeID;
                    selectedStorage.Model = model;
                    selectedStorage.CapacityGB = capacity;
                    selectedStorage.Price = price;
                    selectedStorage.ImagePath = imagePath;
                }
                context.SaveChanges();
                MessageBox.Show("Данные сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                EditPanel.Visibility = Visibility.Collapsed;
                LoadStorages();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVStorages.SelectedItem is Storages storage)
            {
                if (MessageBox.Show("Удалить выбранное хранилище?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = DatabaseEntities.GetContext();
                        context.Storages.Remove(storage);
                        context.SaveChanges();
                        MessageBox.Show("Хранилище удалено.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadStorages();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите хранилище для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
            cbStorageType.SelectedIndex = -1;
            tbModel.Text = "";
            tbCapacity.Text = "";
            tbPrice.Text = "";
            tbImagePath.Text = "";
            selectedStorage = null;
        }
    }
}
