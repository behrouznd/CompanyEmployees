using Entities.LinkModels;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Dynamic;

namespace Service.Contracts.Interfaces
{
    public interface IEmployeeService
    {
        Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync
            (Guid companyId, LinkParameters employeeParameters, bool trackChanges);

 


        Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);

        Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges);

        Task DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges);

        Task UpdateEmployeeForCompany(Guid companyId, Guid id,
            EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges);

        Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(
            Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges);

        void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee
            employeeEntity);
    }
}
