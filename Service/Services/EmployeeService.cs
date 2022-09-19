using AutoMapper;
using Contracts.Interfaces;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Service.Contracts.Interfaces;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Dynamic;

namespace Service.Services
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IEmployeeLinks _employeeLinks;

        public EmployeeService(IRepositoryManager repositoryManager,
            ILoggerManager loggerManager,
            IMapper mapper,
            IEmployeeLinks employeeLinks)
        {
            _repository= repositoryManager;
            _logger = loggerManager;
            _mapper = mapper;
            _employeeLinks = employeeLinks;
        }
        
        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync
        (Guid companyId, LinkParameters linkParameters, bool trackChanges)
        {
            if (!linkParameters.EmployeeParameters.ValidAgeRange)
                throw new MaxAgeRangeBadRequestException();

            await CheckIfCompanyExists(companyId, trackChanges);

            var employeesWithMetaData = await _repository.EmployeeRepository
                .GetEmployeesAsync(companyId, linkParameters.EmployeeParameters, trackChanges);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
            var links = _employeeLinks.TryGenerateLinks(employeesDto, linkParameters.EmployeeParameters.Fields,
                companyId, linkParameters.Context);

            return (linkResponse: links, metaData: employeesWithMetaData.MetaData);
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
