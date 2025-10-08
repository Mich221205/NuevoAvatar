using Microsoft.AspNetCore.Mvc;
using PV_NA_UsuariosRoles.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
//using PV_NA_UsuariosRoles.DTOs;
using PV_NA_UsuariosRoles.Services;


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PV_NA_UsuariosRoles.Services;
using System.ComponentModel.DataAnnotations;
/*
namespace PV_NA_UsuariosRoles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AuthService _auth;
        public LoginController(AuthService auth) => _auth = auth;

        // POST /login -> Body
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var token = await _auth.LoginAsync(req.Username, req.Password);
            return token is null ? Unauthorized(new { error = "Credenciales inválidas" }) : Ok(token);
        }

        // GET /login/validate?token=... -> Query (NO body)
        [HttpGet("validate")]
        [AllowAnonymous]
        public IActionResult Validate([FromQuery, Required] string token)
        {
            var ok = _auth.Validate(token);
            return ok ? Ok(new { valid = true }) : Unauthorized(new { valid = false });
        }

        // POST /login/refresh -> Body
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest req)
        {
            var token = await _auth.RefreshAsync(req.Refresh_Token);
            return token is null ? Unauthorized(new { error = "Refresh inválido" }) : Ok(token);
        }
    }

    // DTOs para claridad
    public record LoginRequest(string Username, string Password);
    public record RefreshRequest([property: Required] string Refresh_Token);
}
*/

namespace PV_NA_UsuariosRoles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(AuthService authService, ILogger<LoginController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint para iniciar sesión - REQUERIMIENTO PRINCIPAL
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromHeader(Name = "usuario")] string usuario,
            [FromHeader(Name = "contrasena")] string contrasena)
        {
            //  Todos los datos son requeridos y no pueden ser nulos o blancos
            if (string.IsNullOrWhiteSpace(usuario))
                return BadRequest(new { message = "El encabezado 'usuario' es requerido." });

            if (string.IsNullOrWhiteSpace(contrasena))
                return BadRequest(new { message = "El encabezado 'contrasena' es requerido." });

            try
            {
                // 
                var result = await _authService.LoginAsync(usuario, contrasena);

                //  Si falla → 401 con mensaje exacto
                if (result == null)
                    return Unauthorized(new { message = "Usuario y/o contraseña incorrectos." });

                
                return StatusCode(StatusCodes.Status201Created, new
                {
                    expires_in = result,        // horaVencimiento
                    access_token = result,    // jwtToken  
                    refresh_token = result,  // refreshToken
                    usuarioID = result           // identificador
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en login para usuario: {Usuario}", usuario);
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        /// <summary>
        /// Endpoint para refrescar token - REQUERIMIENTO SECUNDARIO
        /// </summary>
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            // 
            if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest(new { message = "El refresh token es requerido." });

            try
            {
                var result = await _authService.RefreshAsync(request.RefreshToken);

               
                if (result == null)
                    return Unauthorized(new { message = "No autorizado." });

             
                return StatusCode(StatusCodes.Status201Created, new
                {
                    expires_in = result,        // horaVencimiento
                    access_token = result,    // jwtToken
                    refresh_token = result   // refreshToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en refresh token");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        /// <summary>
        /// Endpoint para validar token - REQUERIMIENTO SECUNDARIO
        /// </summary>
        [HttpGet("validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Validate([FromQuery] string token)
        {
            //  Token requerido
            if (string.IsNullOrWhiteSpace(token))
                return BadRequest(new { message = "El token es requerido." });

            try
            {
                bool valido = _authService.Validate(token);

                // Válido → 200 + true, Inválido → 401
                if (!valido)
                    return Unauthorized(); // 401 sin mensaje (según requerimiento)

               
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando token");
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }
    }

    /// <summary>
    /// DTO para refresh request
    /// </summary>
    public class RefreshRequest
    {
        public string RefreshToken { get; set; }
    }
}