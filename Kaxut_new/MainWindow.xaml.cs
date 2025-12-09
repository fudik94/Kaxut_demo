using Kaxut_new.Models;
using System;
using System.Windows;

namespace Kaxut_new
{
    /// <summary>
    /// Главное окно-конструктор: создаёт список вопросов и запускает игру.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Хранилище викторины (список вопросов)
        private readonly Kahoot _kahoot = new Kahoot();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _kahoot;
        }

        // Запуск игры: открываем GameWindow и передаём текущую викторину
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (_kahoot.Questions.Count == 0)
            {
                MessageBox.Show(this, "Add at least one question before starting.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var gameWindow = new GameWindow(_kahoot)
            {
                // Владелец — главное окно (для корректной активации и центрирования)
                Owner = this
            };
            gameWindow.Show();
        }

        private void btnAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            // Сначала проверяем, заполнены ли вопрос и 4 ответа
            var text = txtQuestion.Text?.Trim();
            var a0 = txtAnswer0.Text?.Trim();
            var a1 = txtAnswer1.Text?.Trim();
            var a2 = txtAnswer2.Text?.Trim();
            var a3 = txtAnswer3.Text?.Trim();

            if (string.IsNullOrWhiteSpace(text) ||
                string.IsNullOrWhiteSpace(a0) ||
                string.IsNullOrWhiteSpace(a1) ||
                string.IsNullOrWhiteSpace(a2) ||
                string.IsNullOrWhiteSpace(a3))
            {
                MessageBox.Show(this,
                    "Fill in the question and all FOUR answers.",
                    "Input error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // Только после того как все текстовые поля заполнены — проверяем, выбран ли правильный ответ
            int correctIndex =
                rbCorrect0.IsChecked == true ? 0 :
                rbCorrect1.IsChecked == true ? 1 :
                rbCorrect2.IsChecked == true ? 2 :
                rbCorrect3.IsChecked == true ? 3 : -1;

            if (correctIndex == -1)
            {
                MessageBox.Show(this,
                    "Choose the correct answer.",
                    "Input error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                _kahoot.AddQuestion(text, a0, a1, a2, a3, correctIndex);
                txtStatus.Text = $"Added: {_kahoot.Questions.Count}";

                // Очистка полей
                txtQuestion.Clear();
                txtAnswer0.Clear();
                txtAnswer1.Clear();
                txtAnswer2.Clear();
                txtAnswer3.Clear();
                rbCorrect0.IsChecked = rbCorrect1.IsChecked =
                    rbCorrect2.IsChecked = rbCorrect3.IsChecked = false;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this,
                    "Error: " + ex.Message,
                    "Exception",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstQuestions.SelectedItem is Question selected)
            {
                _kahoot.Questions.Remove(selected);
                txtStatus.Text = "Question deleted.";
            }
            else
            {
                MessageBox.Show(this, "Select a question to delete.", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Необязательно: публичное свойство для доступа к текущей викторине
        public Kahoot CurrentKahoot => _kahoot;
    }
}
