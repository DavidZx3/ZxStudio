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

        // 3. MATRÍCULA
        private string _matricula;
        public string Matricula
        {
            get { return _matricula; }
            set { _matricula = value; OnPropertyChanged(); }
        }

        // 4. CORREO
        private string _correo;
        public string Correo
        {
            get { return _correo; }
            set { _correo = value; OnPropertyChanged(); }
        }

        // 5. TELÉFONO (Agregado para corregir tu error)
        private string _telefono;
        public string Telefono
        {
            get { return _telefono; }
            set { _telefono = value; OnPropertyChanged(); }
        }

        // 6. SECCIÓN (Agregado para corregir tu error)
        private string _seccion;
        public string Seccion
        {
            get { return _seccion; }
            set { _seccion = value; OnPropertyChanged(); }
        }

        // EVENTO PARA NOTIFICAR CAMBIOS A LA INTERFAZ (WPF)
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        // En Estudiante.cs
        public void GenerarIniciales()
        {
            // ... lógica para calcular Iniciales a partir de NombreCompleto ...
        }
    }
}