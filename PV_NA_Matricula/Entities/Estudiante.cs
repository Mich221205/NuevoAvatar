namespace PV_NA_Matricula.Entities
{
    public class Estudiante
    {
        public int ID_Estudiante { get; set; }
        public string Identificacion { get; set; }
        public string Tipo_Identificacion { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
    }
}
