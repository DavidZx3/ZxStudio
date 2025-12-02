using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows.Controls;
using ZxStudio.Models;

namespace ZxStudio.Views
{
    public partial class EditarEstudianteWindow : Window
    {
        // Referencia al estudiante que estamos editando
        private Estudiante _estudianteOriginal;

        // Lista de secciones predeterminadas
        private readonly string[] SeccionesPredeterminadas = {
            "6TO A", "6TO B",
            "5TO A", "5TO B",
            "4TO A", "4TO B",
            "3RO A", "3RO B",
            "2DO A", "2DO B",
            "1RO A", "1RO B"
        };

        public EditarEstudianteWindow(Estudiante estudianteAEditar)
        {
            InitializeComponent();

            // Guardamos la referencia al estudiante original
            _estudianteOriginal = estudianteAEditar;

            // Vinculamos el estudiante recibido a la vista
            DataContext = _estudianteOriginal;

            // Extraemos solo los números de la cédula para mostrar en el campo
            if (!string.IsNullOrEmpty(_estudianteOriginal.Cedula))
            {
                // Si la cédula tiene formato "V-12345678", extraemos solo "12345678"
                _estudianteOriginal.CedulaNumeros = _estudianteOriginal.Cedula.Replace("V-", "").Replace("E-", "").Trim();
            }

            // Seleccionamos la sección actual en el ComboBox
            SeleccionarSeccionActual();
        }

        /// <summary>
        /// Selecciona la sección actual del estudiante en el ComboBox
        /// </summary>
        private void SeleccionarSeccionActual()
        {
            if (string.IsNullOrEmpty(_estudianteOriginal.Seccion))
                return;

            // Buscamos el ComboBoxItem que coincide con la sección del estudiante
            foreach (ComboBoxItem item in CmbSeccion.Items)
            {
                if (item.Content.ToString().ToUpper() == _estudianteOriginal.Seccion.ToUpper())
                {
                    CmbSeccion.SelectedItem = item;
                    break;
                }
            }
        }

        /// <summary>
        /// Evento que valida el campo de Nombre Completo
        /// Solo permite letras, espacios y algunos caracteres especiales (acentos, ñ)
        /// NO permite números
        /// </summary>
        private void TxtNombreCompleto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Esta expresión regular SOLO permite letras (incluyendo acentos y ñ) y espacios
            Regex regex = new Regex("[^a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]");
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
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Construimos la cédula completa con el prefijo V-
            _estudianteOriginal.Cedula = "V-" + (_estudianteOriginal.CedulaNumeros ?? "").Trim();

            // Buscar el ComboBox por su nombre
            ComboBox cmbSeccion = (ComboBox)this.FindName("CmbSeccion");

            // Obtenemos el valor seleccionado del ComboBox
            if (cmbSeccion != null && cmbSeccion.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)cmbSeccion.SelectedItem;
                _estudianteOriginal.Seccion = selectedItem.Content.ToString();
            }

            // VALIDACIÓN 1: Verificar que no haya campos vacíos
            if (string.IsNullOrWhiteSpace(_estudianteOriginal.NombreCompleto) ||
                string.IsNullOrWhiteSpace(_estudianteOriginal.CedulaNumeros) ||
                string.IsNullOrWhiteSpace(_estudianteOriginal.Correo) ||
                string.IsNullOrWhiteSpace(_estudianteOriginal.Telefono) ||
                string.IsNullOrWhiteSpace(_estudianteOriginal.Seccion))
            {
                MessageBox.Show("¡Atención! Todos los campos deben estar completos.",
                    "Campos Incompletos",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // VALIDACIÓN 2: Verificar que el nombre no contenga números
            if (ContieneNumeros(_estudianteOriginal.NombreCompleto))
            {
                MessageBox.Show("El nombre completo no puede contener números.",
                    "Nombre Inválido",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // VALIDACIÓN 3: Verificar que el correo sea un Gmail válido
            if (!EsGmailValido(_estudianteOriginal.Correo))
            {
                MessageBox.Show("El correo electrónico debe ser una cuenta de Gmail válida.\nEjemplo: usuario@gmail.com",
                    "Correo Inválido",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // VALIDACIÓN 4: Verificar que la cédula venezolana sea válida
            if (!EsCedulaVenezolaanaValida(_estudianteOriginal.Cedula))
            {
                MessageBox.Show("La cédula debe tener entre 7 y 8 dígitos.\nEjemplo: V-12345678",
                    "Cédula Inválida",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // VALIDACIÓN 5: Verificar que el teléfono venezolano sea válido
            if (!EsTelefonoVenezolaanoValido(_estudianteOriginal.Telefono))
            {
                MessageBox.Show("El teléfono debe ser válido.\nFormato: 0412-1234567 o 04121234567\n(Debe empezar con 0412, 0414, 0416, 0424, 0426 para móviles)",
                    "Teléfono Inválido",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // VALIDACIÓN 6: Verificar que la sección sea una de las predeterminadas
            if (!EsSeccionValida(_estudianteOriginal.Seccion))
            {
                MessageBox.Show($"Debe seleccionar una sección válida: {string.Join(", ", SeccionesPredeterminadas)}",
                    "Sección Inválida",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // Si todas las validaciones pasan, limpiamos y formateamos los datos
            _estudianteOriginal.NombreCompleto = _estudianteOriginal.NombreCompleto.Trim();
            _estudianteOriginal.Cedula = _estudianteOriginal.Cedula.Trim().ToUpper();
            _estudianteOriginal.Correo = _estudianteOriginal.Correo.Trim().ToLower();
            _estudianteOriginal.Seccion = _estudianteOriginal.Seccion.Trim().ToUpper();

            // Confirmamos la edición
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
            cedula = cedula.Trim().ToUpper();
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
            telefono = telefono.Trim();
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
            string seccionLimpia = seccion.Trim().ToUpper();
            return SeccionesPredeterminadas.Any(s => s.ToUpper() == seccionLimpia);
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            // Cancelamos y cerramos
            this.DialogResult = false;
            this.Close();
        }

        // Para poder mover la ventana ya que tiene WindowStyle="None"
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}