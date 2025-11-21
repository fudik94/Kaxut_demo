using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Media;
using System.IO;

namespace Kaxut_demo
{
    /// <summary>
    /// Окно игры Kahoot: таймер, выбор ответа, подсветка, набор очков.
    /// </summary>
    public partial class GameWindow : Window
    {
        private readonly Kahoot _kahoot;

        private int _currentIndex;
        private int _score;
        private int _correctCount; // Количество правильных ответов
        private int _timeRemaining;
        private bool _answered;

        private const int QuestionTimeSeconds = 15;

        private DispatcherTimer _timer;

        // Храним дефолтные кисти кнопок, чтобы сбрасывать стили
        private Brush _defaultButtonBackground;
        private Brush _defaultButtonForeground;

        // Плееры звуков
        private SoundPlayer _soundWin;
        private SoundPlayer _soundFail;

        public GameWindow(Kahoot kahoot)
        {
            InitializeComponent();
            _kahoot = kahoot ?? throw new ArgumentNullException(nameof(kahoot));

            // Сохраняем стандартные стили кнопок
            _defaultButtonBackground = btnAnswer0.Background;
            _defaultButtonForeground = btnAnswer0.Foreground;

            InitSounds();
            InitTimer();
            StartGame();
        }

        private void InitSounds()
        {
            // Ожидается папка "sounds" в корне проекта (копируется в bin)
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var soundsDir = Path.Combine(baseDir, "sounds");

            _soundWin = CreatePlayer(Path.Combine(soundsDir, "win.wav"));
            _soundFail = CreatePlayer(Path.Combine(soundsDir, "fail.wav"));
        }

        private SoundPlayer CreatePlayer(string path)
        {
            if (!File.Exists(path))
                return null;

            var player = new SoundPlayer(path);
            try
            {
                player.LoadAsync(); // асинхронная загрузка чтобы не блокировать UI
            }
            catch
            {
                // Игнорируем ошибки загрузки
            }
            return player;
        }

        // Инициализация таймера
        private void InitTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
        }

        // Старт игры
        private void StartGame()
        {
            _score = 0;
            _correctCount = 0;
            _currentIndex = 0;
            LoadCurrentQuestion();
        }

        // Загрузка текущего вопроса
        private void LoadCurrentQuestion()
        {
            if (_currentIndex < 0 || _currentIndex >= _kahoot.Questions.Count)
            {
                EndGame();
                return;
            }

            _answered = false;
            ResetAnswerButtonsStyles();
            SetAnswerButtonsEnabled(true);

            var q = _kahoot.Questions[_currentIndex];

            // Текст вопроса и ответы
            lblQuestion.Text = q.Text;
            btnAnswer0.Content = q.Answers[0];
            btnAnswer1.Content = q.Answers[1];
            btnAnswer2.Content = q.Answers[2];
            btnAnswer3.Content = q.Answers[3];

            // Таймер
            _timeRemaining = QuestionTimeSeconds;
            UpdateStatus();
            _timer.Stop();
            _timer.Start();
        }

        // Тик таймера
        private async void Timer_Tick(object sender, EventArgs e)
        {
            _timeRemaining--;
            UpdateStatus();

            if (_timeRemaining <= 0)
            {
                _timer.Stop();
                await HandleTimeoutAsync();
            }
        }

        // Клик по ответу
        private async void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_answered) return;

            _answered = true;
            _timer.Stop();

            int selectedIndex = GetButtonIndex(sender);
            if (selectedIndex < 0) return;

            int correctIndex = _kahoot.Questions[_currentIndex].CorrectIndex;
            bool isCorrect = _kahoot.CheckAnswer(_currentIndex, selectedIndex);

            // Плавная подсветка
            AnimateAnswer(correctIndex, true);
            if (!isCorrect)
            {
                AnimateAnswer(selectedIndex, false);
            }

            // Звук
            if (isCorrect)
            {
                _soundWin?.Stop();
                _soundWin?.Play();
            }
            else
            {
                _soundFail?.Stop();
                _soundFail?.Play();
            }

            // Очки
            if (isCorrect)
            {
                _correctCount++;
                _score += 10 + Math.Max(0, _timeRemaining);
                UpdateStatus();
            }

            SetAnswerButtonsEnabled(false);

            await Task.Delay(1000);

            _currentIndex++;
            LoadCurrentQuestion();
        }

        // Обработка тайм-аута
        private async Task HandleTimeoutAsync()
        {
            if (_answered) return;
            _answered = true;

            int correctIndex = _kahoot.Questions[_currentIndex].CorrectIndex;

            AnimateAnswer(correctIndex, true);
            SetAnswerButtonsEnabled(false);

            // Звук как неправильный
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

            var toColor = correct ? Colors.LightGreen : Colors.IndianRed;

            var colorAnimation = new ColorAnimation
            {
                To = toColor,
                Duration = TimeSpan.FromSeconds(0.3)
            };

            var storyboard = new Storyboard();
            Storyboard.SetTarget(colorAnimation, button);
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("(Button.Background).(SolidColorBrush.Color)"));
            storyboard.Children.Add(colorAnimation);
            storyboard.Begin();

            button.Foreground = Brushes.White;
        }

        private void EnsureAnimatableBackground(System.Windows.Controls.Button button)
        {
            var scb = button.Background as SolidColorBrush;

            if (scb == null || scb.IsFrozen)
            {
                Color baseColor = Colors.LightGray;
                if (scb != null)
                {
                    baseColor = scb.Color;
                }

                button.Background = new SolidColorBrush(baseColor);
            }
        }

        private void HighlightAnswer(int index, bool correct)
        {
            AnimateAnswer(index, correct);
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
            txtProgress.Text = $"{_currentIndex + 1}/{_kahoot.Questions.Count}";
        }

        private void EndGame()
        {
            _timer.Stop();

            var results = new ResultsWindow(_correctCount, _score)
            {
                Owner = this.Owner
            };
            results.Show();

            Close();
        }

        private System.Windows.Controls.Button GetButtonByIndex(int index)
        {
            switch (index)
            {
                case 0: return btnAnswer0;
                case 1: return btnAnswer1;
                case 2: return btnAnswer2;
                case 3: return btnAnswer3;
                default: return null;
            }
        }

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