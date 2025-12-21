using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using App.Domain.Entities;

namespace Kaxut_new.views
{
    public partial class LoadGameWindow : Window
    {
        public Quiz? SelectedQuiz { get; private set; }

        public LoadGameWindow()
        {
            InitializeComponent();
            Loaded += LoadGameWindow_Loaded;
        }

        private async void LoadGameWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadQuizzesAsync();
        }

        private async Task LoadQuizzesAsync()
        {
            if (App.Services is null)
            {
                var message = (string)Application.Current.Resources["ProviderErrorInit"];
                MessageBox.Show(this, message);
                return;
            }

            using var scope = App.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var data = await db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.AnswerOptions)
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            lstQuizzes.ItemsSource = data;
        }

        private async void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (lstQuizzes.SelectedItem is not Quiz selected)
            {
                var message = (string)Application.Current.Resources["SelectQuiz"];
                MessageBox.Show(this, message);
                return;
            }

            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var quiz = await db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.Id == selected.Id);

            if (quiz == null)
            {
                var message = (string)Application.Current.Resources["QuizNotFound"];
                MessageBox.Show(this, message);
                return;
            }

            SelectedQuiz = quiz;
            DialogResult = true;
            Close();
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstQuizzes.SelectedItem is not Quiz quiz) return;

            var confirm = MessageBox.Show(this, $"Delete '{quiz.Title}'?", "Confirm",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes) return;

            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Quizzes.Remove(quiz);
            await db.SaveChangesAsync();

            await LoadQuizzesAsync();
        }
    }
}
