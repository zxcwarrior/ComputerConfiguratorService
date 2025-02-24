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
    /// Логика взаимодействия для RAMsPage.xaml
    /// </summary>
    public partial class RAMsPage : Page
    {
        private RAMs selectedRAM = null;

        public RAMsPage()
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadRAMs();
        }
        private void LoadComboBoxes()
        {
            var context = DatabaseEntities.GetContext();
            cbManufacturer.ItemsSource = context.Manufacturers.ToList();
            cbRAMType.ItemsSource = context.RAMTypes.ToList();
        }
        private void LoadRAMs()
        {
            LVRAMs.ItemsSource = DatabaseEntities.GetContext().RAMs.ToList();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVRAMs.SelectedItem is RAMs ram)
            {
                selectedRAM = ram;
                cbManufacturer.SelectedValue = ram.ManufacturerID;
                cbRAMType.SelectedValue = ram.RAMTypeID;
                tbModel.Text = ram.Model;
                tbCapacity.Text = ram.CapacityGB.ToString();
                tbSpeed.Text = ram.SpeedMHz.ToString();
                tbPrice.Text = ram.Price.ToString();
                tbImagePath.Text = ram.ImagePath;
            }
            else
            {
                selectedRAM = null;
                ClearForm();
            }
            EditPanel.Visibility = Visibility.Visible;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbManufacturer.SelectedValue == null || cbRAMType.SelectedValue == null)
                {
                    MessageBox.Show("Выберите производителя и тип RAM.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                int manufacturerID = (int)cbManufacturer.SelectedValue;
                int ramTypeID = (int)cbRAMType.SelectedValue;
                string model = tbModel.Text;
                int capacity = int.Parse(tbCapacity.Text);
                int speed = int.Parse(tbSpeed.Text);
                decimal price = decimal.Parse(tbPrice.Text);
                string imagePath = tbImagePath.Text;
                var context = DatabaseEntities.GetContext();
                if (selectedRAM == null)
                {
                    RAMs newRam = new RAMs
                    {
                        ManufacturerID = manufacturerID,
                        RAMTypeID = ramTypeID,
                        Model = model,
                        CapacityGB = capacity,
                        SpeedMHz = speed,
                        Price = price,
                        ImagePath = imagePath
                    };
                    context.RAMs.Add(newRam);
                }
                else
                {
                    selectedRAM.ManufacturerID = manufacturerID;
                    selectedRAM.RAMTypeID = ramTypeID;
                    selectedRAM.Model = model;
                    selectedRAM.CapacityGB = capacity;
                    selectedRAM.SpeedMHz = speed;
                    selectedRAM.Price = price;
                    selectedRAM.ImagePath = imagePath;
                }
                context.SaveChanges();
                MessageBox.Show("Данные сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                EditPanel.Visibility = Visibility.Collapsed;
                LoadRAMs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVRAMs.SelectedItem is RAMs ram)
            {
                if (MessageBox.Show("Удалить выбранную RAM?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = DatabaseEntities.GetContext();
                        context.RAMs.Remove(ram);
                        context.SaveChanges();
                        MessageBox.Show("RAM удалена.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadRAMs();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите RAM для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
            cbRAMType.SelectedIndex = -1;
            tbModel.Text = "";
            tbCapacity.Text = "";
            tbSpeed.Text = "";
            tbPrice.Text = "";
            tbImagePath.Text = "";
            selectedRAM = null;
        }
    }
}
