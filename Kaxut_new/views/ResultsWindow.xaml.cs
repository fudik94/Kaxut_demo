using System.Windows;

namespace Kaxut_new
{
    /// <summary>
    /// Окно результатов: показывает количество правильных ответов и общий счёт.
    /// </summary>
    public partial class ResultsWindow : Window
    {
        private readonly int _correctCount;
        private readonly int _totalScore;

        public ResultsWindow(int correctCount, int totalScore)
        {
            InitializeComponent();
            _correctCount = correctCount;
            _totalScore = totalScore;
            Loaded += ResultsWindow_Loaded;
        }

        // Заполнение визуальных элементов после загрузки
        private void ResultsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtCorrect.Text = _correctCount.ToString();
            txtScore.Text = _totalScore.ToString();
        }

        // Кнопка "New Game"
        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
