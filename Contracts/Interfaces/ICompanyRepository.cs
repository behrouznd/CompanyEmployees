using Entities.Models;

namespace Contracts.Interfaces
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies(bool trackChanges);
        Company GetCompany(Guid CompanyId , bool trackChanges);

    }
}
