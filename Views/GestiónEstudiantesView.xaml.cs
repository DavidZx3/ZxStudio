using System;
using System.Collections.Generic; // NUEVO: para List<string>
using System.Collections.ObjectModel;
using System.IO; // ¡NUEVO: para trabajar con archivos!
using System.Linq; // ¡NUEVO: para trabajar con Skip()!
using System.Windows;
using System.Windows.Controls;
using ZxStudio.Models;

namespace ZxStudio.Views
{
    public partial class GestionEstudiantesView : UserControl
    {
        // 📌 RUTA donde se guardará el archivo CSV
        private const string RUTA_ARCHIVO = "estudiantes.csv";

        public ObservableCollection<Estudiante> ListaEstudiantes { get; set; }

        public GestionEstudiantesView()
        {
            InitializeComponent();

            // 1. Intentar cargar los datos guardados al inicio
            ListaEstudiantes = CargarEstudiantesDesdeCSV();

            // 2. Si no hay datos guardados, usar los datos de ejemplo (SOLO PARA INICIO)
            if (ListaEstudiantes.Count == 0)
            {
                ListaEstudiantes.Add(new Estudiante { Iniciales = "JD", NombreCompleto = "Juan Diaz", Matricula = "2023-001", Correo = "juan@mail.com", Telefono = "809-555-0101", Seccion = "A1" });
                ListaEstudiantes.Add(new Estudiante { Iniciales = "MP", NombreCompleto = "Maria Perez", Matricula = "2023-002", Correo = "maria@mail.com", Telefono = "809-555-0202", Seccion = "B2" });
            }

            this.DataContext = this;
        }

        // --- FUNCIONES DE PERSISTENCIA (GUARDAR Y CARGAR) ---

