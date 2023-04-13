using JobInterviewProject.Infrastructure.Database.Repository;

namespace JobInterviewProject.Infrastructure.Database.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        public IAuthRepository AuthRepository { get; init; }
        public IEmployeesRepository EmployeesRepository { get; init; }

        public UnitOfWork(AppDbContext context, IAuthRepository authRepository, IEmployeesRepository employeesRepository)
        {
            _context = context;
            AuthRepository = authRepository;
            EmployeesRepository = employeesRepository;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
