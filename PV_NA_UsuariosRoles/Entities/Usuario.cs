namespace PV_NA_UsuariosRoles.Entities
{
    public class Usuario
    {
        public int ID_Usuario { get; set; }
        public string Email { get; set; }
        public string Tipo_Identificacion { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Contrasena { get; set; }
        public int ID_Rol { get; set; }
    }
}

