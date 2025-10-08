using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using PV_NA_UsuariosRoles.Entities;
using PV_NA_UsuariosRoles.Repository;

/*
namespace PV_NA_UsuariosRoles.Services
{
    public class AuthService
    {
        private readonly TokenService _tokenService;
        private readonly TokenOptions _opts;
        private readonly UsuarioRepository _usuarioRepo;
        private readonly SesionRepository _sesionRepo;
        private readonly ILogger<AuthService> _logger; //Inyectar logger

        public AuthService(
            TokenService tokenService,
            IOptions<TokenOptions> opts,
            UsuarioRepository usuarioRepo,
            SesionRepository sesionRepo,
            ILogger<AuthService> logger)
        {
            _tokenService = tokenService;
            _opts = opts.Value;
            _usuarioRepo = usuarioRepo;
            _sesionRepo = sesionRepo;
            _logger = logger; //Inyectar logger
        }

        /// <summary>
        /// Autentica un usuario y genera tokens JWT
        /// </summary>
        public async Task<object?> LoginAsync(string email, string password)
        {
            var user = await _usuarioRepo.GetUsuarioByEmailAsync(email);
            if (user == null || user.Contrasena != password)
                return null;
            //USR1 (crear usuarios), ahí sí generas hashes BCrypt reales
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.ID_Usuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.ID_Rol.ToString())
            };

            var (accessToken, expiresAt) = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Eliminar sesiones previas del usuario
            await _sesionRepo.DeleteOldSessionsAsync(user.ID_Usuario);

            // Guardar nueva sesión en base de datos
            var sesion = new Sesion
            {
                ID_Usuario = user.ID_Usuario,
                Token_JWT = accessToken,
                Refresh_Token = refreshToken,
                Expira = DateTime.UtcNow.AddMinutes(_opts.RefreshTokenMinutes)
            };
            await _sesionRepo.SaveAsync(sesion);

            return new
            {
                expires_in = expiresAt,
                access_token = accessToken,
                refresh_token = refreshToken,
                usuarioID = user.ID_Usuario
            };
        }
        

        /// <summary>
        /// Renueva los tokens usando un refresh token válido
        /// </summary>
        public async Task<object?> RefreshAsync(string refreshToken)
        {
            var sesion = await _sesionRepo.GetByRefreshTokenAsync(refreshToken);
            if (sesion == null || sesion.Expira < DateTime.UtcNow)
                return null;

            var user = await _usuarioRepo.GetUsuarioByIdAsync(sesion.ID_Usuario);
            if (user == null) return null;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.ID_Usuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.ID_Rol.ToString())
            };

            var (accessToken, expiresAt) = _tokenService.GenerateAccessToken(claims);
            var newRefresh = _tokenService.GenerateRefreshToken();

            // Rotar tokens: eliminar sesión anterior y crear nueva
            await _sesionRepo.DeleteOldSessionsAsync(user.ID_Usuario);
            await _sesionRepo.SaveAsync(new Sesion
            {
                ID_Usuario = user.ID_Usuario,
                Token_JWT = accessToken,
                Refresh_Token = newRefresh,
                Expira = DateTime.UtcNow.AddMinutes(_opts.RefreshTokenMinutes)
            });

            return new
            {
                expires_in = expiresAt,
                access_token = accessToken,
                refresh_token = newRefresh,
                usuarioID = user.ID_Usuario
            };
        }

        /// <summary>
        /// Valida si un token JWT es válido
        /// </summary>
        public bool Validate(string token)
        {
            return _tokenService.ValidateToken(token);
        }
    }
}
*/


//USR1 (crear usuarios), ahí sí generas hashes BCrypt reales
/*
if (user == null || !VerificarContrasena(password, user.Contrasena))
{
    _logger.LogWarning("Intento de login fallido para usuario: {Email}", email);
    return null;
}

_logger.LogInformation("Login exitoso para usuario: {Email}, ID: {UserId}",
                      user.Email, user.ID_Usuario);
*/


using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using PV_NA_UsuariosRoles.Entities;
using PV_NA_UsuariosRoles.Repository;

