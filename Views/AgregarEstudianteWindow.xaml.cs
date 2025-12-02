using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows.Controls;
using ZxStudio.Models;

namespace ZxStudio.Views
{
    public partial class AgregarEstudianteWindow : Window
    {
        // Propiedad pública para obtener el nuevo estudiante
        public Estudiante NuevoEstudiante { get; set; }

        // Lista de secciones predeterminadas
        private readonly string[] SeccionesPredeterminadas = {
            "6TO A", "6TO B",
            "5TO A", "5TO B",
            "4TO A", "4TO B",
            "3RO A", "3RO B",
            "2DO A", "2DO B",
            "1RO A", "1RO B"
        };

        public AgregarEstudianteWindow()
        {
            InitializeComponent();
            // Creamos una nueva instancia vacía del estudiante
            NuevoEstudiante = new Estudiante();
            // El DataContext se establece para enlazar los campos
            DataContext = NuevoEstudiante;
        }

        /// <summary>
        /// Evento que valida el campo de Nombre Completo
        /// Solo permite letras, espacios y algunos caracteres especiales (acentos, ñ)
        /// NO permite números
        /// </summary>
        private void TxtNombreCompleto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Esta expresión regular SOLO permite letras (incluyendo acentos y ñ) y espacios
            // [^a-zA-ZáéíóúÁÉÍÓÚñÑ\s] significa "cualquier cosa que NO sea letra o espacio"
            Regex regex = new Regex("[^a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]");

            // Si detecta un número u otro caracter, bloquea la escritura
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Evento que valida el campo de Cédula
        /// Solo permite números
        /// </summary>
        private void TxtCedulaNumeros_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Esta expresión regular verifica si el texto NO es un número
            Regex regex = new Regex("[^0-9]+");
            // Si NO es un número, cancelamos el evento (no deja escribir)
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Construimos la cédula completa con el prefijo V-
            NuevoEstudiante.Cedula = "V-" + (NuevoEstudiante.CedulaNumeros ?? "").Trim();

            // Obtenemos el valor seleccionado del ComboBox
            if (CmbSeccion.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)CmbSeccion.SelectedItem;
                NuevoEstudiante.Seccion = selectedItem.Content.ToString();
            }

