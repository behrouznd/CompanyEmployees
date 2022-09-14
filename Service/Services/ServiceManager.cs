﻿using AutoMapper;
using Contracts.Interfaces;
using Service.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICompanyService> _companyService;
        private readonly Lazy<IEmployeeService> _employeeService;

        public ServiceManager(IRepositoryManager repositoryManager,
            ILoggerManager loggerManager,
            IMapper mapper)
        {
            _companyService = new Lazy<ICompanyService>(()=> 
                new CompanyService(repositoryManager, loggerManager , mapper));
            
            _employeeService = new Lazy<IEmployeeService>(()=>
                new EmployeeService(repositoryManager, loggerManager , mapper));
        }

        public ICompanyService CompanyService => _companyService.Value;

        public IEmployeeService EmployeeService => _employeeService.Value;
    }
}