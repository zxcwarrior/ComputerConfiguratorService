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
    /// Логика взаимодействия для PowerSuppliesPage.xaml
    /// </summary>
    public partial class PowerSuppliesPage : Page
    {
        private PowerSupplies selectedPS = null;
        public PowerSuppliesPage()
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadPowerSupplies();
        }
        private void LoadComboBoxes()
        {
            var context = DatabaseEntities.GetContext();
            cbManufacturer.ItemsSource = context.Manufacturers.ToList();
            cbEfficiency.ItemsSource = context.EfficiencyRatings.ToList();
        }
        private void LoadPowerSupplies()
        {
            LVPowerSupplies.ItemsSource = DatabaseEntities.GetContext().PowerSupplies.ToList();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVPowerSupplies.SelectedItem is PowerSupplies ps)
            {
                selectedPS = ps;
                cbManufacturer.SelectedValue = ps.ManufacturerID;
                tbModel.Text = ps.Model;
                tbWattage.Text = ps.Wattage.ToString();
                cbEfficiency.SelectedValue = ps.EfficiencyRatingID;
                tbPrice.Text = ps.Price.ToString();
                tbImagePath.Text = ps.ImagePath;
            }
            else
            {
                selectedPS = null;
                ClearForm();
            }
            EditPanel.Visibility = Visibility.Visible;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbManufacturer.SelectedValue == null || cbEfficiency.SelectedValue == null)
                {
                    MessageBox.Show("Выберите производителя и рейтинг эффективности.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                int manufacturerID = (int)cbManufacturer.SelectedValue;
                string model = tbModel.Text;
                int wattage = int.Parse(tbWattage.Text);
                int efficiencyID = (int)cbEfficiency.SelectedValue;
                decimal price = decimal.Parse(tbPrice.Text);
                string imagePath = tbImagePath.Text;
                var context = DatabaseEntities.GetContext();
                if (selectedPS == null)
                {
                    PowerSupplies newPS = new PowerSupplies
                    {
                        ManufacturerID = manufacturerID,
                        Model = model,
                        Wattage = wattage,
                        EfficiencyRatingID = efficiencyID,
                        Price = price,
                        ImagePath = imagePath
                    };
                    context.PowerSupplies.Add(newPS);
                }
                else
                {
                    selectedPS.ManufacturerID = manufacturerID;
                    selectedPS.Model = model;
                    selectedPS.Wattage = wattage;
                    selectedPS.EfficiencyRatingID = efficiencyID;
                    selectedPS.Price = price;
                    selectedPS.ImagePath = imagePath;
                }
                context.SaveChanges();
                MessageBox.Show("Данные сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                EditPanel.Visibility = Visibility.Collapsed;
                LoadPowerSupplies();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVPowerSupplies.SelectedItem is PowerSupplies ps)
            {
                if (MessageBox.Show("Удалить выбранный блок питания?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = DatabaseEntities.GetContext();
                        context.PowerSupplies.Remove(ps);
                        context.SaveChanges();
                        MessageBox.Show("Блок питания удалён.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadPowerSupplies();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите блок питания для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
            cbEfficiency.SelectedIndex = -1;
            tbModel.Text = "";
            tbWattage.Text = "";
            tbPrice.Text = "";
            tbImagePath.Text = "";
            selectedPS = null;
        }
    }
}