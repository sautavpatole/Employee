using AutoMapper;
using EmployePortal.Authentication;
using EmployePortal.Models;
using EmployePortal.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EmployePortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public readonly ApplicationDBContext _dbContext;

        public readonly IMapper _mapper;

        public readonly ILogger<Employee> _logger;

        private readonly IApiKeyValidation _apiKeyValidator;
        public EmployeeController(ApplicationDBContext applicationDBContext, IMapper mapper, ILogger<Employee> logger, IApiKeyValidation apiKeyValidator)
        {
            _dbContext = applicationDBContext;
            _mapper = mapper;
            _logger = logger;
            _apiKeyValidator = apiKeyValidator;
        }

        [HttpGet("GetEmployeeDetails")]
        public IActionResult GetEmployee()
        {
            _logger.LogInformation("Received request to fetch employe details");
            var employee = _dbContext.Employees.ToList();
            return Ok(employee);
        }

        [HttpPost("GetEmployeeDetails")]
        public IActionResult AddEmployee(EmployeeDTO employeeDTO)
        {
            var apiKey = Request.Headers[Constants.Constants.ApiKeyHeaderName].FirstOrDefault();
            if (!_apiKeyValidator.IsValidApiKey(apiKey))
            {
                _logger.LogError("Invalid API Key");
                return StatusCode(StatusCodes.Status401Unauthorized, "Invalid API Key");
            }
            try
            {
                _logger.LogInformation("Received request to add the Employee");
                var employeEntity = _mapper.Map<Employee>(employeeDTO);


                _dbContext.Employees.Add(employeEntity);
                _dbContext.SaveChanges();
                return Ok(employeEntity);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "An Error Occurred {Message}", exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,exception.Message);
            }
        }

        [HttpPut]
        [Route(("{id:guid}"))]
        public IActionResult UpdateEmployeDetails( Guid id, UpdateEmployeeDTO updateEmployeeDTO)
        {
            try
            {
                var employeeId = _dbContext.Employees.Find(id);

                if(employeeId is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                _mapper.Map<Employee>(updateEmployeeDTO);

                _dbContext.SaveChanges();

                return Ok(employeeId);

            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "An Error Occurred {Message}", exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            try
            {
                var getEmployeeDetails = _dbContext.Employees.Find(id);
                if(getEmployeeDetails is null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                _dbContext.Employees.Remove(getEmployeeDetails);
                _dbContext.SaveChanges();
                return Ok();

            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "An Error Occurred {Message}", exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}