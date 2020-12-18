using System.Windows.Input;

namespace MiniBlinkDemo
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static RoutedCommand OpenUrlCommand { get; } = new();

        public void ExecutedOpenUrlCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is not string url)
                return;

            Blink.Url = url;
        }

        public void CanExecutedOpenUrlCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
