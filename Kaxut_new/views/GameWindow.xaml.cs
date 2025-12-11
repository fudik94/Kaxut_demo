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

namespace Kaxut_new
{
    public partial class GameWindow : Window
    {
        private readonly Quiz _quiz;

        private int _currentIndex;
        private int _score;
        private int _correctCount;
        private int _timeRemaining;
        private bool _answered;

        private const int QuestionTimeSeconds = 15;

        private DispatcherTimer? _timer;
        private SoundPlayer? _soundWin;
        private SoundPlayer? _soundFail;

        private Brush? _defaultButtonBackground;
        private Brush? _defaultButtonForeground;

        public GameWindow(Quiz quiz)
        {
            InitializeComponent();
            _quiz = quiz ?? throw new ArgumentNullException(nameof(quiz));

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
            if (_currentIndex < 0 || _currentIndex >= _quiz.Questions.Count)
            {
                EndGame();
                return;
            }

            _answered = false;
            ResetAnswerButtonsStyles();
            SetAnswerButtonsEnabled(true);

            if (_quiz.Questions[_currentIndex] is not MultipleChoiceQuestion q) return;

            lblQuestion.Text = q.Text;

            btnAnswer0.Content = q.Options[0].Text;
            btnAnswer1.Content = q.Options[1].Text;
            btnAnswer2.Content = q.Options[2].Text;
            btnAnswer3.Content = q.Options[3].Text;

            _timeRemaining = QuestionTimeSeconds;
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

            if (_quiz.Questions[_currentIndex] is not MultipleChoiceQuestion mcq) return;

            int correctIndex = mcq.CorrectIndex;
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

            if (_quiz.Questions[_currentIndex] is not MultipleChoiceQuestion mcq) return;

            AnimateAnswer(mcq.CorrectIndex, true);
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
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Button.Background).(SolidColorBrush.Color)"));
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

        private void EndGame()
        {
            _timer?.Stop();

            var results = new ResultsWindow(_correctCount, _score)
            {
                Owner = this.Owner
            };
            results.Show();
            Close();
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
