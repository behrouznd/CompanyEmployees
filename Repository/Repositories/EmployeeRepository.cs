using Contracts.Interfaces;
using Entities.Models;

namespace Repository.Repositories
{
    public class EmployeeRepository : RepositoryBase<Employee> , IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {}

        public Employee GetEmployee(Guid companyId, Guid id, bool trackChanges)=>
            FindByCondition(x=> x.CompanyId.Equals(companyId) && x.Id.Equals(id),trackChanges)
                .OrderBy(x => x.Name)
                .SingleOrDefault();
         

        public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges)=>
            FindByCondition(x => x.CompanyId.Equals(companyId),trackChanges)
                .OrderBy(x => x.Name)
                .ToList();

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee) => Delete(employee);

    }
}
