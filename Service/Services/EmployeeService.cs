using AutoMapper;
using Contracts.Interfaces;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts.Interfaces;
using Shared.DataTransferObjects;

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

        public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
        {
            var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges);
            if(company is null)
                throw new CompanyNotFoundException(companyId);

            var employeesFromDb = _repository.EmployeeRepository.GetEmployees(companyId,trackChanges);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
            return employeesDto;

        }

        public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeDb = _repository.EmployeeRepository.GetEmployee(companyId, id, trackChanges);
            if (employeeDb is null)
                throw new EmployeeNotFoundException(id);
            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee;
        }


        public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
        {
            var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeEntity = _mapper.Map<Employee>(employeeForCreation);
            _repository.EmployeeRepository.CreateEmployeeForCompany(companyId, employeeEntity);
            _repository.Save();
            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
            return employeeToReturn;
        }

        public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeForCompany = _repository.EmployeeRepository.GetEmployee(companyId, id, trackChanges);
            if (employeeForCompany is null)
                throw new EmployeeNotFoundException(id);
            _repository.EmployeeRepository.DeleteEmployee(employeeForCompany);
            _repository.Save();
        }


        public void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate,
            bool compTrackChanges, bool empTrackChanges)
        {
            var company = _repository.CompanyRepository.GetCompany(companyId, compTrackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeEntity = _repository.EmployeeRepository.GetEmployee(companyId, id,
            empTrackChanges);
            if (employeeEntity is null)
                throw new EmployeeNotFoundException(id);
            _mapper.Map(employeeForUpdate, employeeEntity);
            _repository.Save();
        }

        public (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) GetEmployeeForPatch
            (Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            var company = _repository.CompanyRepository.GetCompany(companyId, compTrackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeEntity = _repository.EmployeeRepository.GetEmployee(companyId, id, empTrackChanges);
            if (employeeEntity is null)
                throw new EmployeeNotFoundException(companyId);
            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
            return (employeeToPatch, employeeEntity);
        }

        public void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            _repository.Save();
        }

    }
}
