using System.Windows;
using System.Windows.Controls;

namespace ZxStudio
{
    // Define la clase principal de la ventana como parcial.
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Lógica de inicialización adicional, si es necesaria.
        }

        /// <summary>
        /// Maneja el evento de cambio de selección en el menú lateral (ListBox) para cambiar la pestaña activa (TabControl).
        /// </summary>
        private void MenuLateral_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Verificamos que el control principal exista y que haya un índice seleccionado válido (>= 0).
            if (ContenidoPrincipal != null && MenuLateral.SelectedIndex >= 0)
            {
                // Establecemos el índice de la pestaña visible (TabControl)
                // al mismo índice del elemento seleccionado en la lista (ListBox).
                ContenidoPrincipal.SelectedIndex = MenuLateral.SelectedIndex;
            }
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}