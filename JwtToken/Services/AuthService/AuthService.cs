using JwtToken.D;
using JwtToken.Data;
using JwtToken.Models;
using JwtToken.Services.SenhaService;
using Microsoft.EntityFrameworkCore;

namespace JwtToken.Services.AuthService
{
    public class AuthService : IAuthInterface
    {
        private readonly AppDbContext _context; //Var privada apenas para leitura
        private readonly ISenhaInterface _senhaInterface;
        public AuthService(AppDbContext context, ISenhaInterface senhaInterface)  //Construtor da classe
        {
            this._context = context;  //Agora consigo acessar o meu db
            _senhaInterface = senhaInterface;
        }

        public async Task<Response<UsuarioCriacao>> Registrar(UsuarioCriacao usuarioRegistro)
        {
            Response<UsuarioCriacao> respostaServico = new Response<UsuarioCriacao>();

            try
            {
                if (!VerificaSeEmailUsuarioJaExiste(usuarioRegistro))
                {
                    respostaServico.Dados = null;
                    respostaServico.Status = false;
                    respostaServico.Mensagem = "Email/Usuario já cadastrados!";
                    return respostaServico;
                }

                _senhaInterface.CriarSenhaHash(usuarioRegistro.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                UsuarioModel usuario = new UsuarioModel()
                {
                    Usuario = usuarioRegistro.Usuario,
                    Email = usuarioRegistro.Email,
                    Cargo = usuarioRegistro.Cargo,
                    SenhaHash = senhaHash,
                    SenhaSalt = senhaSalt
               };

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                respostaServico.Mensagem = "Usuario Criado com sucesso!";


            }
            catch (Exception ex)
            {
                respostaServico.Dados = null;
                respostaServico.Mensagem = ex.Message;
                respostaServico.Status = false;
            }

            return respostaServico;
        }

        public async Task<Response<string>> Login(UsuarioLoginDto usuarioLogin)
        {
            Response<string> respostaServico = new Response<string>();

            try
            {
                var usuario = await _context.Usuario.FirstOrDefaultAsync(userBanco => userBanco.Email == usuarioLogin.Email);

                if(usuario == null)
                {
                    respostaServico.Mensagem = "Credenciais Inválidas";
                    respostaServico.Status=false;
                    return respostaServico;
                }
                
                if (!_senhaInterface.VerificaSenhaHash(usuarioLogin.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                {
                    respostaServico.Mensagem = "Credenciais Inválidas";
                    respostaServico.Status = false;
                    return respostaServico;
                }

                var token = _senhaInterface.CriarToken(usuario);

                respostaServico.Dados = token;
                respostaServico.Mensagem = "Usuário logado com sucesso!";
            }
            catch(Exception ex)
            {
                respostaServico.Dados = null;
                respostaServico.Mensagem = ex.Message;
                respostaServico.Status = false;
            }

            return respostaServico;
        }
        public bool VerificaSeEmailUsuarioJaExiste(UsuarioCriacao usuarioRegistro)
        {
            //Aqui eu vou procurar o primeiro Registro Estou fazendo a comparação com o que tem no banco com o que eu estou enviando
            var usuario = _context.Usuario.FirstOrDefault(userBanco => userBanco.Email == usuarioRegistro.Email || userBanco.Usuario == usuarioRegistro.Usuario );

            if (usuario != null) return false;

            return true;
        }
    }
}
