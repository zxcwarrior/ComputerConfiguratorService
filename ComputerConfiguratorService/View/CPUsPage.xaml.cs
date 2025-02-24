using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ComputerConfiguratorService.Model;

namespace ComputerConfiguratorService.View
{
    public partial class CPUsPage : Page
    {
        // Текущий выбранный процессор для редактирования. Если null – создаётся новая запись.
        private CPUs selectedCPU = null;

        public CPUsPage()
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadCPUs();
        }

        // Загрузка списков производителей и сокетов для ComboBox
        private void LoadComboBoxes()
        {
            var context = DatabaseEntities.GetContext();
            cbManufacturer.ItemsSource = context.Manufacturers.ToList();
            cbSocket.ItemsSource = context.Sockets.ToList();
        }

        // Загрузка списка процессоров
        private void LoadCPUs()
        {
            LVCPUs.ItemsSource = DatabaseEntities.GetContext().CPUs.ToList();
        }

        // Обработчик кнопки "Редактировать"
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Если выбран элемент – заполняем форму для редактирования
            if (LVCPUs.SelectedItem is CPUs cpu)
            {
                selectedCPU = cpu;
                cbManufacturer.SelectedValue = cpu.ManufacturerID;
                cbSocket.SelectedValue = cpu.SocketID;
                tbModel.Text = cpu.Model;
                tbCores.Text = cpu.Cores.ToString();
                tbThreads.Text = cpu.Threads.ToString();
                tbBaseClock.Text = cpu.BaseClock.ToString();
                tbBoostClock.Text = cpu.BoostClock.ToString();
                tbTDP.Text = cpu.TDP.ToString();
                tbPrice.Text = cpu.Price.ToString();
                tbImagePath.Text = cpu.ImagePath;
            }
            else
            {
                // Если не выбран элемент – очищаем форму для добавления нового процессора
                selectedCPU = null;
                ClearForm();
            }
            EditPanel.Visibility = Visibility.Visible;
        }

        // Обработчик кнопки "Сохранить" (создание или обновление)
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация и получение выбранных значений из ComboBox
                if (cbManufacturer.SelectedValue == null || cbSocket.SelectedValue == null)
                {
                    MessageBox.Show("Выберите производителя и сокет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int manufacturerID = (int)cbManufacturer.SelectedValue;
                int socketID = (int)cbSocket.SelectedValue;
                string model = tbModel.Text;
                int cores = int.Parse(tbCores.Text);
                int threads = int.Parse(tbThreads.Text);
                decimal baseClock = decimal.Parse(tbBaseClock.Text);
                decimal boostClock = decimal.Parse(tbBoostClock.Text);
                int tdp = int.Parse(tbTDP.Text);
                decimal price = decimal.Parse(tbPrice.Text);
                string imagePath = tbImagePath.Text;

                var context = DatabaseEntities.GetContext();

                if (selectedCPU == null)
                {
                    // Создание новой записи
                    CPUs newCpu = new CPUs
                    {
                        ManufacturerID = manufacturerID,
                        SocketID = socketID,
                        Model = model,
                        Cores = cores,
                        Threads = threads,
                        BaseClock = baseClock,
                        BoostClock = boostClock,
                        TDP = tdp,
                        Price = price,
                        ImagePath = imagePath
                    };
                    context.CPUs.Add(newCpu);
                }
                else
                {
                    // Обновление существующей записи
                    selectedCPU.ManufacturerID = manufacturerID;
                    selectedCPU.SocketID = socketID;
                    selectedCPU.Model = model;
                    selectedCPU.Cores = cores;
                    selectedCPU.Threads = threads;
                    selectedCPU.BaseClock = baseClock;
                    selectedCPU.BoostClock = boostClock;
                    selectedCPU.TDP = tdp;
                    selectedCPU.Price = price;
                    selectedCPU.ImagePath = imagePath;
                }
                context.SaveChanges();
                MessageBox.Show("Данные сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                EditPanel.Visibility = Visibility.Collapsed;
                LoadCPUs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик кнопки "Удалить"
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVCPUs.SelectedItem is CPUs cpu)
            {
                var result = MessageBox.Show("Вы действительно хотите удалить этот процессор?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = DatabaseEntities.GetContext();
                        context.CPUs.Remove(cpu);
                        context.SaveChanges();
                        MessageBox.Show("Процессор удален.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadCPUs();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите процессор для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Обработчик кнопки "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            EditPanel.Visibility = Visibility.Collapsed;
        }

        // Метод очистки полей ввода
        private void ClearForm()
        {
            cbManufacturer.SelectedIndex = -1;
            cbSocket.SelectedIndex = -1;
            tbModel.Text = "";
            tbCores.Text = "";
            tbThreads.Text = "";
            tbBaseClock.Text = "";
            tbBoostClock.Text = "";
            tbTDP.Text = "";
            tbPrice.Text = "";
            tbImagePath.Text = "";
            selectedCPU = null;
        }
    }
}