        private ObservableCollection<Estudiante> CargarEstudiantesDesdeCSV()
        {
            var estudiantesCargados = new ObservableCollection<Estudiante>();

            if (!File.Exists(RUTA_ARCHIVO))
            {
                return estudiantesCargados;
            }

            try
            {
                // Leer todas las líneas del archivo, saltando la primera (encabezado)
                var lineas = File.ReadAllLines(RUTA_ARCHIVO).Skip(1);

                foreach (var linea in lineas)
                {
                    // Separar los valores por coma
                    var valores = linea.Split(',');

                    if (valores.Length >= 6) // Debe tener al menos 6 campos
                    {
                        // Aseguramos el orden de los campos al cargar
                        estudiantesCargados.Add(new Estudiante
                        {
                            Iniciales = valores[0],
                            NombreCompleto = valores[1],
                            Matricula = valores[2],
                            Correo = valores[3],
                            Telefono = valores[4],
                            Seccion = valores[5]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error de Carga", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return estudiantesCargados;
        }

        private void GuardarEstudiantesACSV()
        {
            try
            {
                var lineasCSV = new List<string>();

                // 1. Agregar el encabezado
                lineasCSV.Add("Iniciales,NombreCompleto,Matricula,Correo,Telefono,Seccion");

                // 2. Convertir cada estudiante a una línea de CSV
                foreach (var estudiante in ListaEstudiantes)
                {
                    string linea = $"{estudiante.Iniciales},{estudiante.NombreCompleto},{estudiante.Matricula},{estudiante.Correo},{estudiante.Telefono},{estudiante.Seccion}";
                    lineasCSV.Add(linea);
                }

                // 3. Escribir todas las líneas en el archivo (sobrescribe)
                File.WriteAllLines(RUTA_ARCHIVO, lineasCSV);
                MessageBox.Show("Datos guardados localmente en estudiantes.csv.", "Guardado Exitoso");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar los datos: {ex.Message}", "Error de Guardado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ---------- NUEVO: LÓGICA DEL BOTÓN GUARDAR DATOS ----------
        private void GuardarDatos_Click(object sender, RoutedEventArgs e)
        {
            GuardarEstudiantesACSV();
        }

        // ---------- LÓGICA DEL BOTÓN AGREGAR ----------
        // ---------- LÓGICA DEL BOTÓN AGREGAR ----------
        private void AgregarEstudiante_Click(object sender, RoutedEventArgs e)
        {
            // 1. Crear una instancia de la ventana de agregar estudiante
            var ventanaAgregar = new AgregarEstudianteWindow();

            // Opcional: Oscurecer la ventana principal mientras el diálogo está abierto
            this.Opacity = 0.5;

            // 2. Mostrar la ventana como un diálogo modal. Retorna true si se hizo clic en Guardar.
            bool? resultado = ventanaAgregar.ShowDialog();

            // Opcional: Devolver la opacidad a la normalidad
            this.Opacity = 1;

            // 3. Verificar si el usuario guardó (resultado es true)
            if (resultado == true)
            {
                // 4. Obtener el nuevo estudiante de la propiedad NuevoEstudiante de la ventana
                Estudiante nuevoEstudiante = ventanaAgregar.NuevoEstudiante;

                // 5. Generar las iniciales (para que se muestren en la columna de avatar)
                // ESTA FUNCIÓN DEBE EXISTIR EN LA CLASE ESTUDIANTE.
                nuevoEstudiante.GenerarIniciales();

                // 6. Agregar el nuevo estudiante a la colección ObservableCollection
                ListaEstudiantes.Add(nuevoEstudiante);

                MessageBox.Show("Estudiante agregado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                // 7. Guardar los cambios en el archivo CSV inmediatamente
                GuardarEstudiantesACSV();
            }
        }
        // ---------- LÓGICA DEL BOTÓN EDITAR ----------
        private void EditarEstudiante_Click(object sender, RoutedEventArgs e)
        {
            // OBTENER ESTUDIANTE DIRECTAMENTE DE LA SELECCIÓN DEL DATAGRID
            if (EstudiantesDataGrid.SelectedItem is Estudiante estudianteSeleccionado)
            {
                var ventanaEdicion = new EditarEstudianteWindow(estudianteSeleccionado);
                this.Opacity = 0.5;
                bool? resultado = ventanaEdicion.ShowDialog();
                this.Opacity = 1;

                if (resultado == true)
                {
                    MessageBox.Show("Estudiante actualizado correctamente.");
                    // 💡 Nota: Aquí podrías llamar a GuardarEstudiantesACSV() si quieres guardar al instante
                }
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ningún alumno para editar.", "Error de Selección");
            }
        }

        // ---------- LÓGICA DEL BOTÓN VISUALIZAR ----------
        private void VisualizarEstudiante_Click(object sender, RoutedEventArgs e)
        {
            // OBTENER ESTUDIANTE DIRECTAMENTE DE LA SELECCIÓN DEL DATAGRID
            if (EstudiantesDataGrid.SelectedItem is Estudiante estudianteSeleccionado)
            {
                var ventanaVisualizar = new VisualizarEstudianteWindow(estudianteSeleccionado);
                this.Opacity = 0.5;
                ventanaVisualizar.ShowDialog();
                this.Opacity = 1;
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ningún alumno para visualizar.", "Error de Selección");
            }
        }

        // ---------- LÓGICA DEL BOTÓN ELIMINAR ----------
        private void EliminarEstudiante_Click(object sender, RoutedEventArgs e)
        {
            // OBTENER ESTUDIANTE DIRECTAMENTE DE LA SELECCIÓN DEL DATAGRID
            if (EstudiantesDataGrid.SelectedItem is Estudiante estudianteSeleccionado)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"¿Estás seguro de que deseas eliminar a {estudianteSeleccionado.NombreCompleto}?",
                    "Confirmar Eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    ListaEstudiantes.Remove(estudianteSeleccionado);
                    MessageBox.Show("Estudiante eliminado correctamente.");
                    // 💡 Nota: Aquí podrías llamar a GuardarEstudiantesACSV() si quieres guardar al instante
                }
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ningún alumno para eliminar.", "Error de Selección");
            }
        }
    }
}