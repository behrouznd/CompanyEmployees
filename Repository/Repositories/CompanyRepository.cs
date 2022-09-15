using Contracts.Interfaces;
using Entities.Models;

namespace Repository.Repositories
{
    public class CompanyRepository : RepositoryBase<Company> , ICompanyRepository
    {

        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public IEnumerable<Company> GetAllCompanies(bool trackChanges)=>
            FindAll(trackChanges)
                .OrderBy(c=> c.Name)
                .ToList();

        public Company GetCompany(Guid CompanyId, bool trackChanges)=>
            FindByCondition(c=>c.Id.Equals(CompanyId), trackChanges)
                .SingleOrDefault();

         
    }
}
