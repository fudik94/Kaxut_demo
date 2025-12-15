using System.Windows;

namespace Kaxut_new.views
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow { Owner = this };
            main.Show();
            this.Hide();
        }

        private void btnLoadGame_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new LoadGameWindow { Owner = this };
            if (dlg.ShowDialog() == true && dlg.SelectedQuiz != null)
            {
                var game = new GameWindow(dlg.SelectedQuiz, isLoadedQuiz: true) { Owner = this };
                game.Show();
                this.Hide();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void SetLanguageDictionary(string source)
        {
            var dict = new ResourceDictionary { Source = new System.Uri(source, System.UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private void LangEn_Click(object sender, RoutedEventArgs e) => SetLanguageDictionary("Resources/Strings.en.xaml");
        private void LangRu_Click(object sender, RoutedEventArgs e) => SetLanguageDictionary("Resources/Strings.ru.xaml");
        private void LangEt_Click(object sender, RoutedEventArgs e) => SetLanguageDictionary("Resources/Strings.et.xaml");
    }
}

