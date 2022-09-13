using Contracts.Interfaces;
using Service.Contracts.Interfaces;
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

        public EmployeeService(IRepositoryManager repositoryManager,
            ILoggerManager loggerManager)
        {
            _repository= repositoryManager;
            _logger = loggerManager;
        }

        
    }
}
