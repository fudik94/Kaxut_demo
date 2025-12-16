using App.Domain.Entities;
using Kaxut_new;
using System;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Kaxut_new.views;

namespace Kaxut_new
{
    public partial class GameWindow : Window
    {
        private readonly Quiz _quiz;
        private readonly bool _isLoadedQuiz;

        private int _currentIndex;
        private int _score;
        private int _correctCount;
        private int _timeRemaining;
        private bool _answered;

        private DispatcherTimer? _timer;
        private SoundPlayer? _soundWin;
        private SoundPlayer? _soundFail;

        private Brush? _defaultButtonBackground;
        private Brush? _defaultButtonForeground;

        public GameWindow(Quiz quiz, bool isLoadedQuiz)
        {
            InitializeComponent();
            _quiz = quiz ?? throw new ArgumentNullException(nameof(quiz));
            _isLoadedQuiz = isLoadedQuiz;

            _defaultButtonBackground = btnAnswer0.Background;
            _defaultButtonForeground = btnAnswer0.Foreground;

            InitSounds();
            InitTimer();
            StartGame();
        }

        private void InitSounds()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var soundsDir = Path.Combine(baseDir, "sounds");

            _soundWin = CreatePlayer(Path.Combine(soundsDir, "win.wav"));
            _soundFail = CreatePlayer(Path.Combine(soundsDir, "fail.wav"));
        }

        private SoundPlayer? CreatePlayer(string path)
        {
            if (!File.Exists(path)) return null;
            var player = new SoundPlayer(path);
            try { player.LoadAsync(); } catch { }
            return player;
        }

