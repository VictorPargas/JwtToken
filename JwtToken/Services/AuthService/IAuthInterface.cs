using JwtToken.D;
using JwtToken.Models;

namespace JwtToken.Services.AuthService
{
    public interface IAuthInterface
    {
        //Interface é como se fosse um contrato Ex do metodo abaixo no controler que herda esse contrato.
        Task<Response<UsuarioCriacao>> Registrar(UsuarioCriacao usuarioRegistro);
    }
}
