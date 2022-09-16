using Contracts.Interfaces;
using Entities.Models;

namespace Repository.Repositories
{
    public class CompanyRepository : RepositoryBase<Company> , ICompanyRepository
    {

        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {}


        public IEnumerable<Company> GetAllCompanies(bool trackChanges)=>
            FindAll(trackChanges)
                .OrderBy(c=> c.Name)
                .ToList();

        public Company GetCompany(Guid CompanyId, bool trackChanges)=>
            FindByCondition(c=>c.Id.Equals(CompanyId), trackChanges)
                .SingleOrDefault();

        public void CreateCompany(Company company) => Create(company);

        public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
             FindByCondition(x => ids.Contains(x.Id), trackChanges)
             .ToList();

        public void DeleteCompany(Company company) => Delete(company);
    }
}
