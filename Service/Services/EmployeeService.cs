using AutoMapper;
using Contracts.Interfaces;
using Entities.Exceptions;
using Service.Contracts.Interfaces;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
