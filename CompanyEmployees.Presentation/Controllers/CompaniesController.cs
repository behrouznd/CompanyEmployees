using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController :ControllerBase
    {
        private readonly IServiceManager _service;
        public CompaniesController(IServiceManager serviceManager) => _service = serviceManager;
         
        [HttpGet]
        public IActionResult GetCompanies()
        {
            //try
            //{
            //throw new Exception("Exception");

                var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);
                return Ok(companies);
            //}
            //catch (Exception ex)
            //{

            //    return StatusCode(500, "Internal server error");
            //}
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetCompany(Guid id)
        {
            var company = _service.CompanyService.GetCompany(id, false);
            
            return Ok(company);
        }
    }
}
