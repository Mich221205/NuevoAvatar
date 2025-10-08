namespace PV_NA_UsuariosRoles.Entities
{
    public class TokenOptions
    {

        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Secret { get; set; } = null!;
        public int AccessTokenMinutes { get; set; } = 5;
        public int RefreshTokenMinutes { get; set; } = 15;

    }
}
