using JwtToken.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtToken.Services.SenhaService
{
    public class SenhaService : ISenhaInterface
    {
        private readonly IConfiguration _config;

        public SenhaService(IConfiguration config) 
        {
            _config = config;
        }
        //Ele nao retorna nada no Void mais ele retorna o OutByte     
        public void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt)
        {
            //Quando eu uso using eu nao tenho a obrigatoridade de usar um dispose (fechar).
            using (var hmac = new HMACSHA512())
            {
                senhaSalt = hmac.Key; //Chave para conseguir criar a senha hash
                senhaHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha)); //Senha criptografada ele é criado com base no Salt 
            } 
        }

        public bool VerificaSenhaHash(string senha, byte[] senhaHash, byte[] senhaSalt)
        {
            using (var hmac = new HMACSHA512(senhaSalt))
            {
                //Esse hash tem que ser igual ao hash que está no banco  se a senha estiver correta
                var computerHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
                return computerHash.SequenceEqual(senhaHash);
            }
        }

        public string CriarToken(UsuarioModel usuario)
        {
            //Clains é as informaçoes que nos iremos salvar as informações que está dentro do token
            List<Claim> claims = new List<Claim>()
            {
                new Claim("Cargo", usuario.Cargo.ToString()),
                new Claim("Email", usuario.Email),
                new Claim("Username", usuario.Usuario)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);


            return jwt;
        }
    }
}
