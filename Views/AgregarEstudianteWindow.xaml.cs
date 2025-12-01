using System.Windows;
using System.Windows.Input;
using ZxStudio.Models;

namespace ZxStudio.Views
{
    public partial class AgregarEstudianteWindow : Window
    {
        // 1. AÑADE ESTA PROPIEDAD PÚBLICA (Para que GestionEstudiantesView pueda obtener el nuevo objeto)
        public Estudiante NuevoEstudiante { get; set; }

        // 2. CORRIGE EL CONSTRUCTOR (Debe inicializar la propiedad y no debe recibir parámetros)
        public AgregarEstudianteWindow()
        {
            InitializeComponent();

            // Creamos una NUEVA instancia vacía del estudiante.
            NuevoEstudiante = new Estudiante();

            // El DataContext se establece a esta NUEVA instancia para que los campos de texto
            // se enlacen a sus propiedades.
            DataContext = NuevoEstudiante;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            // El objeto NuevoEstudiante ya tiene los datos si el DataContext está bien configurado.

            // VALIDACIÓN DE CAMPOS OBLIGATORIOS
            if (string.IsNullOrWhiteSpace(NuevoEstudiante.NombreCompleto) ||
                string.IsNullOrWhiteSpace(NuevoEstudiante.Matricula) ||
                string.IsNullOrWhiteSpace(NuevoEstudiante.Correo) ||
                string.IsNullOrWhiteSpace(NuevoEstudiante.Telefono) ||
                string.IsNullOrWhiteSpace(NuevoEstudiante.Seccion))
            {
                // Si falta algún campo, mostramos un mensaje de error y salimos de la función.
                MessageBox.Show("¡Atención! Todos los campos deben estar rellenos para poder guardar el estudiante.", "Campos Incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Si la validación es exitosa, se cierra la ventana y se confirma la acción.
            this.DialogResult = true;
            this.Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            // Cancela la acción
            this.DialogResult = false;
            this.Close();
        }

        // Para poder mover la ventana sin el borde
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}