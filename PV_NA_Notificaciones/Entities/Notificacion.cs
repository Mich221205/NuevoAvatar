namespace PV_NA_Notificaciones.Entities
{
    public class Notificacion
    {
        public int ID_Notificacion { get; set; }
        public string Email_Destino { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public DateTime FechaEnvio { get; set; }
        public string Estado { get; set; }
    }
}