            // VALIDACIÓN 1: Verificar que no haya campos vacíos
            if (string.IsNullOrWhiteSpace(NuevoEstudiante.NombreCompleto) ||
                string.IsNullOrWhiteSpace(NuevoEstudiante.CedulaNumeros) ||
                string.IsNullOrWhiteSpace(NuevoEstudiante.Correo) ||
                string.IsNullOrWhiteSpace(NuevoEstudiante.Telefono) ||
                string.IsNullOrWhiteSpace(NuevoEstudiante.Seccion))
            {
                MessageBox.Show("¡Atención! Todos los campos deben estar completos.",
                    "Campos Incompletos",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // VALIDACIÓN 2: Verificar que el nombre no contenga números
            if (ContieneNumeros(NuevoEstudiante.NombreCompleto))
            {
                MessageBox.Show("El nombre completo no puede contener números.",
                    "Nombre Inválido",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // VALIDACIÓN 3: Verificar que el correo sea un Gmail válido
            if (!EsGmailValido(NuevoEstudiante.Correo))
            {
                MessageBox.Show("El correo electrónico debe ser una cuenta de Gmail válida.\nEjemplo: usuario@gmail.com",
                    "Correo Inválido",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // VALIDACIÓN 4: Verificar que la cédula venezolana sea válida
            if (!EsCedulaVenezolaanaValida(NuevoEstudiante.Cedula))
            {
                MessageBox.Show("La cédula debe tener entre 7 y 8 dígitos.\nEjemplo: V-12345678",
                    "Cédula Inválida",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // VALIDACIÓN 5: Verificar que el teléfono venezolano sea válido
            if (!EsTelefonoVenezolaanoValido(NuevoEstudiante.Telefono))
            {
                MessageBox.Show("El teléfono debe ser válido.\nFormato: 0412-1234567 o 04121234567\n(Debe empezar con 0412, 0414, 0416, 0424, 0426 para móviles)",
                    "Teléfono Inválido",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // VALIDACIÓN 6: Verificar que la sección sea una de las predeterminadas
            if (!EsSeccionValida(NuevoEstudiante.Seccion))
            {
                MessageBox.Show($"Debe seleccionar una sección válida: {string.Join(", ", SeccionesPredeterminadas)}",
                    "Sección Inválida",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // Si todas las validaciones pasan, limpiamos y formateamos los datos
            NuevoEstudiante.NombreCompleto = NuevoEstudiante.NombreCompleto.Trim();
            NuevoEstudiante.Cedula = NuevoEstudiante.Cedula.Trim().ToUpper();
            NuevoEstudiante.Correo = NuevoEstudiante.Correo.Trim().ToLower();
            NuevoEstudiante.Seccion = NuevoEstudiante.Seccion.Trim().ToUpper();

            // Cerramos la ventana con éxito
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Verifica si un texto contiene números
        /// </summary>
        private bool ContieneNumeros(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            // Patrón que busca cualquier dígito del 0 al 9
            return Regex.IsMatch(texto, @"\d");
        }

        /// <summary>
        /// Valida que el correo sea un Gmail válido
        /// Ejemplo válido: usuario@gmail.com
        /// </summary>
        private bool EsGmailValido(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            // Patrón regex para validar Gmail
            // Explicación: debe tener caracteres alfanuméricos, puntos o guiones antes del @
            // y debe terminar exactamente en @gmail.com
            string patron = @"^[a-zA-Z0-9][a-zA-Z0-9._-]*@gmail\.com$";

            return Regex.IsMatch(correo.Trim(), patron, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Valida que la cédula venezolana sea válida
        /// Formato válido: V-12345678 (entre 7 y 8 dígitos)
        /// </summary>
        private bool EsCedulaVenezolaanaValida(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
                return false;

            // Limpiamos espacios y convertimos a mayúsculas
            cedula = cedula.Trim().ToUpper();

            // Patrón regex para cédulas venezolanas con V- por defecto
            // Explicación:
            // ^V- = Debe empezar con V-
            // \d{7,8} = Debe tener entre 7 y 8 dígitos
            // $ = Fin de la cadena
            string patron = @"^V-\d{7,8}$";

            return Regex.IsMatch(cedula, patron);
        }

        /// <summary>
        /// Valida que el teléfono venezolano sea válido
        /// Formatos válidos para móviles: 0412-1234567, 04121234567
        /// Formatos válidos para fijos: 0212-1234567, 02121234567
        /// </summary>
        private bool EsTelefonoVenezolaanoValido(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return false;

            // Limpiamos espacios
            telefono = telefono.Trim();

            // Patrón regex para teléfonos venezolanos
            // Explicación:
            // ^0 = Debe empezar con 0
            // (212|412|414|416|424|426|...) = Códigos de área válidos
            // -? = El guion es opcional
            // \d{7} = Debe tener exactamente 7 dígitos después del código
            // $ = Fin de la cadena
            string patron = @"^0(212|241|243|244|245|246|247|248|249|251|252|253|254|255|256|257|258|259|261|262|263|264|265|266|267|268|269|271|272|273|274|275|276|277|278|279|281|282|283|284|285|286|287|288|289|291|292|293|294|295|412|414|416|424|426)-?\d{7}$";

            return Regex.IsMatch(telefono, patron);
        }

        /// <summary>
        /// Valida que la sección sea una de las predeterminadas
        /// </summary>
        private bool EsSeccionValida(string seccion)
        {
            if (string.IsNullOrWhiteSpace(seccion))
                return false;

            // Convertimos a mayúscula y comparamos
            string seccionLimpia = seccion.Trim().ToUpper();

            // Verificamos si la sección está en el array
            return SeccionesPredeterminadas.Any(s => s.ToUpper() == seccionLimpia);
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