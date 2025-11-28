using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ZxStudio.Models;

namespace ZxStudio.Views
{
    public partial class GestionEstudiantesView : UserControl
    {
        public ObservableCollection<Estudiante> ListaEstudiantes { get; set; }

        public int TotalEstudiantes
        {
            get { return ListaEstudiantes != null ? ListaEstudiantes.Count : 0; }
        }

        public GestionEstudiantesView()
        {
            InitializeComponent();
            CargarEstudiantes();
            DataContext = this;
        }

        // ============================================================
        // CARGAR LISTA
        // ============================================================
        private void CargarEstudiantes()
        {
            ListaEstudiantes = new ObservableCollection<Estudiante>
            {
                new Estudiante
                {
                    Iniciales = "JP",
                    NombreCompleto = "Juan Pérez",
                    Matricula = "2024-001",
                    Correo = "juan@example.com",
                    Telefono = "555-1234",
                    Seccion = "A"
                },
                new Estudiante
                {
                    Iniciales = "MS",
                    NombreCompleto = "María Santos",
                    Matricula = "2024-002",
                    Correo = "maria@example.com",
                    Telefono = "555-5678",
                    Seccion = "B"
                }
            };
        }

        // ============================================================
        // OBTENER ESTUDIANTE SELECCIONADO
        // ============================================================
        private Estudiante ObtenerSeleccion()
        {
            if (EstudiantesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un estudiante.", "Aviso");
                return null;
            }

            return EstudiantesDataGrid.SelectedItem as Estudiante;
        }

        // ============================================================
        // VER ESTUDIANTE
        // ============================================================
        private void VisualizarEstudiante_Click(object sender, RoutedEventArgs e)
        {
            var estudiante = ObtenerSeleccion();
            if (estudiante == null) return;

            MessageBox.Show(
                $"Nombre: {estudiante.NombreCompleto}\n" +
                $"Correo: {estudiante.Correo}\n" +
                $"Teléfono: {estudiante.Telefono}",
                "Detalle del Estudiante");
        }

        // ============================================================
        // EDITAR ESTUDIANTE
        // ============================================================
        private void EditarEstudiante_Click(object sender, RoutedEventArgs e)
        {
            var estudiante = ObtenerSeleccion();
            if (estudiante == null) return;

            MessageBox.Show($"(Aquí abrirías la ventana de edición)\nEditando: {estudiante.NombreCompleto}");
        }

        // ============================================================
        // ELIMINAR ESTUDIANTE
        // ============================================================
        private void EliminarEstudiante_Click(object sender, RoutedEventArgs e)
        {
            var estudiante = ObtenerSeleccion();
            if (estudiante == null) return;

            var confirm = MessageBox.Show(
                $"¿Eliminar estudiante: {estudiante.NombreCompleto}?",
                "Confirmar",
                MessageBoxButton.YesNo);

            if (confirm == MessageBoxResult.Yes)
            {
                ListaEstudiantes.Remove(estudiante);
                MessageBox.Show("Estudiante eliminado.");

                // Refrescar conteo
                OnPropertyChanged("TotalEstudiantes");
            }
        }

        // ============================================================
        // AGREGAR ESTUDIANTE
        // ============================================================
        private void AgregarEstudiante_Click(object sender, RoutedEventArgs e)
        {
            var nuevo = new Estudiante
            {
                Iniciales = "NN",
                NombreCompleto = "Nuevo Estudiante",
                Matricula = "0000",
                Correo = "nuevo@example.com",
                Telefono = "000-000",
                Seccion = "X"
            };

            ListaEstudiantes.Add(nuevo);

            MessageBox.Show("Estudiante agregado.");
            OnPropertyChanged("TotalEstudiantes");
        }

        // ============================================================
        // NOTIFICAR CAMBIO (simple para C# 7.3 sin MVVM)
        // ============================================================
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string nombre)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nombre));
        }
    }
}