        private void InitTimer()
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += Timer_Tick;
        }

        private void StartGame()
        {
            _score = 0;
            _correctCount = 0;
            _currentIndex = 0;
            LoadCurrentQuestion();
        }

        private void LoadCurrentQuestion()
        {
            if (_quiz.Questions == null || _quiz.Questions.Count == 0)
            {
                lblQuestion.Text = "Add questions to start the quiz!";
                SetAnswerButtonsEnabled(false);
                return;
            }

            if (_currentIndex < 0 || _currentIndex >= _quiz.Questions.Count)
            {
                _ = EndGameAsync(); 
                return;
            }

            _answered = false;
            ResetAnswerButtonsStyles();
            SetAnswerButtonsEnabled(true);

            var q = _quiz.Questions[_currentIndex];
            lblQuestion.Text = q.Text;

            var opts = q.AnswerOptions;
            if (opts == null || opts.Count < 4)
            {
                MessageBox.Show(this, "Question has no options.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            btnAnswer0.Content = opts[0].Text;
            btnAnswer1.Content = opts[1].Text;
            btnAnswer2.Content = opts[2].Text;
            btnAnswer3.Content = opts[3].Text;

            _timeRemaining = q.TimeLimitSeconds > 0 ? q.TimeLimitSeconds : 15;
            UpdateStatus();

            _timer?.Stop();
            _timer?.Start();
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            _timeRemaining--;
            UpdateStatus();

            if (_timeRemaining <= 0)
            {
                _timer?.Stop();
                await HandleTimeoutAsync();
            }
        }

        private async void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_answered) return;
            _answered = true;
            _timer?.Stop();

            int selectedIndex = GetButtonIndex(sender);
            if (selectedIndex < 0) return;

            var q = _quiz.Questions[_currentIndex];
            int correctIndex = q.AnswerOptions.FindIndex(o => o.IsCorrect);
            if (correctIndex < 0) return;

            bool isCorrect = selectedIndex == correctIndex;

            AnimateAnswer(correctIndex, true);
            if (!isCorrect) AnimateAnswer(selectedIndex, false);

            if (isCorrect)
            {
                _soundWin?.Stop();
                _soundWin?.Play();
                _correctCount++;
                _score += 10 + Math.Max(0, _timeRemaining);
            }
            else
            {
                _soundFail?.Stop();
                _soundFail?.Play();
            }

            UpdateStatus();
            SetAnswerButtonsEnabled(false);

            await Task.Delay(1000);

            _currentIndex++;
            LoadCurrentQuestion();
        }

        private async Task HandleTimeoutAsync()
        {
            if (_answered) return;
            _answered = true;

            var q = _quiz.Questions[_currentIndex];
            int correctIndex = q.AnswerOptions.FindIndex(o => o.IsCorrect);

            if (correctIndex >= 0)
                AnimateAnswer(correctIndex, true);

            SetAnswerButtonsEnabled(false);

            _soundFail?.Stop();
            _soundFail?.Play();

            await Task.Delay(1000);

            _currentIndex++;
            LoadCurrentQuestion();
        }

        private void AnimateAnswer(int index, bool correct)
        {
            var button = GetButtonByIndex(index);
            if (button == null) return;

            EnsureAnimatableBackground(button);

            var color = correct ? Colors.LightGreen : Colors.IndianRed;

            var animation = new ColorAnimation
            {
                To = color,
                Duration = TimeSpan.FromSeconds(0.3)
            };

            var storyboard = new Storyboard();
            Storyboard.SetTarget(animation, button);
            Storyboard.SetTargetProperty(animation,
                new PropertyPath("(Button.Background).(SolidColorBrush.Color)"));
            storyboard.Children.Add(animation);
            storyboard.Begin();

            button.Foreground = Brushes.White;
        }

        private void EnsureAnimatableBackground(System.Windows.Controls.Button button)
        {
            var scb = button.Background as SolidColorBrush;
            if (scb == null || scb.IsFrozen)
            {
                var baseColor = scb?.Color ?? Colors.LightGray;
                button.Background = new SolidColorBrush(baseColor);
            }
        }

        private void ResetAnswerButtonsStyles()
        {
            btnAnswer0.Background = _defaultButtonBackground;
            btnAnswer1.Background = _defaultButtonBackground;
            btnAnswer2.Background = _defaultButtonBackground;
            btnAnswer3.Background = _defaultButtonBackground;

            btnAnswer0.Foreground = _defaultButtonForeground;
            btnAnswer1.Foreground = _defaultButtonForeground;
            btnAnswer2.Foreground = _defaultButtonForeground;
            btnAnswer3.Foreground = _defaultButtonForeground;
        }

        private void SetAnswerButtonsEnabled(bool enabled)
        {
            btnAnswer0.IsEnabled = enabled;
            btnAnswer1.IsEnabled = enabled;
            btnAnswer2.IsEnabled = enabled;
            btnAnswer3.IsEnabled = enabled;
        }

        private void UpdateStatus()
        {
            txtTimer.Text = $"{_timeRemaining}s";
            txtScore.Text = $"Score: {_score}";
            txtProgress.Text = $"{_currentIndex + 1}/{_quiz.Questions.Count}";
        }

        private async Task EndGameAsync()
        {
            _timer?.Stop();

            var results = new ResultsWindow(_correctCount, _score)
            {
                Owner = this
            };
            results.ShowDialog(); 

            if (!_isLoadedQuiz)
            {
                var save = MessageBox.Show(this,
                    "Do you want to save this quiz to the database?",
                    "Save Quiz", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (save == MessageBoxResult.Yes)
                {
                    var titleWindow = new TitlePromptWindow(_quiz.Title) { Owner = this };
                    bool? titleResult = titleWindow.ShowDialog();
                    if (titleResult == true && !string.IsNullOrWhiteSpace(titleWindow.ResultTitle))
                    {
                        _quiz.Title = titleWindow.ResultTitle.Trim();
                        await SaveQuizAsync(_quiz);
                        MessageBox.Show(this, "Quiz saved successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }

            CloseAllExceptStartWindow();

            ShowStartScreen();
        }

        private void CloseAllExceptStartWindow()
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w != this && !(w is StartWindow))
                {
                    w.Close();
                }
            }

            this.Close();
        }

        private void ShowStartScreen()
        {
            var startWindow = Application.Current.Windows.OfType<StartWindow>().FirstOrDefault();
            if (startWindow != null)
            {
                startWindow.Show();
                startWindow.Activate();
            }
            else
            {
                var newStart = new StartWindow();
                newStart.Show();
            }
        }


        private static async Task SaveQuizAsync(Quiz quiz)
        {
            if (App.Services is null) throw new InvalidOperationException("Service provider is not initialized");

            using var scope = App.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var existing = await db.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.Code == quiz.Code);

            if (existing != null)
            {
                db.Quizzes.Remove(existing);
                await db.SaveChangesAsync();
            }

            var newQuiz = new Quiz
            {
                Id = Guid.NewGuid(),
                Code = quiz.Code,
                Title = quiz.Title,
                CreatedAt = DateTime.UtcNow,
            };

            foreach (var q in quiz.Questions)
            {
                var nq = new MultipleChoiceQuestion
                {
                    Id = Guid.NewGuid(),
                    Text = q.Text,
                    Order = q.Order,
                    CorrectIndex = q.AnswerOptions.FindIndex(o => o.IsCorrect),
                    TimeLimitSeconds = q.TimeLimitSeconds,
                    Quiz = newQuiz,
                    QuizId = newQuiz.Id
                };

                foreach (var opt in q.AnswerOptions)
                {
                    nq.AnswerOptions.Add(new AnswerOption
                    {
                        Id = Guid.NewGuid(),
                        Text = opt.Text,
                        IsCorrect = opt.IsCorrect,
                        Question = nq,
                        QuestionId = nq.Id
                    });
                }

                newQuiz.Questions.Add(nq);
            }

            db.Quizzes.Add(newQuiz);
            await db.SaveChangesAsync();
        }

        private System.Windows.Controls.Button? GetButtonByIndex(int index) =>
            index switch
            {
                0 => btnAnswer0,
                1 => btnAnswer1,
                2 => btnAnswer2,
                3 => btnAnswer3,
                _ => null
            };

        private int GetButtonIndex(object sender)
        {
            if (ReferenceEquals(sender, btnAnswer0)) return 0;
            if (ReferenceEquals(sender, btnAnswer1)) return 1;
            if (ReferenceEquals(sender, btnAnswer2)) return 2;
            if (ReferenceEquals(sender, btnAnswer3)) return 3;
            return -1;
        }
    }
}
