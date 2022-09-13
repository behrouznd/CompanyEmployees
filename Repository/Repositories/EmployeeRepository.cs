﻿using Contracts.Interfaces;
using Entities.Models;

namespace Repository.Repositories
{
    public class EmployeeRepository : RepositoryBase<Employee> , IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
