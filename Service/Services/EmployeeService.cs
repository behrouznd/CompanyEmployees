using AutoMapper;
using Contracts.Interfaces;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts.Interfaces;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Services
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public EmployeeService(IRepositoryManager repositoryManager,
            ILoggerManager loggerManager,
            IMapper mapper)
        {
            _repository= repositoryManager;
            _logger = loggerManager;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync
            (Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);
            var employeesWithMetaData = await _repository.EmployeeRepository.GetEmployeesAsync(companyId, 
                employeeParameters, trackChanges);
            var employeesDto =  _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
            return (employees: employeesDto, metaData: employeesWithMetaData.MetaData);
        }


        public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee;
        }


        public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var employeeEntity = _mapper.Map<Employee>(employeeForCreation);
            _repository.EmployeeRepository.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repository.SaveAsync();
            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
            return employeeToReturn;
        }

        public async Task DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);
            var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

            _repository.EmployeeRepository.DeleteEmployee(employeeDb);
            await _repository.SaveAsync();
        }


        public async Task UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate,
            bool compTrackChanges, bool empTrackChanges)
        {
            await CheckIfCompanyExists(companyId, compTrackChanges);
            var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id,  empTrackChanges);

            _mapper.Map(employeeForUpdate, employeeDb);
            await _repository.SaveAsync();
        }

        public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync
            (Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            await CheckIfCompanyExists(companyId, compTrackChanges);
            var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id,  empTrackChanges);
            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeDb);
            return (employeeToPatch: employeeToPatch, employeeEntity: employeeDb);
        }

        public async void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repository.SaveAsync();
        }


        private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var company = await _repository.CompanyRepository.GetCompanyAsync(companyId,
            trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
        }

        private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists
            (Guid companyId, Guid id, bool trackChanges)
        {
            var employeeDb = await _repository.EmployeeRepository.GetEmployeeAsync(companyId, id,  trackChanges);
            if (employeeDb is null)
                throw new EmployeeNotFoundException(id);
            return employeeDb;
        }



    }
}
