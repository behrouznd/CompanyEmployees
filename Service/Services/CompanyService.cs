using AutoMapper;
using Contracts.Interfaces;
using Entities.Models;
using Service.Contracts.Interfaces;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CompanyService(IRepositoryManager repositoryManager,
            ILoggerManager loggerManager,
            IMapper mapper)
        {
            _logger = loggerManager;
            _repository = repositoryManager; 
            _mapper = mapper;
        }

        public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
        {
            //try
            //{
                var companies = _repository.CompanyRepository.GetAllCompanies(trackChanges);
                //var conpaniesDto = companies.Select(c =>
                //     new CompanyDto(c.Id, c.Name ?? "", String.Join(' ', c.Country, c.Address))).ToList();

                var conpaniesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

                return conpaniesDto;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError($"Something went wrong in the {nameof(GetAllCompanies)} service method {ex}");

            //    throw;
            //}
        }
    }
}
