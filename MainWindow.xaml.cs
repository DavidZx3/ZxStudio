using System.Windows;
using System.Windows.Controls;

namespace ZxStudio
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Método común para todos los botones del menú
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag.ToString(), out int index))
            {
                ContenidoPrincipal.SelectedIndex = index;
            }
        }
    }
}
