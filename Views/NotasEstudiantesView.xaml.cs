using System.Windows;
using System.Windows.Controls;

namespace ZxStudio.Views
{
    public partial class NotasEstudiantesView : UserControl
    {
        public NotasEstudiantesView()
        {
            InitializeComponent();
            CargarDatosPrueba();
        }

        private void CargarDatosPrueba()
        {
            // Datos de prueba para estudiantes
            var estudiantes = new[]
            {
                new { Id = "001", Nombre = "Juan Pérez García", Grado = "5° Grado", Seccion = "Sección A", Iniciales = "JP" },
                new { Id = "002", Nombre = "María López Ruiz", Grado = "5° Grado", Seccion = "Sección A", Iniciales = "ML" },
                new { Id = "003", Nombre = "Carlos Sánchez Díaz", Grado = "5° Grado", Seccion = "Sección B", Iniciales = "CS" },
                new { Id = "004", Nombre = "Ana Martínez Torres", Grado = "6° Grado", Seccion = "Sección A", Iniciales = "AM" },
                new { Id = "005", Nombre = "Pedro González Vega", Grado = "6° Grado", Seccion = "Sección B", Iniciales = "PG" }
            };

            EstudiantesListBox.ItemsSource = estudiantes;
        }

        private void EstudiantesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EstudiantesListBox.SelectedItem != null)
            {
                dynamic estudiante = EstudiantesListBox.SelectedItem;

                // Actualizar información del estudiante
                EstudianteNombre.Text = estudiante.Nombre;
                EstudianteGrado.Text = estudiante.Grado + " - " + estudiante.Seccion;
                EstudianteId.Text = "ID: " + estudiante.Id;
                EstudianteIniciales.Text = estudiante.Iniciales;
                EstudiantePromedio.Text = "8.5";

                // Cargar notas del estudiante
                var notas = new[]
                {
                    new { Materia = "Matemáticas", Periodo1 = "9.0", Periodo2 = "8.5", Periodo3 = "9.2", Promedio = "8.9" },
                    new { Materia = "Lengua y Literatura", Periodo1 = "8.0", Periodo2 = "8.5", Periodo3 = "8.8", Promedio = "8.4" },
                    new { Materia = "Ciencias Naturales", Periodo1 = "7.5", Periodo2 = "8.0", Periodo3 = "8.5", Promedio = "8.0" },
                    new { Materia = "Ciencias Sociales", Periodo1 = "9.5", Periodo2 = "9.0", Periodo3 = "9.3", Promedio = "9.3" },
                    new { Materia = "Inglés", Periodo1 = "8.5", Periodo2 = "8.8", Periodo3 = "9.0", Promedio = "8.8" },
                    new { Materia = "Educación Física", Periodo1 = "9.0", Periodo2 = "9.0", Periodo3 = "9.5", Promedio = "9.2" }
                };

                NotasDataGrid.ItemsSource = notas;
            }
        }

        private void AgregarNota_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcionalidad de Agregar Nota");
        }
    }
}