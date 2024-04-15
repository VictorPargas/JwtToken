using JwtToken.Enum;
using System.ComponentModel.DataAnnotations;

namespace JwtToken.D
{
    public class UsuarioCriacao
    {
        [Required(ErrorMessage = "O campo Usuário é Obrigatório")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "O campo email é obrigatório"), EmailAddress(ErrorMessage = "Email Inválido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        public string Senha { get; set; }
        [Compare("Senha", ErrorMessage = "Senhas não coincidem!")]
        public string ConfirmarSenha { get; set; }
        [Required(ErrorMessage = "O campo Cargo é obrigatório")]
        public CargoEnum Cargo { get; set; }
    }
}
