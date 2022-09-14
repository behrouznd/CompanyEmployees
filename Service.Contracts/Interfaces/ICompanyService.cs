using Shared.DataTransferObjects;

namespace Service.Contracts.Interfaces
{
    public interface ICompanyService
    {
        IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
    }
}
