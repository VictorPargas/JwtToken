using JwtToken.Data;
using JwtToken.Models;

namespace JwtToken.Services.ProfessorService
{
    public class ProfessorService : IProfessorService
    {

        private readonly AppDbContext _dbContext;
        private readonly IProfessorService _professorService;

        public ProfessorService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
