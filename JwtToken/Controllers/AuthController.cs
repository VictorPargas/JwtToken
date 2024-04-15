using JwtToken.D;
using JwtToken.Services.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface _authInterface;

        public AuthController(IAuthInterface authInterface)
        {
            _authInterface = authInterface;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(UsuarioLoginDto usuarioLogin)
        {    
            var resposta = await _authInterface.Login(usuarioLogin);
            return Ok(resposta);
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(UsuarioCriacao usuarioRegister)
        {
           var resposta = await _authInterface.Registrar(usuarioRegister);
            return Ok(resposta);
        }
    }
}
