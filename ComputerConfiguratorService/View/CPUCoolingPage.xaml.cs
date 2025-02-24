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
    /// Логика взаимодействия для CPUCoolingPage.xaml
    /// </summary>
    public partial class CPUCoolingPage : Page
    {
        private CPUCooling selectedCooling = null;
        public CPUCoolingPage()
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadCPUCooling();
        }
        private void LoadComboBoxes()
        {
            var context = DatabaseEntities.GetContext();
            cbManufacturer.ItemsSource = context.Manufacturers.ToList();
            cbCoolingType.ItemsSource = context.CoolingTypes.ToList();
        }
        private void LoadCPUCooling()
        {
            LVCPUCooling.ItemsSource = DatabaseEntities.GetContext().CPUCooling.ToList();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVCPUCooling.SelectedItem is CPUCooling cooling)
            {
                selectedCooling = cooling;
                cbManufacturer.SelectedValue = cooling.ManufacturerID;
                cbCoolingType.SelectedValue = cooling.CoolingTypeID;
                tbModel.Text = cooling.Model;
                tbMaxTDP.Text = cooling.MaxSupportedTDP.ToString();
                tbPrice.Text = cooling.Price.ToString();
                tbImagePath.Text = cooling.ImagePath;
            }
            else
            {
                selectedCooling = null;
                ClearForm();
            }
            EditPanel.Visibility = Visibility.Visible;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbManufacturer.SelectedValue == null || cbCoolingType.SelectedValue == null)
                {
                    MessageBox.Show("Выберите производителя и тип охлаждения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                int manufacturerID = (int)cbManufacturer.SelectedValue;
                int coolingTypeID = (int)cbCoolingType.SelectedValue;
                string model = tbModel.Text;
                int maxTDP = int.Parse(tbMaxTDP.Text);
                decimal price = decimal.Parse(tbPrice.Text);
                string imagePath = tbImagePath.Text;
                var context = DatabaseEntities.GetContext();
                if (selectedCooling == null)
                {
                    CPUCooling newCooling = new CPUCooling
                    {
                        ManufacturerID = manufacturerID,
                        CoolingTypeID = coolingTypeID,
                        Model = model,
                        MaxSupportedTDP = maxTDP,
                        Price = price,
                        ImagePath = imagePath
                    };
                    context.CPUCooling.Add(newCooling);
                }
                else
                {
                    selectedCooling.ManufacturerID = manufacturerID;
                    selectedCooling.CoolingTypeID = coolingTypeID;
                    selectedCooling.Model = model;
                    selectedCooling.MaxSupportedTDP = maxTDP;
                    selectedCooling.Price = price;
                    selectedCooling.ImagePath = imagePath;
                }
                context.SaveChanges();
                MessageBox.Show("Данные сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                EditPanel.Visibility = Visibility.Collapsed;
                LoadCPUCooling();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVCPUCooling.SelectedItem is CPUCooling cooling)
            {
                if (MessageBox.Show("Удалить выбранную систему охлаждения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = DatabaseEntities.GetContext();
                        context.CPUCooling.Remove(cooling);
                        context.SaveChanges();
                        MessageBox.Show("Система охлаждения удалена.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadCPUCooling();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите систему охлаждения для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
            cbCoolingType.SelectedIndex = -1;
            tbModel.Text = "";
            tbMaxTDP.Text = "";
            tbPrice.Text = "";
            tbImagePath.Text = "";
            selectedCooling = null;
        }
    }
}
