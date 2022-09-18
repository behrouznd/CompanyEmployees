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

        

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
        {
            //try
            //{
                var companies = await _repository.CompanyRepository.GetAllCompaniesAsync(trackChanges);
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

        public async Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges)
        {
            var companyEntity = await GetCompanyAndCheckIfItExists(id, trackChanges);


            var companyDto = _mapper.Map<CompanyDto>(companyEntity);

            return companyDto;
        }

        public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);

            _repository.CompanyRepository.CreateCompany(companyEntity);
            await _repository.SaveAsync();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return companyToReturn;
        }


        public async Task< IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();
            var companyEntities = await _repository.CompanyRepository.GetByIdsAsync(ids, trackChanges);
            if (ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return companiesToReturn;
        }



        public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollection
                (IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.CompanyRepository.CreateCompany(company);
            }
            await _repository.SaveAsync();
            var companyCollectionToReturn =
                _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
            return (companies: companyCollectionToReturn, ids: ids);
        }


        public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
        {
            var companyEntity = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            _repository.CompanyRepository.DeleteCompany(companyEntity);
            await _repository.SaveAsync();
        }

        public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            var companyEntity = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            _mapper.Map(companyForUpdate, companyEntity);
            await _repository.SaveAsync();
        }


        private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var company = await _repository.CompanyRepository.GetCompanyAsync(id, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(id);
            return company;
        }

        public Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            throw new NotImplementedException();
        }

        
 
    }

}
