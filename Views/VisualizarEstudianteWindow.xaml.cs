using System.Windows;
using System.Windows.Input;
using ZxStudio.Models;

namespace ZxStudio.Views
{
    public partial class VisualizarEstudianteWindow : Window
    {
        public VisualizarEstudianteWindow(Estudiante estudianteAVer)
        {
            InitializeComponent();

            // Asignamos el estudiante al DataContext
            DataContext = estudianteAVer;
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            // Simplemente cierra la ventana
            this.Close();
        }

        // Opcional: Permite mover la ventana sin bordes
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}