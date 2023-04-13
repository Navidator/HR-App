using JobInterviewProject.Infrastructure.Database.Repository;

namespace JobInterviewProject.Infrastructure.Database.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IAuthRepository AuthRepository { get; init; }
        public IEmployeesRepository EmployeesRepository { get; init; }
        Task<int> Complete();
    }
}
