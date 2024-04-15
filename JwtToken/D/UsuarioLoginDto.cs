using System.ComponentModel.DataAnnotations;

namespace JwtToken.D
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "O campo email é obrigatório"), EmailAddress(ErrorMessage = "Email Inválido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        public string Senha { get; set; }
    }
}
