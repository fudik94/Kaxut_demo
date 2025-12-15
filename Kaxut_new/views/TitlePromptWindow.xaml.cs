using System.Windows;

namespace Kaxut_new.views
{
    public partial class TitlePromptWindow : Window
    {
        public string? ResultTitle { get; private set; }

        public TitlePromptWindow(string currentTitle)
        {
            InitializeComponent();
            Loaded += (s, e) => txtTitle.Text = currentTitle;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ResultTitle = txtTitle.Text?.Trim();
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
