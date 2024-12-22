using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Modelos.Modelos
{
    public class Estudiante
    {
        public string? Id { get; set; }
        public string? PNombre { get; set; }
        public string? SNombre { get; set; }
        public string? PApellido { get; set; }
        public string? SApellido { get; set; }
        public int Edad { get; set; }
        public string? CElectronico { get; set; }
        public Curso Curso { get; set; }
        public bool? Estado { get; set; }
        public string NombreCompleto => $"{PNombre} {SNombre} {PApellido} {SApellido}";
        public string EstadoTexto => Estado.HasValue ? (Estado.Value ? "Activo" : "Inactivo") : "No definido";
    }
}
