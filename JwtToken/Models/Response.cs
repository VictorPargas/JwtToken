namespace JwtToken.Models
{
    public class Response<T>  //Eu consigo criar um generico que retorna qualquer tipo de modelo que eu quiser
    {
        public T? Dados { get; set; }
        public string Mensagem {  get; set; }

        public bool Status { get; set; } = true;
    }
}
