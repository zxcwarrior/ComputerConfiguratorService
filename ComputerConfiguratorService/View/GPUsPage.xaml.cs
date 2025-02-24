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
    /// Логика взаимодействия для GPUsPage.xaml
    /// </summary>
    public partial class GPUsPage : Page
    {
        private GPUs selectedGPU = null;
        public GPUsPage()
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadGPUs();
        }
        private void LoadComboBoxes()
        {
            var context = DatabaseEntities.GetContext();
            cbManufacturer.ItemsSource = context.Manufacturers.ToList();
            cbVendor.ItemsSource = context.Vendors.ToList();
            cbMemoryType.ItemsSource = context.GPUMemoryTypes.ToList();
        }
        private void LoadGPUs()
        {
            LVGPUs.ItemsSource = DatabaseEntities.GetContext().GPUs.ToList();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVGPUs.SelectedItem is GPUs gpu)
            {
                selectedGPU = gpu;
                cbManufacturer.SelectedValue = gpu.ManufacturerID;
                cbVendor.SelectedValue = gpu.VendorID;
                tbModel.Text = gpu.Model;
                cbMemoryType.SelectedValue = gpu.GPUMemoryTypeID;
                tbMemoryGB.Text = gpu.MemoryGB.ToString();
                tbCoreClock.Text = gpu.CoreClock.ToString();
                tbGPULength.Text = gpu.GPULength.ToString();
                tbPowerConsumption.Text = gpu.PowerConsumption.ToString();
                tbPrice.Text = gpu.Price.ToString();
                tbImagePath.Text = gpu.ImagePath;
            }
            else
            {
                selectedGPU = null;
                ClearForm();
            }
            EditPanel.Visibility = Visibility.Visible;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbManufacturer.SelectedValue == null || cbVendor.SelectedValue == null || cbMemoryType.SelectedValue == null)
                {
                    MessageBox.Show("Выберите производителя, вендора и тип памяти.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                int manufacturerID = (int)cbManufacturer.SelectedValue;
                int vendorID = (int)cbVendor.SelectedValue;
                string model = tbModel.Text;
                int gpuMemoryTypeID = (int)cbMemoryType.SelectedValue;
                int memoryGB = int.Parse(tbMemoryGB.Text);
                int coreClock = int.Parse(tbCoreClock.Text);
                int gpuLength = int.Parse(tbGPULength.Text);
                int powerConsumption = int.Parse(tbPowerConsumption.Text);
                decimal price = decimal.Parse(tbPrice.Text);
                string imagePath = tbImagePath.Text;
                var context = DatabaseEntities.GetContext();
                if (selectedGPU == null)
                {
                    GPUs newGPU = new GPUs
                    {
                        ManufacturerID = manufacturerID,
                        VendorID = vendorID,
                        Model = model,
                        GPUMemoryTypeID = gpuMemoryTypeID,
                        MemoryGB = memoryGB,
                        CoreClock = coreClock,
                        GPULength = gpuLength,
                        PowerConsumption = powerConsumption,
                        Price = price,
                        ImagePath = imagePath
                    };
                    context.GPUs.Add(newGPU);
                }
                else
                {
                    selectedGPU.ManufacturerID = manufacturerID;
                    selectedGPU.VendorID = vendorID;
                    selectedGPU.Model = model;
                    selectedGPU.GPUMemoryTypeID = gpuMemoryTypeID;
                    selectedGPU.MemoryGB = memoryGB;
                    selectedGPU.CoreClock = coreClock;
                    selectedGPU.GPULength = gpuLength;
                    selectedGPU.PowerConsumption = powerConsumption;
                    selectedGPU.Price = price;
                    selectedGPU.ImagePath = imagePath;
                }
                context.SaveChanges();
                MessageBox.Show("Данные сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                EditPanel.Visibility = Visibility.Collapsed;
                LoadGPUs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVGPUs.SelectedItem is GPUs gpu)
            {
                if (MessageBox.Show("Удалить выбранную видеокарту?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = DatabaseEntities.GetContext();
                        context.GPUs.Remove(gpu);
                        context.SaveChanges();
                        MessageBox.Show("Видеокарта удалена.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadGPUs();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите видеокарту для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
            cbVendor.SelectedIndex = -1;
            cbMemoryType.SelectedIndex = -1;
            tbModel.Text = "";
            tbMemoryGB.Text = "";
            tbCoreClock.Text = "";
            tbGPULength.Text = "";
            tbPowerConsumption.Text = "";
            tbPrice.Text = "";
            tbImagePath.Text = "";
            selectedGPU = null;
        }
    }
}
