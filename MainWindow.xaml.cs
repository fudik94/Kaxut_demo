using System;
using System.Windows;

namespace Kaxut_demo
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
            // Определяем индекс правильного ответа
            int correctIndex =
                rbCorrect0.IsChecked == true ? 0 :
                rbCorrect1.IsChecked == true ? 1 :
                rbCorrect2.IsChecked == true ? 2 :
                rbCorrect3.IsChecked == true ? 3 : -1;

            if (correctIndex == -1)
            {
                txtStatus.Text = "Choose the correct answer.";
                return;
            }

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
                txtStatus.Text = "Fill in the question and all answers.";
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
                rbCorrect0.IsChecked = rbCorrect1.IsChecked = rbCorrect2.IsChecked = rbCorrect3.IsChecked = false;
            }
            catch (System.Exception ex)
            {
                txtStatus.Text = "Error: " + ex.Message;
            }
        }

        // Необязательно: публичное свойство для доступа к текущей викторине
        public Kahoot CurrentKahoot => _kahoot;
    }
}
