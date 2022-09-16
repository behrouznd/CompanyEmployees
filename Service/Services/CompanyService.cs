using AutoMapper;
using Contracts.Interfaces;
using Entities.Exceptions;
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

                var conpaniesDto = _mapper.Map<IEnumerable<Shared.DataTransferObjects.CompanyDto>>(companies);

                return conpaniesDto;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError($"Something went wrong in the {nameof(GetAllCompanies)} service method {ex}");

            //    throw;
            //}
        }

        public CompanyDto GetCompany(Guid id, bool trackChanges)
        {
            var company = _repository.CompanyRepository.GetCompany(id, trackChanges);
            if (company == null)
                throw new CompanyNotFoundException(id);

            var companyDto = _mapper.Map<CompanyDto>(company);

            return companyDto;
        }

        public CompanyDto CreateCompany(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);

            _repository.CompanyRepository.CreateCompany(companyEntity);
            _repository.Save();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return companyToReturn;
        }


        public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();
            var companyEntities = _repository.CompanyRepository.GetByIds(ids, trackChanges);
            if (ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return companiesToReturn;
        }



        public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection
                (IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.CompanyRepository.CreateCompany(company);
            }
            _repository.Save();
            var companyCollectionToReturn =
                _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
            return (companies: companyCollectionToReturn, ids: ids);
        }


        public void DeleteCompany(Guid companyId, bool trackChanges)
        {
            var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            _repository.CompanyRepository.DeleteCompany(company);
            _repository.Save();
        }

        public void UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            var companyEntity = _repository.CompanyRepository.GetCompany(companyId, trackChanges);
            if (companyEntity is null)
                throw new CompanyNotFoundException(companyId);
            _mapper.Map(companyForUpdate, companyEntity);
            _repository.Save();
        }

    }

}