namespace PV_NA_UsuariosRoles.Services
{
    public class AuthService
    {
        private readonly TokenService _tokenService;
        private readonly TokenOptions _opts;
        private readonly UsuarioRepository _usuarioRepo;
        private readonly SesionRepository _sesionRepo;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            TokenService tokenService,
            IOptions<TokenOptions> opts,
            UsuarioRepository usuarioRepo,
            SesionRepository sesionRepo,
            ILogger<AuthService> logger)
        {
            _tokenService = tokenService;
            _opts = opts.Value;
            _usuarioRepo = usuarioRepo;
            _sesionRepo = sesionRepo;
            _logger = logger;
        }

        /// <summary>
        /// Autentica un usuario y genera tokens JWT - CUMPLE REQUERIMIENTOS
        /// </summary>
        public async Task<object?> LoginAsync(string email, string password)
        {
            // ✅ VERIFICACIÓN DE CREDENCIALES (según requerimiento)
            var user = await _usuarioRepo.GetUsuarioByEmailAsync(email);
            if (user == null || user.Contrasena != password)
            {
                _logger.LogWarning("Intento de login fallido para usuario: {Email}", email);
                return null;
            }

            _logger.LogInformation("Login exitoso para usuario: {Email}, ID: {UserId}", user.Email, user.ID_Usuario);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.ID_Usuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.ID_Rol.ToString())
            };

            // ✅ TOKEN CON VALIDEZ PARAMETRIZABLE (5min por defecto)
            var (accessToken, expiresAt) = _tokenService.GenerateAccessToken(claims);

            // ✅ REFRESH TOKEN CON VALIDEZ CONFIGURABLE (superior al JWT)
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Eliminar sesiones previas del usuario
            await _sesionRepo.DeleteOldSessionsAsync(user.ID_Usuario);

            // Guardar nueva sesión en base de datos
            var sesion = new Sesion
            {
                ID_Usuario = user.ID_Usuario,
                Token_JWT = accessToken,
                Refresh_Token = refreshToken,
                Expira = DateTime.UtcNow.AddMinutes(_opts.RefreshTokenMinutes) // ✅ Configurable
            };
            await _sesionRepo.SaveAsync(sesion);

            // ✅ ESTRUCTURA EXACTA REQUERIDA POR EL ARQUITECTO
            return new
            {
                expires_in = expiresAt.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ"), // ✅ Formato ISO
                access_token = accessToken,
                refresh_token = refreshToken,
                usuarioID = user.ID_Usuario
            };
        }

        /// <summary>
        /// Renueva los tokens usando un refresh token válido - CUMPLE REQUERIMIENTOS
        /// </summary>
        public async Task<object?> RefreshAsync(string refreshToken)
        {
            var sesion = await _sesionRepo.GetByRefreshTokenAsync(refreshToken);
            if (sesion == null || sesion.Expira < DateTime.UtcNow)
            {
                _logger.LogWarning("Refresh token inválido o expirado");
                return null;
            }

            var user = await _usuarioRepo.GetUsuarioByIdAsync(sesion.ID_Usuario);
            if (user == null)
            {
                _logger.LogWarning("Usuario no encontrado para refresh token");
                return null;
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.ID_Usuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.ID_Rol.ToString())
            };

            var (accessToken, expiresAt) = _tokenService.GenerateAccessToken(claims);
            var newRefresh = _tokenService.GenerateRefreshToken();

            // Rotar tokens: eliminar sesión anterior y crear nueva
            await _sesionRepo.DeleteOldSessionsAsync(user.ID_Usuario);
            await _sesionRepo.SaveAsync(new Sesion
            {
                ID_Usuario = user.ID_Usuario,
                Token_JWT = accessToken,
                Refresh_Token = newRefresh,
                Expira = DateTime.UtcNow.AddMinutes(_opts.RefreshTokenMinutes)
            });

            _logger.LogInformation("Tokens refrescados para usuario ID: {UserId}", user.ID_Usuario);

            // ✅ ESTRUCTURA EXACTA PARA REFRESH (sin usuarioID según requerimiento)
            return new
            {
                expires_in = expiresAt.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ"),
                access_token = accessToken,
                refresh_token = newRefresh
                // ❌ NOTA: El arquitecto NO pide usuarioID en refresh, solo en login
            };
        }

        /// <summary>
        /// Valida si un token JWT es válido - CUMPLE REQUERIMIENTOS
        /// </summary>
        public bool Validate(string token)
        {
            try
            {
                bool isValid = _tokenService.ValidateToken(token);
                _logger.LogDebug("Validación de token: {IsValid}", isValid);
                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando token JWT");
                return false;
            }
        }
    }
}