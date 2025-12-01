using System.Windows;
using System.Windows.Input;
using ZxStudio.Models; // Asegúrate de que apunte a donde está tu clase Estudiante

namespace ZxStudio.Views
{
    public partial class EditarEstudianteWindow : Window
    {
        public EditarEstudianteWindow(Estudiante estudianteAEditar)
        {
            InitializeComponent();

            // Vinculamos el estudiante recibido a la vista
            DataContext = estudianteAEditar;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Aquí confirmamos la edición
            this.DialogResult = true;
            this.Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            // Cancelamos y cerramos
            this.DialogResult = false;
            this.Close();
        }

        // Opcional: Para poder mover la ventana ya que tiene WindowStyle="None"
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}