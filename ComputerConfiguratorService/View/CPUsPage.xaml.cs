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
            var manufacturers = context.Manufacturers
                               .Where(m => m.ManufacturerName == "AMD" || m.ManufacturerName == "Intel")
                               .ToList();
            cbManufacturer.ItemsSource = manufacturers;
            cbSocket.ItemsSource = context.Sockets.ToList();
        }

        // Загрузка списка процессоров
        private void LoadCPUs()
        {
            LVCPUs.ItemsSource = DatabaseEntities.GetContext().CPUs.ToList();
        }

        // Обработчик кнопки добавить
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            selectedCPU = null; // Сбрасываем, так как добавляем новый процессор
            ClearForm(); // Очищаем поля формы
            EditPanel.Visibility = Visibility.Visible; // Показываем панель
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
                EditPanel.Visibility = Visibility.Visible;
            }
            else
            {
                // Если не выбран элемент выводим сообщение об ошибке
                MessageBox.Show("Выберите процессор для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Обработчик кнопки "Сохранить" (создание или обновление)
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new System.Text.StringBuilder();
            try
            {
                if (cbManufacturer.SelectedValue == null)
                {
                    stringBuilder.AppendLine("Выберите производителя.");
                }
                if (cbSocket.SelectedValue == null)
                {
                    stringBuilder.AppendLine("Выберите сокет.");
                }

                int manufacturerID = cbManufacturer.SelectedValue != null ? (int)cbManufacturer.SelectedValue : 0;
                int socketID = cbSocket.SelectedValue != null ? (int)cbSocket.SelectedValue : 0;

                string model = tbModel.Text.Trim();
                if (string.IsNullOrEmpty(model))
                {
                    stringBuilder.AppendLine("Введите модель процессора.");
                }

                int cores = 0;
                if (string.IsNullOrEmpty(tbCores.Text.Trim()))
                {
                    stringBuilder.AppendLine("Укажите количество ядер.");
                }
                else if (!int.TryParse(tbCores.Text.Trim(), out cores) || cores <= 0)
                {
                    stringBuilder.AppendLine("Количество ядер должно быть положительным числом.");
                }

                int threads = 0;
                if (string.IsNullOrEmpty(tbThreads.Text.Trim()))
                {
                    stringBuilder.AppendLine("Укажите количество потоков.");
                }
                else if (!int.TryParse(tbThreads.Text.Trim(), out threads) || threads <= 0)
                {
                    stringBuilder.AppendLine("Количество потоков должно быть положительным числом.");
                }

                decimal baseClock = 0;
                if (string.IsNullOrEmpty(tbBaseClock.Text.Trim()))
                {
                    stringBuilder.AppendLine("Укажите базовую частоту.");
                }
                else if (!decimal.TryParse(tbBaseClock.Text.Trim(), out baseClock) || baseClock <= 0)
                {
                    stringBuilder.AppendLine("Базовая частота должна быть положительным числом.");
                }

                decimal boostClock = 0;
                if (string.IsNullOrEmpty(tbBoostClock.Text.Trim()))
                {
                    stringBuilder.AppendLine("Укажите Boost частоту.");
                }
                else if (!decimal.TryParse(tbBoostClock.Text.Trim(), out boostClock) || boostClock <= 0)
                {
                    stringBuilder.AppendLine("Boost частота должна быть положительным числом.");
                }

                int tdp = 0;
                if (string.IsNullOrEmpty(tbTDP.Text.Trim()))
                {
                    stringBuilder.AppendLine("Укажите TDP.");
                }
                else if (!int.TryParse(tbTDP.Text.Trim(), out tdp) || tdp <= 0)
                {
                    stringBuilder.AppendLine("TDP должен быть положительным числом.");
                }

                decimal price = 0;
                if (string.IsNullOrEmpty(tbPrice.Text.Trim()))
                {
                    stringBuilder.AppendLine("Укажите цену.");
                }
                else if (!decimal.TryParse(tbPrice.Text.Trim(), out price) || price <= 0)
                {
                    stringBuilder.AppendLine("Цена должна быть положительным числом.");
                }

                string imagePath = tbImagePath.Text.Trim();

                if (stringBuilder.Length > 0)
                {
                    MessageBox.Show(stringBuilder.ToString(), "Ошибки валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var context = DatabaseEntities.GetContext();

                if (selectedCPU == null)
                {
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