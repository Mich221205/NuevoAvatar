namespace PV_NA_Reportes.Entities
{
    public class Reporte
    {
        public int ID_Reporte { get; set; }
        public string Tipo { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaGeneracion { get; set; }
    }
}
