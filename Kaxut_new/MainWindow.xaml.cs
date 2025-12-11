using App.Domain.Entities;
using Kaxut_new;
using System;
using System.Windows;

namespace Kaxut_new
{
    public partial class MainWindow : Window
    {
        private readonly Quiz _quiz = new Quiz
        {
            Code = "QZ001",
            Title = "New Quiz"
        };

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _quiz;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (_quiz.Questions.Count == 0)
            {
                MessageBox.Show(this, "Add at least one question before starting.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var gameWindow = new GameWindow(_quiz)
            {
                Owner = this
            };
            gameWindow.Show();
        }

        private void btnAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            var text = txtQuestion.Text?.Trim();
            var a0 = txtAnswer0.Text?.Trim();
            var a1 = txtAnswer1.Text?.Trim();
            var a2 = txtAnswer2.Text?.Trim();
            var a3 = txtAnswer3.Text?.Trim();

            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(a0) ||
                string.IsNullOrWhiteSpace(a1) || string.IsNullOrWhiteSpace(a2) ||
                string.IsNullOrWhiteSpace(a3))
            {
                MessageBox.Show(this, "Fill in the question and all FOUR answers.", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int correctIndex =
                rbCorrect0.IsChecked == true ? 0 :
                rbCorrect1.IsChecked == true ? 1 :
                rbCorrect2.IsChecked == true ? 2 :
                rbCorrect3.IsChecked == true ? 3 : -1;

            if (correctIndex == -1)
            {
                MessageBox.Show(this, "Choose the correct answer.", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Создаём MultipleChoiceQuestion
            var question = new MultipleChoiceQuestion
            {
                Text = text,
                Order = _quiz.Questions.Count + 1,
                CorrectIndex = correctIndex,
                Options = new System.Collections.Generic.List<AnswerOption>
                {
                    new AnswerOption { Text = a0, IsCorrect = correctIndex == 0 },
                    new AnswerOption { Text = a1, IsCorrect = correctIndex == 1 },
                    new AnswerOption { Text = a2, IsCorrect = correctIndex == 2 },
                    new AnswerOption { Text = a3, IsCorrect = correctIndex == 3 }
                }
            };

            _quiz.Questions.Add(question);

            txtStatus.Text = $"Added: {_quiz.Questions.Count}";

            txtQuestion.Clear();
            txtAnswer0.Clear();
            txtAnswer1.Clear();
            txtAnswer2.Clear();
            txtAnswer3.Clear();
            rbCorrect0.IsChecked = rbCorrect1.IsChecked =
                rbCorrect2.IsChecked = rbCorrect3.IsChecked = false;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstQuestions.SelectedItem is Question selected)
            {
                _quiz.Questions.Remove(selected);
                txtStatus.Text = "Question deleted.";
            }
            else
            {
                MessageBox.Show(this, "Select a question to delete.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public Quiz CurrentQuiz => _quiz;
    }
}
