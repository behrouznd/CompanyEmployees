using Entities.Models;

namespace Contracts.Interfaces
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges);
        Task<Company> GetCompanyAsync(Guid CompanyId , bool trackChanges);
        void CreateCompany(Company company);

        Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        void DeleteCompany(Company company);
    }
}
