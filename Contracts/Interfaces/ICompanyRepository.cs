using Entities.Models;

namespace Contracts.Interfaces
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies(bool trackChanges);
        Company GetCompany(Guid CompanyId , bool trackChanges);
        void CreateCompany(Company company);

        IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

        void DeleteCompany(Company company);
    }
}
