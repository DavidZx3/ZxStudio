using System.Windows;
using System.Windows.Input;
using ZxStudio.Models;

namespace ZxStudio.Views
{
    public partial class VisualizarEstudianteWindow : Window
    {
        private Estudiante _estudiante;

        public VisualizarEstudianteWindow(Estudiante estudiante)
        {
            InitializeComponent();
            _estudiante = estudiante;
            DataContext = _estudiante;
        }

        /// <summary>
        /// Copia el nombre completo al portapapeles
        /// </summary>
        private void CopiarNombre_Click(object sender, RoutedEventArgs e)
        {
            CopiarAlPortapapeles(_estudiante.NombreCompleto, "Nombre");
        }

        /// <summary>
        /// Copia la cédula al portapapeles
        /// </summary>
        private void CopiarCedula_Click(object sender, RoutedEventArgs e)
        {
            CopiarAlPortapapeles(_estudiante.Cedula, "Cédula");
        }

        /// <summary>
        /// Copia el correo al portapapeles
        /// </summary>
        private void CopiarCorreo_Click(object sender, RoutedEventArgs e)
        {
            CopiarAlPortapapeles(_estudiante.Correo, "Correo");
        }

        /// <summary>
        /// Copia el teléfono al portapapeles
        /// </summary>
        private void CopiarTelefono_Click(object sender, RoutedEventArgs e)
        {
            CopiarAlPortapapeles(_estudiante.Telefono, "Teléfono");
        }

        /// <summary>
        /// Copia la sección al portapapeles
        /// </summary>
        private void CopiarSeccion_Click(object sender, RoutedEventArgs e)
        {
            CopiarAlPortapapeles(_estudiante.Seccion, "Sección");
        }

        /// <summary>
        /// Método auxiliar que copia texto al portapapeles y muestra un mensaje
        /// </summary>
        /// <param name="texto">El texto a copiar</param>
        /// <param name="nombreCampo">El nombre del campo para el mensaje</param>
        private void CopiarAlPortapapeles(string texto, string nombreCampo)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(texto))
                {
                    // Copia el texto al portapapeles
                    Clipboard.SetText(texto);

                    // Muestra un mensaje de confirmación
                    MessageBox.Show($"{nombreCampo} copiado al portapapeles.",
                        "Copiado",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"No hay {nombreCampo.ToLower()} para copiar.",
                        "Campo vacío",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error al copiar: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Cierra la ventana
        /// </summary>
        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Permite mover la ventana arrastrándola
        /// </summary>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}