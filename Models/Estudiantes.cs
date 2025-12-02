using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ZxStudio.Models
{
    public class Estudiante : INotifyPropertyChanged
    {
        // 1. INICIALES
        private string _iniciales;
        public string Iniciales
        {
            get { return _iniciales; }
            set { _iniciales = value; OnPropertyChanged(); }
        }

        // 2. NOMBRE COMPLETO
        private string _nombreCompleto;
        public string NombreCompleto
        {
            get { return _nombreCompleto; }
            set { _nombreCompleto = value; OnPropertyChanged(); }
        }

        // 3. CÉDULA/MATRÍCULA (Ajustado el nombre de la variable interna para claridad)
        private string _cedula;
        public string Cedula
        {
            get { return _cedula; }
            set { _cedula = value; OnPropertyChanged(); }
        }

        // 4. MATRÍCULA (Si necesitas un campo distinto a Cedula, si no, usa solo Cedula)
        private string _matricula;
        public string Matricula
        {
            get { return _matricula; }
            set { _matricula = value; OnPropertyChanged(); }
        }

        // 5. CORREO
        private string _correo;
        public string Correo
        {
            get { return _correo; }
            set { _correo = value; OnPropertyChanged(); }
        }

        // 6. TELÉFONO
        private string _telefono;
        public string Telefono
        {
            get { return _telefono; }
            set { _telefono = value; OnPropertyChanged(); }
        }

        // 7. SECCIÓN
        private string _seccion;
        public string Seccion
        {
            get { return _seccion; }
            set { _seccion = value; OnPropertyChanged(); }
        }

        // 8. MÉTODO REQUERIDO: Genera las iniciales a partir del nombre completo
        public void GenerarIniciales()
        {
            if (string.IsNullOrWhiteSpace(NombreCompleto))
            {
                Iniciales = "??";
                return;
            }

            var partesNombre = NombreCompleto.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (partesNombre.Length >= 2)
            {
                // Toma la inicial del primer y segundo nombre/apellido
                Iniciales = (partesNombre[0].Substring(0, 1) + partesNombre[1].Substring(0, 1)).ToUpper();
            }
            else if (partesNombre.Length == 1)
            {
                // Si solo hay una palabra, toma las dos primeras letras (si existen)
                Iniciales = partesNombre[0].Substring(0, Math.Min(2, partesNombre[0].Length)).ToUpper();
            }
            else
            {
                Iniciales = "??";
            }

            OnPropertyChanged(nameof(Iniciales));
        }


        // EVENTO PARA NOTIFICAR CAMBIOS A LA INTERFAZ (WPF)
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        // En tu archivo Estudiante.cs, agrega esta propiedad:
        public string CedulaNumeros { get; set; }
        
    }
}